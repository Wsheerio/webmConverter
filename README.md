webmConverter
=========
moot <3

It should be a lot more stable now, please open an issue if you find anything.

![Screenshot](http://a.pomf.se/mpkzap.png)
Downloads
=========
[This](https://github.com/Wsheerio/webmConverter/raw/master/Executable/webmConverter.zip) should be a build of the most recent version.

You're going to need [ffmpeg](http://ffmpeg.zeranoe.com/builds/), I recommend grabbing one of the static builds.

Place webm.exe and the fonts folder in the same directory as ffmpeg.exe

You also need .NET Framework 4.0

Documentation
=========

If your webm is too big press go again, after every encode it will automatically adjust the bit-rate to get closer to the  size limit.

Video

Path of the input file.

    C:\Users\Name\Videos\chinese cartoon.mkv

Subtitles

Path of the subtitle file. Currently only supports .ass. Leave blank if you don't want subtitles. Pressing the subtitle browse button extracts the subs from the video and places it in the same directory as the video.

    C:\Users\Name\Videos\chinese cartoon.ass
    
Output

Where you want to put the output file and what you want it to be named.

    C:\Users\Name\Videos\chinese cartoon.webm
    
Meta Data

    Putting the source here will make it a lot easier for people to find the source.

Start Time

Enter the start time in seconds, defaults to 0, can be manually set or changed with the sliders. Kind of buggy right now.

    0
    44

Duration, defaults to video length, can be manually set or changed with the sliders.

Enter the your desired length in seconds. Kind of buggy right now.

    54.123

Runs

The more runs you do the closer your output is going to be to the allowed size. If your output file is over the input limit set this to two and try again, if it's still over set it to three, etc. The loop will always break if your file is below the allowed size. If set to 0 it will do a lot of loops.

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

The text box in the bottom right is the command the program is giving to ffmpeg.

Advanced settings

Use Internal Subs

Using internal subs will only work with containers that support subtitles and files with only one subtitle track.
