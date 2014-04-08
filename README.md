webmConverter
=========
moot <3

The next release is going to be error catching and fixing the terrible preview GUI I threw together.

![Screenshot](http://a.pomf.se/esxeab.png)
Downloads
=========
[This](https://github.com/Wsheerio/webmConverter/raw/master/Executable/webmConverter.zip) should be a build of the most recent version.

You're going to need [ffmpeg](http://ffmpeg.zeranoe.com/builds/), I recommend grabbing one of the static builds.

Place WebMConverter.exe and the fonts folder in the same directory as ffmpeg.exe

You also need .NET Framework 4.0

Documentation
=========

If your webm is too big press go again, after every encode it will automatically adjust the bit-rate to get closer to the wanted size.

Video

Path of the input file.

    C:\Users\Name\Videos\chinese cartoon.mkv

Subtitles

Path of the subtitle file. Currently only supports .ass. Leave blank if you don't want subtitles. Pressing the subtitle browse button extracts the subs from the video and places it in the same directory as the video.

    C:\Users\Name\Videos\chinese cartoon.ass

Start Time

Enter the start time in seconds, defaults to 0.

    0
    44

Duration

Enter the your desired length in seconds.

    54.123

Size limit

The maximum size allowed for the output in megabytes, defaults to 3.

    1
    2.5
    3.12341

Resolution

The resolution of the output file. The first number is the width, the second is the height. -1 scales the other size to keep the same aspect ratio, leave both as -1 to keep the input resolution. If your webm looks terrible or you can't get the size low enough with the adjust value, lower this.

    -1:720
    -1:1080
    1280:-1

Sound

Enter the bitrate for audio in kilobits. Leave blank if you don't want sound.

    32
    192

Crop

This lets you crop a video\. The command looks like this, Width:Height:X:Y. Width is the width of the rectangle being cropped, height is the height, x and y are the coordinates of the rectangle being cropped. in_w and in_h grab the videos resolution.

    500:500:10:10

Advanced

Let's you use more commands. You can't use anything with -vf right now, I'll fix that in a later version

Output

This is where you put your output file name.

    C:\Users\Name\Videos\thefuture.webm

To Time

Seeks to this point in the video in seconds.

    54.12
    18.94
    
Back Forward

Go back or forward the amount of time in seconds input into the text box in between them. Defaults to 0.04.

    0.04
    0.4
    1
    
Refresh

This is for refreshing the preview after change the crop.

Current Time

This displays the current time relative to the starting time.
