#!/bin/sh
mkdir UniTTT
mkdir UniTTT/usr
mkdir UniTTT/usr/share
mkdir UniTTT/usr/share/UniTTT
mkdir UniTTT/DEBIAN

cp ../../bin/UniTTT.exe UniTTT/usr/share/UniTTT/
cp ../../bin/UniTTT.Logik.dll UniTTT/usr/share/UniTTT/
cp ../../bin/data UniTTT/usr/share/UniTTT
cp control UniTTT/DEBIAN/

dpkg -b ./UniTTT unittt.deb