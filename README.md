# Resolution Blocks Upscaler
 A method to process resolution buffer into other with block-high quality without pixels lose.

![Snapshot 1](/snaps/snap0.PNG?raw=true "Snapshot 1")
![Snapshot 2](/snaps/snap1.PNG?raw=true "Snapshot 2")
![Snapshot 3](/snaps/snap2.PNG?raw=true "Snapshot 3")
![Snapshot 4](/snaps/snap3.PNG?raw=true "Snapshot 4")
![Snapshot 5](/snaps/snap4.PNG?raw=true "Snapshot 5")
![Snapshot 6](/snaps/snap5.PNG?raw=true "Snapshot 6")

# How it works ?
It simply allows to upscale image buffer from res into another, using blocks method. 

For example, let's say we have an image buffer in res 256 x 240 (the buffer size is 256 * 240), we need to output it or upscale it into res 640 x 480.

Then, each pixel of 256 x 240 image can be aligned with a **block of pixels** of the target image 640 x 480.

The resolution of a block of pixels in the target image can be calculated like this:

target_pixel_block_width= 640 / 256 = 2.5

target_pixel_block_height= 480 / 240 = 2

This mean, each pixel of the image of 256 x 240 (numbers are for example only, it should work with all resolutions) can be set into block of pixels of res 2.5 x 2 in the target image of res 640 x 480.

Another example: 400 x 300 buffer upscaled into 1200 x 600, then i pixel of 400 x 300 can be set into j block of pixels of size 3 x 2 of image 1200 x 600.

Based on this, this source file is written. 

Let's say we want to upscale res 256 x 240 image into 640 x 480

We can do this with this library by simply:

1. At the program start, we call Initialize() method like this:

`Initialize (256,240,640,480);`

2. At the time when the program need to upscale the image (i.e. at frame end), we do:

`Process (original_res_buffer,target_res_buffer);`

And:

`original_res_buffer` is the res 256 x 240 buffer. It must be in size 256 * 240 = 61440.

`target_res_buffer` is the res 640 x 480 buffer. It must be in size 640 * 480 = 307200.

It should work with all resolutions, but:
- The original resolution must be smaller than the target resolution.
- The pixel block size number should be whole number or x.5 numbers (with only .5, example : 1, 1.5, 2, 2.5, 3, 3.5 ...etc), for example, 640 / 256 = 2.5 is accepted, but 720 / 256 =~ 2.8 is not accepted. In other hand, 768 / 256 = 3 is accepted. 
Wrong numbers will result distorted image. Method `void ListUpscalableResolutionsForAResolution` can be used to see what resolutions can be upscaled into from a resolution.

Currently only buffer stored in "int" integer is supported, that's mean A8888 color format can be used.

This method is based on My Nes upscaling code, it works 100% in [My Nes emulator](https://github.com/jegqamas/My-Nes/blob/master/MyNes/SDL2Renderers/SDL2VideoRenderer.cs). 
This method is implemented in [ANES emulator](https://github.com/jegqamas/ANES/blob/main/src/SDLOut/SDLVideoOut.c) which is a nes emulator written in C. 
