WebM Converter
=========

**Bitrate is now entered as Kb/s, not MB/s.**

![Screenshot](https://a.pomf.cat/bdsztg.png)
Downloads
=========
The [release page](https://github.com/Wsheerio/webmConverter/releases) should have a build of the most recent version.

You're going to need [ffmpeg](http://ffmpeg.zeranoe.com/builds/), I recommend grabbing one of the static builds.

You also need [.NET Framework 4.0](https://www.microsoft.com/en-us/download/details.aspx?id=17851)

Place WebM Converter.exe and the fonts folder in the same directory as ffmpeg.exe

Documentation
=========

Video

Path of the input file.

    C:\Users\Name\Videos\chinese cartoon.mkv

Subtitles

Path of the subtitle file. Currently only supports .ass. Leave blank if you don't want subtitles. Press the button to switch between external and internal subtitles. Using internal subs will only work with containers that support subtitles and files with only one subtitle track.

    C:\Users\Name\Videos\chinese cartoon.ass
    
Output

Where you want to put the output file and what you want it to be named.

    C:\Users\Name\Videos\chinese cartoon.webm

MetaData

As of now only supports the title tag.

    Saiki Kusuo

Start Time

Enter the start time in seconds or hh:mm:ss.

    0
    44
    00:43:54.901341

Stop Time

Enter the stop time in hh:mm:ss.

    00:30:1471.163
    01:34:12.123

Size Limit/Bitrate

The maximum size in megabytes or the bitrate in **kb/s** of the output. Press the button to switch between size limit and bitrate.

    1
    2.5
    3.12341

Audio/Video or Audio Only

Enter the bitrate for audio in kilobits. Use "0" if you don't want sound. Pressing the button will produce a webm with only audio.

    32
    192

Resolution

The resolution of the output file. The first number is the width, the second is the height. -1 scales the other size to keep the same aspect ratio, leave both as -1 to keep the input resolution.

    -1:720
    -1:1080
    1280:-1

Crop

This lets you crop a video\. The command looks like this, Width:Height:X:Y. Width is the width of the rectangle being cropped, height is the height, x and y are the coordinates of the rectangle being cropped. in_w and in_h grab the videos width and height respectively.

    500:500:10:10

Saturation

This lets you remove color from the output file, this is useful if you want to output to greyscale or remove a small amount of color to reduce the filesize. Note that using this to reduce filesize will only work if the file is easily fittable within the size limit. This can be a number from 0 to 1, 0 being greyscale and 1 being full color.

    0
    0.4324
    1

Speed

Adjust the speed with a multiplier. As of now this works by dropping frames.

    2
    0.4
    16

Image Preview

This lets you preview the output video, very useful for checking cropping. Subtitles are not shown. Start seeks to the beginning, Preview refreshes, End seeks to the end.
