#!/bin/sh
#
cd UniTTT.Logik/
dmcs -optimize -target:library -out:../bin/UniTTT.Logik.dll *.cs Properties/.*.cs Database/*.cs Fields/*.cs KI/*.cs Player/*.cs Network/*.cs
cd ..
cd UniTTT.Konsole/
dmcs -optimize -r:../bin/UniTTT.Logik.dll -target:exe -out:../bin/UniTTT.Konsole.exe *.cs Games/*.cs
cd ..
