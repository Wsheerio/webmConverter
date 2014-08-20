webmConverter
=========
moot <3

I re-wrote it from scratch, it should be much nicer to use now.

![Screenshot](http://a.pomf.se/oqzaxk.png)
Downloads
=========
[This](https://github.com/Wsheerio/webmConverter/raw/master/Executable/webmConverter.zip) should be a build of the most recent version.

You're going to need [ffmpeg](http://ffmpeg.zeranoe.com/builds/), I recommend grabbing one of the static builds.

Place webm Converter.exe and the fonts folder in the same directory as ffmpeg.exe

You also need .NET Framework 4.0

Documentation
=========

Video

Path of the input file.

    C:\Users\Name\Videos\chinese cartoon.mkv

Subtitles

Path of the subtitle file. Currently only supports .ass. Leave blank if you don't want subtitles.

    C:\Users\Name\Videos\chinese cartoon.ass
    
Output

Where you want to put the output file and what you want it to be named.

    C:\Users\Name\Videos\chinese cartoon.webm
    
Meta Data

    Putting the source here is a nice thing to do.

Start Time

Enter the start time in seconds, defaults to 0, can be manually set or changed with the sliders. Kind of buggy right now.

    0
    44

Duration, defaults to video length, can be manually set or changed with the sliders.

Enter the your desired length in seconds.

    54.123

Size limit

The maximum size allowed for the output in megabytes, defaults to 3.

    1
    2.5
    3.12341

Crop

This lets you crop a video\. The command looks like this, Width:Height:X:Y. Width is the width of the rectangle being cropped, height is the height, x and y are the coordinates of the rectangle being cropped. in_w and in_h grab the videos resolution.

    500:500:10:10

Sound

Enter the bitrate for audio in kilobits. Leave blank if you don't want sound.

    32
    192

Resolution

The resolution of the output file. The first number is the width, the second is the height. -1 scales the other size to keep the same aspect ratio, leave both as -1 to keep the input resolution. If your webm looks terrible or you can't get the size low enough with the adjust value, lower this.

    -1:720
    -1:1080
    1280:-1

Use Internal Subs

    Using internal subs will only work with containers that support subtitles and files with only one subtitle track.

Output Preview

    The trackbar lets you preview your output from your set start time to your set duration.
    The preview button refreshses the preview without you having to move the trackbar, it's only useful if you change the crop.
