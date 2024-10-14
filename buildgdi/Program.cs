using System;
using System.IO;
using DiscUtils.Gdrom;
using System.Collections.Generic;

namespace buildgdi
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                PrintUsage();
                return;
            }
            string data = GetSoloArgument("-data", args);
            string ipBin = GetSoloArgument("-ip", args);
            List<string> outPath = GetMultiArgument("-output", args);
            List<string> cdda = GetMultiArgument("-cdda", args);
            string gdiPath = GetSoloArgument("-gdi", args);
            string volume = GetSoloArgument("-V", args);
            string buildDate = GetSoloArgument("-date", args);
            bool rebuild = HasArgument("-rebuild", args);
            bool extract = HasArgument("-extract", args);
            bool truncate = HasArgument("-truncate", args);
            bool useIsoSectors = HasArgument("-iso", args);
            if (CheckArguments(extract, rebuild, data, ipBin, gdiPath, outPath, cdda, truncate, buildDate, 
                out bool fileOutput, out DateTime? builtTime) == false)
            {
                return;
            }
            if (extract)
            {
                Console.WriteLine("Beginn der Extraktion");
                Extract(gdiPath, ipBin, outPath);
                Console.WriteLine("Erledigt!");
            }
            else if (rebuild)
            {
                Rebuild(gdiPath, data, ipBin, cdda, outPath, volume, useIsoSectors, truncate, builtTime);
            }
            else
            {
                BuildDisc(ipBin, cdda, useIsoSectors, truncate, builtTime, data, volume, fileOutput, outPath, gdiPath);
            }
        }

        private static void Extract(string gdiPath, string ipBin, List<string> outPath)
        {
            using (GDReader reader = GDReader.FromGDIfile(gdiPath))
            {
                if (!string.IsNullOrEmpty(ipBin))
                {
                    if (string.IsNullOrEmpty(Path.GetExtension(ipBin)))
                    {
                        ipBin = Path.Combine(ipBin, "IP.BIN");
                    }
                    if (!Directory.Exists(Path.GetDirectoryName(ipBin)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(ipBin));
                    }
                    Console.WriteLine("IP.BIN extrahieren");
                    using (Stream input = reader.ReadIPBin())
                    {
                        using (FileStream output = new FileStream(ipBin, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                        {
                            TransferStreams(input, output);
                        }
                    }
                }
                if (!Directory.Exists(outPath[0]))
                {
                    Directory.CreateDirectory(outPath[0]);
                }
                ExtractFolder(reader, "", outPath[0]);
            }
        }

        private static void ExtractFolder(GDReader reader, string folder, string toPath)
        {
            string[] files = reader.GetFiles(folder);
            foreach (var file in files)
            {
                string filePath = file;
                if (filePath.StartsWith("\\"))
                {
                    filePath = filePath.Substring(1); //I don't want the leading slash, it breaks things;
                }
                string destPath = Path.Combine(toPath, filePath);
                Console.WriteLine($"Extrahieren {filePath}");
                using (Stream input = reader.OpenFile(file, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream output = new FileStream(destPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        TransferStreams(input, output);
                    }
                }
                File.SetCreationTimeUtc(destPath, reader.GetCreationTimeUtc(file));
                File.SetLastWriteTimeUtc(destPath, reader.GetLastWriteTimeUtc(file));
            }
            string[] folders = reader.GetDirectories(folder);
            foreach (string subfolder in folders)
            {
                string folderPath = subfolder;
                if (folderPath.StartsWith("\\"))
                {
                    folderPath = folderPath.Substring(1);
                }
                string destPath = Path.Combine(toPath, folderPath);
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                    Directory.SetCreationTimeUtc(destPath, reader.GetCreationTimeUtc(subfolder));
                    Directory.SetLastWriteTimeUtc(destPath, reader.GetLastWriteTimeUtc(subfolder));
                }
                ExtractFolder(reader, folderPath, toPath);
            }
        }

        private static void TransferStreams(Stream input, Stream output)
        {
            byte[] buffer = new byte[8192];
            int bytesRead = 0;
            long bytesLeft = input.Length;
            do
            {
                bytesRead = input.Read(buffer, 0, (int)Math.Min(bytesLeft, buffer.Length));
                output.Write(buffer, 0, bytesRead);
                bytesLeft -= bytesRead;
            } while (bytesLeft > 0 && bytesRead > 0);
        }

        private static void Rebuild(string gdiPath, string data, string ipBin, List<string> cdda, List<string> outPath, 
            string volume, bool useIsoSectors, bool truncate, DateTime? buildDate)
        {
            string gdiDirectory = Path.GetDirectoryName(gdiPath);
            string[] gdiLines = File.ReadAllText(gdiPath).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (cdda.Count == 0 && gdiLines.Length > 3 && int.TryParse(gdiLines[0], out int numTracks) && numTracks > 3)
            {
                //Original had CDDA and user didn't override, let's copy them
                for (int i = 4; i < numTracks && i < gdiLines.Length; i++)
                {
                    string track = ParseGdiTrackPath(gdiLines[i], gdiDirectory);
                    if (!File.Exists(track))
                    {
                        Console.WriteLine($"FEHLER: Der CDDA Track {Path.GetFileName(track)}, auf den in der ursprünglichen .GDI Datei verwiesen wird, kann nicht gefunden werden");
                        return;
                    }
                    cdda.Add(track);
                }
            }
            
            using (GDReader reader = GDReader.FromGDItext(gdiLines, gdiDirectory))
            {
                GDromBuilder builder;
                if (!string.IsNullOrEmpty(ipBin) && File.Exists(ipBin))
                {
                    builder = new GDromBuilder(ipBin, cdda);
                }
                else
                {
                    builder = new GDromBuilder(reader.ReadIPBin(), cdda);
                }
                builder.ReportProgress += ProgressReport;
                builder.RawMode = !useIsoSectors;
                builder.TruncateData = truncate;
                builder.BuildDate = buildDate;
                if (volume != null)
                {
                    builder.VolumeIdentifier = volume;
                }
                Console.Write("Schreiben");

                builder.ImportReader(reader);
                builder.ImportFolder(data, "", true);
                if (!Directory.Exists(outPath[0]))
                {
                    Directory.CreateDirectory(outPath[0]);
                }
                //Copy the PC tracks first
                for (int i = 1; i <= 2; i++)
                {
                    string srcPath = ParseGdiTrackPath(gdiLines[i], gdiDirectory);
                    if (File.Exists(srcPath))
                    {
                        File.Copy(srcPath, Path.Combine(outPath[0], Path.GetFileName(srcPath)), true);
                    }
                }
                //Next copy the CDDA
                foreach (string track in cdda)
                {
                    //We have already checked that all of these exist
                    File.Copy(track, Path.Combine(outPath[0], Path.GetFileName(track)), true);
                }
                //Next, save the data track(s)
                List<DiscTrack> tracks = builder.BuildGDROM(outPath[0]);
                //Finally, save the .gdi file
                builder.WriteGdiFile(gdiLines, tracks, Path.Combine(outPath[0], "disc.gdi"));
                Console.WriteLine(" Erledigt!");
            }
        }

        private static string ParseGdiTrackPath(string trackInfo, string sourceDir)
        {
            string[] pieces = trackInfo.Split(' ');
            if (pieces.Length == 6)
            {
                return Path.Combine(sourceDir, pieces[4]);
            }
            return null;
        }

        private static void BuildDisc(string ipBin, List<string> cdda, bool useIsoSectors, bool truncate, DateTime? buildDate,
            string data, string volume, bool fileOutput, List<string> outPath, string gdiPath)
        {
            GDromBuilder builder = new GDromBuilder(ipBin, cdda);
            builder.ReportProgress += ProgressReport;
            builder.RawMode = !useIsoSectors;
            builder.TruncateData = truncate;
            builder.BuildDate = buildDate;
            builder.ImportFolder(data);
            if (volume != null)
            {
                builder.VolumeIdentifier = volume;
            }
            Console.Write("Schreiben");
            List<DiscTrack> tracks = null;
            if (fileOutput)
            {
                builder.Track03Path = Path.GetFullPath(outPath[0]);
                if (outPath.Count == 2 && (cdda.Count > 0 || builder.TruncateData))
                {
                    builder.LastTrackPath = Path.GetFullPath(outPath[1]);
                }
                tracks = builder.BuildGDROM();
            }
            else
            {
                tracks = builder.BuildGDROM(outPath[0]);
            }
            Console.WriteLine(" Erledigt!");
            if (gdiPath != null)
            {
                builder.UpdateGdiFile(tracks, gdiPath);
            }
            else
            {
                Console.WriteLine(builder.GetGDIText(tracks));
            }
        }

        private static void ProgressReport(int amount)
        {
            if (amount % 10 == 0)
            {
                Console.Write('.');
            }
        }

        private static bool CheckArguments(bool extracting, bool rebuild, string data, string ipBin, string gdiPath,
            List<string> outPath, List<string> cdda, bool truncate, string buildDate, out bool fileOutput, out DateTime? builtTime)
        {
            fileOutput = false;
            builtTime = null;
            if (extracting || rebuild)
            {
                if (string.IsNullOrEmpty(gdiPath) || !File.Exists(gdiPath) || !Path.GetExtension(gdiPath).ToLower().Equals(".gdi"))
                {
                    Console.WriteLine($"Eine .GDI Datei ist erforderlich {(extracting ? "extract" : "rebuild")} Mode");
                    return false;
                }
            }
            else
            {
                //User wants to build a new high density data track from scratch
                if (gdiPath != null && !File.Exists(gdiPath))
                {
                    Console.WriteLine("Die angegebene .GDI Datei existiert nicht.");
                    return false;
                }
            }
            if (extracting)
            {
                if (outPath.Count != 1)
                {
                    Console.WriteLine("Für die Extraktion ist nur ein Ausgabepfad zulässig");
                    return false;
                }
                else if (!string.IsNullOrEmpty(Path.GetExtension(outPath[0])))
                {
                    Console.WriteLine("Die Extraktions Ausgabe muss ein Ordner sein, nicht eine Datei!");
                    return false;
                }
                return true; //This is all we need to check for extraction
            }
            if (!string.IsNullOrEmpty(buildDate))
            {
                if (DateTime.TryParse(buildDate, out DateTime parsedDate))
                {
                    builtTime = parsedDate;
                }
                else
                {
                    Console.WriteLine("Das Argument für das benutzerdefinierte Datum wurde nicht verstanden. Bitte versuchen Sie, das Format zu verwenden: JJJJ-MM-tt hh:mm:ss");
                    return false;
                }
            }
            if (data == null || (!rebuild && ipBin == null) || outPath.Count == 0)
            {
                Console.WriteLine("Die erforderlichen Felder sind nicht ausgefüllt worden.");
                return false;
            }
            if (!Directory.Exists(data))
            {
                Console.WriteLine("Das angegebene Datenverzeichnis existiert nicht!");
                return false;
            }
            if (ipBin != null && !File.Exists(ipBin))
            {
                //Rebuild mode will use the existing IP.BIN, unless you specify one to replace it
                Console.WriteLine("Die angegebene IP.BIN Datei existiert nicht!");
                return false;
            }
            foreach (string track in cdda)
            {
                if (!File.Exists(track))
                {
                    Console.WriteLine("Den CDDA Track " + track + " gibt es nicht!");
                    return false;
                }
            }
            if (rebuild)
            {
                if (outPath.Count == 1)
                {
                    string path = outPath[0];
                    if (path.EndsWith(Path.DirectorySeparatorChar.ToString()) || !Path.HasExtension(path))
                    {
                        if (Path.GetDirectoryName(gdiPath).Equals(Path.GetDirectoryName(path)))
                        {
                            Console.WriteLine("Es kann nicht die gleiche GDI wie die Eingabe neu erstellt werden. Das würde den Datenträger, von dem wir Dateien lesen, überschreiben!");
                            return false;
                        }
                        fileOutput = false;
                    }
                    else
                    {
                        Console.WriteLine("Der Rebuild Modus erfordert ein Verzeichnis zur Ausgabe der neuen GDI!");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Der Rebuild Modus akzeptiert nur ein einzelnes Verzeichnis als Ausgabepfad, der nicht mit dem Eingabepfad übereinstimmen darf.");
                    return false;
                }
            }
            else if (outPath.Count > 2)
            {
                Console.WriteLine("Zu viele Ausgabepfade angegeben.");
                return false;
            }
            else if (outPath.Count == 2)
            {
                fileOutput = true;
                if (!Path.HasExtension(outPath[0]) || !Path.HasExtension(outPath[1]))
                {
                    Console.WriteLine("Die Ausgabe Dateinamen sind nicht gültig!");
                    return false;
                }
            }
            else
            {
                string path = outPath[0];
                if (path.EndsWith(Path.DirectorySeparatorChar.ToString()) || !Path.HasExtension(path))
                {
                    fileOutput = false;
                }
                else
                {
                    fileOutput = true;
                }
                if (truncate && fileOutput)
                {
                    Console.WriteLine("Im Modus Abgeschnittene Daten kann eine einzelne Datenspur nicht ausgegeben werden.");
                    Console.WriteLine("Bitte geben Sie zwei verschiedene Tracks an.");
                    return false;
                }
                if (cdda.Count > 0 && fileOutput)
                {
                    Console.WriteLine("Es kann kein einzelner Track ausgegeben werden, wenn CDDA angegeben ist.");
                    return false;
                }
            }
            return true;
        }

        private static bool HasArgument(string argument, string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals(argument, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static string GetSoloArgument(string argument, string[] args)
        {
            for (int i = 0; i < args.Length-1; i++)
            {
                if (args[i].Equals(argument, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        private static List<string> GetMultiArgument(string argument, string[] args)
        {
            List<string> retval = new List<string>();
            int start = -1;
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals(argument, StringComparison.OrdinalIgnoreCase))
                {
                    start = i + 1;
                    break;
                }
            }
            if (start > 0)
            {
                for (int i = start; i < args.Length; i++)
                {
                    if (args[i].StartsWith("-"))
                    {
                        break;
                    }
                    else
                    {
                        retval.Add(args[i]);
                    }
                }
            }
            return retval;
        }

        private static void PrintUsage()
        {
            Console.WriteLine();
            Console.WriteLine("BuildGDI - Command line GDIBuilder");
            Console.WriteLine("Verwendung: buildgdi -data dataFolder -ip IP.BIN -cdda track04.raw track05.raw -output folder -gdi disc.gdi");
            Console.WriteLine();
            Console.WriteLine("Parameter:");
            Console.WriteLine("-data <Ordner> (Erforderlich) = Speicherort der Dateien für die Disk");
            Console.WriteLine("-ip <Datei> (Erforderlich) = Speicherort des Bootsektors der Disk IP.BIN");
            Console.WriteLine("-cdda <Dateien> (Optional) = Liste der RAW CDDA-Titel auf der Disk");
            Console.WriteLine("-output <Ordner oder Datei(en)> (Erforderlich) = Ausgabeort");
            Console.WriteLine("   Wenn es sich bei der Ausgabe um einen Ordner handelt, werden Tracks mit Standard Dateinamen generiert.");
            Console.WriteLine("   Gebe andernfalls einen Dateinamen für track03.bin auf reinen Daten Disks an. ");
            Console.WriteLine("   oder zwei Dateien für Disks mit CDDA.");
            Console.WriteLine("-gdi <Datei> (Optional) = Pfad der disc.gdi Datei für diese Disk");
            Console.WriteLine("   Vorhandene GDI Dateien werden mit den neuen Tracks aktualisiert.");
            Console.WriteLine("   Wenn kein GDI vorhanden ist, werden nur Zeilen für Track 3 und höher geschrieben.");
            Console.WriteLine("-V <Datenträgerkennzeichen> (Optional) = Der Name des Datenträgers (Standard ist DREAMCAST)");
            Console.WriteLine("-iso (Optional) = Ausgabe von 2048 Byte Disk Sektoren, die in ISO9660 gefunden wurden, anstelle von 2352");
            Console.WriteLine("-truncate (Optional) = Generierte Daten nicht auf die richtige Größe auffüllen");
            Console.WriteLine("-date (Optional) = Legen Sie ein benutzerdefiniertes Datum und eine benutzerdefinierte Uhrzeit fest,\n                   zu der die Disk erstellt wurde.");
            Console.WriteLine("-rebuild (Optional) = Erstellen einer neuen GDI unter Verwendung einer bestehenden GDI als Datenquelle");
            Console.WriteLine("   Erfordert die Argumente -gdi, -data und -output. Die Dateien werden kopiert von ");
            Console.WriteLine("   der Originalen Disk. Dateien im Ordner -data werden der kopierten Disk hinzugefügt,");
            Console.WriteLine("   wenn es sich um neue Dateien handelt, oder sie ersetzen vorhandene Dateien am gleichen Ort.");
            Console.WriteLine("   Dazu muss -output ein Ordner sein. -ip ist optional, um die vorhandene IP.BIN zu ersetzen.");
            Console.WriteLine("-extract (Optional) =  Extrahiert eine GDI Datei in einen Ordner");
            Console.WriteLine("   Für die Extraktion sind die Argumente -gdi und -output erforderlich. -ip ist optional, um IP.BIN zu extrahieren.");
        }
    }
}
