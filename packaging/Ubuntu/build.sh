#!/bin/sh
mkdir UniTTT
mkdir UniTTT/usr
mkdir UniTTT/usr/share
mkdir UniTTT/usr/share/UniTTT
mkdir UniTTT/usr/share/pixmaps
mkdir UniTTT/usr/share/applications
mkdir UniTTT/DEBIAN

cp ../../bin/UniTTT.exe UniTTT/usr/share/UniTTT/
cp ../../bin/UniTTT.Logik.dll UniTTT/usr/share/UniTTT/
cp ../../bin/data/ UniTTT/usr/share/UniTTT/
cp ../../graphics/UniTTT.Icon.ico UniTTT/usr/share/pixmaps
cp control UniTTT/DEBIAN/
cp UniTTT.desktop UniTTT/usr/share/applications/

fakeroot dpkg -b ./UniTTT unittt.deb
