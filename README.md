webmConverter
=========
moot <3
Downloads
=========
[This](https://github.com/Wsheerio/webmConverter/raw/master/Executable/webmConverter.zip) should be a build of the most recent version.

You're going to need [ffmpeg](http://ffmpeg.zeranoe.com/builds/), I recommend grabbing one of the static builds.

Place webm.exe and the fonts folder in the same directory as ffmpeg.exe

Documentation
=========
The browe buttons still don't do anything, you'll have to manually enter paths.

Video

Path of the input file.

C:\Users\Name\Videos\chinese cartoon.mkv

Subtitles

Path of the subtitle file. Currently only supports .ass

C:\Users\Name\Videos\chinese cartoon.ass

Size limit

The maximum size allowed for the output in megabytes.

1, 2, 3

Resolution

The resolution of the output file. If your webm looks terrible or you can't get the size low enough with the adjust value, lower this.

640x360, 853x480, 960x540, 1280x720, 1920x1080

Start Time / End Time

The start and end time of the input file you want to convert to webm in HH:MM:SS

00:00:18 00:01:48

Threads

Enter the amount of physical cores you have. If you don't know what this is just leave it blank.

4

Sound

Enter the bitrate for audio in kilobits.

32, 192

Adjust

Getting the right file size doesn't always work. Start off by entering 0 in this box, if your file is too big lower it, if your file is too small raise it.

Output

This is where you put your output file name.

thefuture.webm
