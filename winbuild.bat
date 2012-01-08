cd UniTTT.Logik
"C:\Program Files (x86)\Mono-2.10.6\lib\mono\4.0\dmcs.exe" -target:library -out:../bin/UniTTT.Logik.dll *.cs Properties\.*.cs Database\*.cs Fields\*.cs KI\*.cs Player\*.cs
cd ..
cd UniTTT.Konsole
"C:\Program Files (x86)\Mono-2.10.6\lib\mono\4.0\dmcs.exe" -r:../bin/UniTTT.Logik.dll -target:exe -out:../bin/UniTTT.Konsole.exe *.cs
cd ..