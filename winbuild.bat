mkdir bin
mkdir bin\data\plugins

copy README bin\

cd UniTTT.Logik
dmcs -optimize -target:library -r:../lib/Poc.dll -out:../bin/UniTTT.Logik.dll *.cs  Game/*.cs Fields/*.cs AI/*.cs Player/*.cs Network/*.cs Command/*.cs Command/Commands/*.cs Plugin/*.cs FileSystem/*.cs OS/*.cs
cd ..
cd UniTTT.Konsole
dmcs -optimize -r:../bin/UniTTT.Logik.dll -target:exe -out:../bin/UniTTT.exe *.cs
cd ..
cd UniTTT.ScreenSaver
dmcs -optimize -r:System.Drawing.dll -r:System.Windows.Forms -r:../bin/UniTTT.Logik.dll -target:exe -out:../bin/UniTTT.ScreenSaver.exe *.cs
cd .. 
cd UniTTT.Plugin.FourConnect
dmcs -optimize -target:library -r:../bin/UniTTT.Logik.dll -out:../bin/data/plugins/UniTTT.Plugin.FourConnect.dll *.cs
cd ..
cd UniTTT.Gui
dmcs -optimize -r:../bin/UniTTT.Logik.dll -r:System.Drawing.dll -r:System.Windows.Forms -out:../bin/data/plugins/UniTTT.Gui.exe *.cs
cd ..