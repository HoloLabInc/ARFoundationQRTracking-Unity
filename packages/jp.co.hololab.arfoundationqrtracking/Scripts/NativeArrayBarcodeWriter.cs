// Code referenced from the following link:
// https://github.com/micjahn/ZXing.Net/blob/v0.16.9.0/Source/lib/unity/Color32Renderer.cs

/*
* Copyright 2012 ZXing.Net authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace HoloLab.ARFoundationQRTracking
{
    public class NativeArrayBarcodeWriter : BarcodeWriter<NativeArray<byte>>
    {
        public NativeArrayBarcodeWriter()
        {
            base.Renderer = new NativeArrayRenderer();
        }
    }

    public class NativeArrayRenderer : IBarcodeRenderer<NativeArray<byte>>
    {
        public Color32 Foreground { get; set; }

        public Color32 Background { get; set; }

        public NativeArrayRenderer()
        {
            Foreground = Color.black;
            Background = Color.white;
        }

        public NativeArray<byte> Render(BitMatrix matrix, BarcodeFormat format, string content)
        {
            return Render(matrix, format, content, null);
        }

        public NativeArray<byte> Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {
            var foregroundNativeArray = new NativeArray<byte>(new[] { Foreground.r, Foreground.g, Foreground.b, Foreground.a }, Allocator.Temp);
            var backgroundNativeArray = new NativeArray<byte>(new[] { Background.r, Background.g, Background.b, Background.a }, Allocator.Temp);

            var result = new NativeArray<byte>(matrix.Width * matrix.Height * 4, Allocator.Persistent);

            var offset = matrix.Height - 1;
            var divisionLeftover = matrix.Width % 32;
            var bitRowRemainder = divisionLeftover > 0 ? divisionLeftover : 32;

            for (int y = 0; y < matrix.Height; y++)
            {
                var ba = matrix.getRow(offset - y, null);
                int[] bits = ba.Array;

                for (int x = 0; x < bits.Length; x++)
                {
                    int finalIndex = 32;

                    if (x == bits.Length - 1)
                    {
                        finalIndex = bitRowRemainder;
                    }

                    for (int i = 0; i < finalIndex; i++)
                    {
                        int bit = (bits[x] >> i) & 1;
                        var colorNativeArray = bit == 1 ? foregroundNativeArray : backgroundNativeArray;
                        NativeArray<byte>.Copy(colorNativeArray, 0, result, (matrix.Width * y + x * 32 + i) * 4, 4);
                    }
                }
            }

            return result;
        }
    }
}
