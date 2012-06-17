mkdir bin
cd UniTTT.Logik
dmcs.exe -optimize -target:library -out:../bin/UniTTT.Logik.dll *.cs Properties\.*.cs Fields\*.cs KI\*.cs Player\*.cs Network\*.cs Command\*.cs Command\Commands\*.cs Plugin\*.cs
cd ..
cd UniTTT.Konsole
dmcs.exe -optimize -r:../bin/UniTTT.Logik.dll -target:exe -out:../bin/UniTTT.Konsole.exe *.cs
cd ..