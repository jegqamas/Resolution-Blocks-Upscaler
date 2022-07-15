/*
This file is part of Resolution Blocks Upscaler
Resolution Blocks Upscaler Written by Alaa Ibrahim Hadid.
email: mailto:alaahadidfreeware@gmail.com

Resolution Blocks Upscaler is licensed under the MIT License.


MIT License

Copyright (c) 2021 Alaa Ibrahim Hadid 2021 - 2022

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
/*
This file is a working implemntation of the c++ version, it works and tested. See Program.cs file.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpscalerTest
{
    class ResolutionBlocksUpscaler
    {

        static int orig_buffer_size;
        static int orig_res_width;
        static int orig_res_height;

        static int targ_buffer_site;
        static int targ_res_width;
        static int targ_res_height;

        static double w_mu;
        static double h_mu;

        static bool use_w_mu_add;
        static bool use_h_mu_add;
        static bool w_mu_flip_flop;
        static bool h_mu_flip_flop;

        /*
        w_mu and h_mu are the width-multiplier and height-multiplier
        For example, from res 256 x 240 into 640 x 480 the result is 2.5:2, or w_mu=2.5 and h_mu=2.
        Here, in this method, we ignore anything else and try to make it work by simply applying
        Algorthim and tricks to upscale image-pixels-buffer.

        This method is used in My Nes emulator <https://github.com/alaahadid/My-Nes/blob/master/MyNes/SDL2Renderers/SDL2VideoRenderer.cs>
        and works 100%.
        */

        static int tmp_current_x;
        static int tmp_current_y;

        /// Initialize the scaler and make it ready to use. This method must be called (once) before anything else.
        /// Note that:
        /// target_res_width must be > original_res_width
        /// target_res_height must be > original_res_height
        public static void Initialize(int original_res_width, int original_res_height, int target_res_width, int target_res_height)
        {
            orig_res_width = original_res_width;
            orig_res_height = original_res_height;
            orig_buffer_size = original_res_width * original_res_height;

            targ_res_width = target_res_width;
            targ_res_height = target_res_height;
            targ_buffer_site = target_res_width * target_res_height;

            w_mu = (double)targ_res_width / (double)orig_res_width;

            use_w_mu_add = false;

            int w_mu_int = (int)w_mu;
            if ((w_mu - w_mu_int) != 0)
            {
                use_w_mu_add = true;
            }
            w_mu = (int)Math.Floor(w_mu);



            h_mu = (double)targ_res_height / (double)orig_res_height;

            use_h_mu_add = false;
            int h_mu_int = (int)h_mu;
            if ((h_mu - h_mu_int) != 0)
            {
                use_h_mu_add = true;
            }
            h_mu = (int)Math.Floor(h_mu);
        }

        /// Call this at each buffer update, for example, it can be called when original_res_buffer is outputed and need to be upscaled into target_res_buffer.
        /// Another example: when a nes emu outputs a video buffer at the end of frame in res 256 x 240 and needs to be upscaled into higher res like 640 x 480.
        /// Note that:
        /// Initialize() method must be called before using this method.
        /// original_res_buffer must be original_res_width * original_res_height in size.
        /// target_res_buffer must be target_res_width * target_res_height in size.
        public static void Process(int[] original_res_buffer, ref int[] target_res_buffer)
        {
            tmp_current_x = 0;
            tmp_current_y = 0;
            w_mu_flip_flop = false;
            h_mu_flip_flop = false;

            for (int i = 0; i < orig_buffer_size; i++)// no skip
            {
                // Fill a block
                for (int y_t = 0; y_t < h_mu; y_t++)
                {
                    for (int x_t = 0; x_t < w_mu; x_t++)
                    {
                        target_res_buffer[(tmp_current_x + x_t) + ((tmp_current_y + y_t) * targ_res_width)] = original_res_buffer[i];
                    }
                }
                // Update offsets
                tmp_current_x += (int)w_mu;
                if (tmp_current_x >= targ_res_width)
                {
                    tmp_current_x = 0;
                    tmp_current_y += (int)h_mu;
                    if (tmp_current_y >= targ_res_height)
                    {
                        tmp_current_y = 0;
                    }

                    if (use_h_mu_add)
                    {
                        h_mu_flip_flop = !h_mu_flip_flop;
                        if (h_mu_flip_flop)
                            h_mu++;
                        else
                            h_mu--;
                    }
                }
                // We need to use this trick in case the original resolution and target resolution are not related in division (division result not whole numbers)
                if (use_w_mu_add)
                {
                    w_mu_flip_flop = !w_mu_flip_flop;
                    if (w_mu_flip_flop)
                        w_mu++;
                    else
                        w_mu--;
                }

            }
        }

        /// Get aspect ratio x:y value of an resolution
        /// Value will be set to aspect_ratio
        public static void GetAspectRatioString(int width, int height, string aspect_ratio)
        {
            // The idea is to find 2 numbers, the result of the division of these numbers
            // equal the result of division of width / height. Result will be set in format
            // x:y while x > y and x/y = width/height
            // x and y smallest whole numbers possible to make it correct
            double rat = width / height;
            bool found = false;
            for (double t = 1; t < 1000; t++)
            {
                for (double z = 1; z < 1000; z++)
                {
                    if (rat == z / t)
                    {
                        aspect_ratio = z + ":" + t;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }
        }
        /// Print resolutions that can be scale into from a resolution.
        /// original_res_width x target_res_height the resolution that need to be upscale into
        /// resolutionCount: how many resolutions to list, it must be a number multiple of 2
        public static void ListUpscalableResolutionsForAResolution(int original_res_width, int original_res_height, int resolutionCount)
        {
            double x = 1;
            double y = 1;
            int max = (int)Math.Sqrt(resolutionCount);
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    double w = original_res_width * x;
                    double h = original_res_height * y;

                    string as_ratio = "";
                    GetAspectRatioString((int)w, (int)h, as_ratio);

                    if (as_ratio == "4:3")
                        as_ratio += " Latterbox SDTV/NTSC TV";

                    if (as_ratio == "16:9")
                        as_ratio += " Widescreen SDTV/HDTV";

                    if (w == original_res_width && h == original_res_width)
                        as_ratio += " NO UPSCALE";
                    else if (w == 1920 && h == 1080)
                        as_ratio += "/Full HD";

                    Console.WriteLine(w + " x " + h + " " + as_ratio);

                    x += 0.5;
                }

                y += 0.5;
                x = y;
            }
        }
        /// Add resolutions that can be scale into from a resolution into a string array. Also will print resolutions too into console
        /// original_res_width x target_res_height the resolution that need to be upscale into
        /// resolutionCount: how many resolutions to list, it must be a number multiple of 2, also the resolutions array must be in resolutionCount size. i.e. char* [resolutions];
        /// include_desc: normally it lists resolutions in format w x h, if this option is true, it will add additional desciption such as 640 x 480 NTSC Tv, 1920 x 1080 Widescreen Full HD ..etc
        public static void ListUpscalableResolutionsForAResolution(int original_res_width, int original_res_height, string[] resolutions, int resolutionCount, bool include_desc)
        {
            double x = 1;
            double y = 1;
            int res_index = 0;
            int max = (int)Math.Sqrt(resolutionCount);
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    double w = original_res_width * x;
                    double h = original_res_height * y;
                    string as_ratio = "";
                    GetAspectRatioString((int)w, (int)h, as_ratio);

                    if (include_desc)
                    {
                        if (as_ratio == "4:3")
                            as_ratio += " Latterbox SDTV/NTSC TV";

                        if (as_ratio == "16:9")
                            as_ratio += " Widescreen SDTV/HDTV";

                        if (w == original_res_width && h == original_res_width)
                            as_ratio += " NO UPSCALE";
                        else if (w == 1920 && h == 1080)
                            as_ratio += "/Full HD";
                    }

                    Console.WriteLine(w + " x " + h + " " + as_ratio);


                    resolutions[res_index] = w + " x " + h + " " + as_ratio;

                    res_index++;

                    x += 0.5;
                }

                y += 0.5;
                x = y;
            }
        }

    }
}
