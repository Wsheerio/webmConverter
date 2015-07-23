WebM Converter
=========
moot?

![Screenshot](https://d.maxfile.ro/cfwpyonhib.png)
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

Path of the subtitle file. Currently only supports .ass. Leave blank if you don't want subtitles.

    C:\Users\Name\Videos\chinese cartoon.ass
    
Output

Where you want to put the output file and what you want it to be named.

    C:\Users\Name\Videos\chinese cartoon.webm

Start Time

Enter the start time in seconds or hh:mm:ss.

    0
    44
    00:43:54.901341

Duration/Stop Time

Enter the duration/stop time in hh:mm:ss, clicking the button will change from duration to stop time.

    00:30:1471.163
    01:34:12.123

Size limit

The maximum size allowed for the output in megabytes.

    1
    2.5
    3.12341

Audio

Enter the bitrate for audio in kilobits. Leave blank if you don't want sound.

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

Use Internal Subs

Using internal subs will only work with containers that support subtitles and files with only one subtitle track.

Image Preview

This lets you preview the output video, very useful for checking cropping. Subtitles are not shown.
