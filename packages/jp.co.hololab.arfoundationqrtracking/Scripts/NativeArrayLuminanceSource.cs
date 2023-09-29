using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using ZXing;

namespace HoloLab.ARFoundationQRTracking
{
    /// <summary>
    /// LuminanceSource implementation that uses a NativeArray as its luminance buffer.
    /// </summary>
    public class NativeArrayLuminanceSource : BaseLuminanceSource
    {
        public NativeArrayLuminanceSource(int width, int height)
            : base(width, height)
        {
        }

        public NativeArrayLuminanceSource(NativeArray<byte> data, int width, int height)
            : base(width, height)
        {
            data.CopyTo(luminances);
        }

        protected override LuminanceSource CreateLuminanceSource(byte[] newLuminances, int width, int height)
        {
            return new NativeArrayLuminanceSource(width, height)
            {
                luminances = newLuminances
            };
        }
    }
}

