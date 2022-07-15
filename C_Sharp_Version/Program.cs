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
What it does ?
1. Open file browser to browse for the original file. 
2. Open file browser to SAVE the target upscaled image.
3. After saving the file, the process will take up to 10 seconds and more dependin on source image and target upscale.
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace UpscalerTest
{
    static class Program
    {
		// Edit these to setup image target upscale
        private static double upscale_width_multiply = 2.5;
        private static double upscale_height_multiply = 2;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                Image input = Image.FromFile(op.FileName);

                SaveFileDialog sav = new SaveFileDialog();
                if (sav.ShowDialog() == DialogResult.OK)
                {

                    // 1 Do the process, open the source image first
                    int or_width = input.Width;
                    int or_height = input.Height;

                    ResolutionBlocksUpscaler.Initialize(or_width, or_height, (int)(upscale_height_multiply * or_width), (int)(upscale_width_multiply * or_height));

                    Bitmap bt = (Bitmap)input;

                    int[] input_buffer = new int[or_width * or_height];
                    int[] target_buffer = new int[(int)(upscale_height_multiply * or_width * upscale_width_multiply * or_height)];
                    // store image pixels into input buffer
                    for (int input_x = 0; input_x < or_width; input_x++)
                    {
                        for (int input_y = 0; input_y < or_height; input_y++)
                        {
                            input_buffer[input_x + (input_y * or_width)] = bt.GetPixel(input_x, input_y).ToArgb();
                        }
                    }
                    // 2 do upscaling process
                    ResolutionBlocksUpscaler.Process(input_buffer, ref target_buffer);

                    // 3 save target buffer into image
                    Bitmap bt_target = new Bitmap((int)(upscale_height_multiply * or_width), (int)(upscale_width_multiply * or_height));
                    for (int i = 0; i < target_buffer.Length; i++)
                    {
                        int y = i / (int)(upscale_height_multiply * or_width);
                        int x = i % (int)(upscale_height_multiply * or_width);
                        bt_target.SetPixel(x, y, Color.FromArgb(target_buffer[i]));
                    }

                    bt_target.Save(sav.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }


        }
    }
}
