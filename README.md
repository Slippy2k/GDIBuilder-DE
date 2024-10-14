GDIBuilder
==========

Hilfsprogramm zur Erstellung von Dreamcast .GDI Images von Grund auf

Wird mit einem Verzeichnis von Daten Dateien, dem IP.BIN Bootsektor und optional RAW CDDA Tracks geliefert,
erzeugt dieses Tool automatisch der Track(s) für den High Density Bereich eines GD-ROM Images.
Es erzeugt auch den TOC Track, der in den Bootsektor geschrieben wird.

Ein bootfähiges GD-ROM erfordert, dass das primäre Executable (normalerweise 1ST_READ.BIN genannt) am Ende der finalen Datenspur platziert wird.
Ende der endgültigen Datenspur platziert werden, da es sonst nicht von der Konsole geladen wird. Diese Anforderung besteht nicht
für MIL-CDs.

Der ISO9660 Code wurde von den .NET DiscUtils abgeleitet, wobei einige Änderungen vorgenommen wurden:
- Wenn Joilet deaktiviert ist (was bei diesem Tool der Fall ist), werden keine zusätzlichen Dateitabellen ausgegeben.
- Die Reihenfolge, in der DiscUtils ISO Sektionen ausgibt, wurde umgekehrt. (Verzeichnis Tabellen kommen jetzt vor Dateien)
- Fehler bei der Ausgabe von Nicht Joilet Dateinamen behoben. Dateinamen wurden nicht wie vorgesehen mit ;1 angehängt.
- Hinzufügen eines LBA Offset für das gesamte Image.
- Hinzufügen eines End LBA Offsets für das gesamte Image. Das Image wird auf die gewünschte Größe skaliert.
- Ende der letzten Datei LBA hinzugefügt, wenn gesetzt, werden alle Dateien im Image an diese Stelle zurückgeschoben.
- Es wurden Eigenschaften hinzugefügt, um die meisten Text Identifikatoren (Anwendung, Volume Set, Preparer, etc.) zu definieren.
- Von dieser Anwendung nicht verwendete Elemente wie andere Iage formate und Dateisysteme wurden entfernt.
