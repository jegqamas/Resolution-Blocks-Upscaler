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


namespace ResolutionBlocksUpscaler
{
/// Initialize the scaler and make it ready to use. This method must be called (once) before anything else.
/// Note that:
/// target_res_width must be > original_res_width
/// target_res_height must be > original_res_height
extern void Initialize(int original_res_width, int original_res_height, int target_res_width, int target_res_height);

/// Call this at each buffer update, for example, it can be called when  original_res_buffer is outputed and need to be upscaled into target_res_buffer.
/// Note that:
/// Initialize() method must be called before using this method.
/// original_res_buffer must be original_res_width * original_res_height in size.
/// target_res_buffer must be target_res_width * target_res_height in size.
extern void Process(int* original_res_buffer, int* target_res_buffer);

/// Get aspect ratio x:y value of an resolution
extern void GetAspectRatioString(int width, int height, char* aspect_ratio);
/// Print resolutions that can be scale into from a resolution.
/// original_res_width x target_res_height the resolution that need to be upscale into
/// resolutionCount: how many resolutions to list, it must be a number multiple of 2
extern void ListUpscalableResolutionsForAResolution(int original_res_width, int original_res_height, int resolutionCount);

/// Add resolutions that can be scale into from a resolution into a string array. Also will print resolutions too into console
/// original_res_width x target_res_height the resolution that need to be upscale into
/// resolutionCount: how many resolutions to list, it must be a number multiple of 2, also the resolutions array must be in resolutionCount size. i.e. char* [resolutions];
/// include_desc: normally it lists resolutions in format w x h, if this option is true, it will add additional desciption such as 640 x 480 NTSC Tv, 1920 x 1080 Widescreen Full HD ..etc
extern void ListUpscalableResolutionsForAResolution(int original_res_width, int original_res_height, char** resolutions, int resolutionCount, bool include_desc);
}
