"c:\Program Files\Saxonica\SaxonEE9.4N\bin\Transform.exe" -s:D:\Programování\EVOXSVN\SchematronTest\LastSchSchema.sch  -o:D:\Programování\EVOXSVN\SchematronTest\LastSchSchema1.sch -xsl:D:\Programování\EvoXSVN\IsoSchematron\iso_dsdl_include.xsl 
"c:\Program Files\Saxonica\SaxonEE9.4N\bin\Transform.exe" -s:D:\Programování\EVOXSVN\SchematronTest\LastSchSchema1.sch -o:D:\Programování\EVOXSVN\SchematronTest\LastSchSchema2.sch -xsl:D:\Programování\EvoXSVN\IsoSchematron\iso_abstract_expand.xsl 
"c:\Program Files\Saxonica\SaxonEE9.4N\bin\Transform.exe" -s:D:\Programování\EVOXSVN\SchematronTest\LastSchSchema2.sch -o:D:\Programování\EVOXSVN\SchematronTest\LastSchSchema3.xsl -xsl:D:\Programování\EvoXSVN\IsoSchematron\iso_svrl_for_xslt2.xsl allow-foreign=true
"c:\Program Files\Saxonica\SaxonEE9.4N\bin\Transform.exe" -s:D:\Programování\EVOXSVN\SchematronTest\MatchSchedule.xml  -o:D:\Programování\EVOXSVN\SchematronTest\MatchSchedule.svrl -xsl:D:\Programování\EvoXSVN\SchematronTest\LastSchSchema3.xsl 

REM "c:\Program Files\Saxonica\SaxonEE9.4N\bin\Transform.exe" -s:D:\Programování\EVOXSVN\SchematronTest\MatchSchedule.xml  -o:D:\Programování\EVOXSVN\SchematronTest\MatchSchedule.out.xml -xsl:D:\Programování\EvoXSVN\SchematronTest\MatchSchedule.xsl 

