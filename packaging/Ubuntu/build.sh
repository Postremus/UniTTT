#!/bin/sh
mkdir -p UniTTT
mkdir -p UniTTT/usr
mkdir -p UniTTT/usr/share
mkdir -p UniTTT/usr/share/UniTTT
mkdir -p UniTTT/DEBIAN

cp ../../bin/UniTTT.Logik.Konsole.exe UniTTT/usr/share/UniTTT/
cp ../../bin/UniTTT.Logik.dll UniTTT/usr/share/UniTTT/
cp ../../bin/data UniTTT/usr/share/UniTTT
cp control UniTTT/DEBIAN/

cd UniTTT

dpkg -b ./UniTTT UniTTT.deb
