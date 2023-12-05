![3DS Tools](https://files.catbox.moe/8x0evs.png)<br>
3DSPal created by 3DSTools team. (ndsboy87)<br>Nintendo, I understand if you want a takedown, but these aren't my games and other stuff, go to Credits.<br>2DS basically can support some of these.<br>DS can't.<br>2DSPal and DSPal may be next versions?
# 3DSPal
A command-line tool that uses commands for 3DS Mod Installation.
## Installation
Simple, go to releases and latest release and extract 3DSPal.zip and run 3dspals.exe<br>
3DSPal will auto-update and use the version variable in 3DSPal.
## Developer Guide
3DSPal is open-source. You can get the source now!<br>Open Visual Studio 2022<br>Open Project Folder<br>Start editing!
## Credits
Me, Myself, and I.<br>Oh also the awesome team at stackoverflow!
## How to Compile
You need Visual Studio 2022 with .NET developer toolkit.<br>.NET 4.7.2 is used, and C#!
## 3DSPal Commands
Set SD Card:
```txt
set SDCardFolder = "D:\"
```
Download CIA (Direct Link):
```txt
install --cia="http://streetpass.ct8.pl/port/cias/faceraiders.cia"
```
Quick warning, my website is currently dry and only has 1 cia to download which you can find at: [CIA List](http://streetpass.ct8.pl/port/)<br>
Download CIA (From My Website):
```txt
install --faceraiders
```
You can download boot9strap with 3DSPal!<br>
Download boot9strap (in 2 steps):
```txt
install --boot9strap --step1
install --boot9strap --step2
```
Test command:
```txt
wafkee
```
Docs in development.
