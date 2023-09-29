using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using ZXing;

namespace HoloLab.ARFoundationQRTracking
{
    public class ARFoundationQRDetector
    {
        private readonly BarcodeReader barcodeReader;

        public ARFoundationQRDetector()
        {
            barcodeReader = new BarcodeReader
            {
                AutoRotate = false,
                Options = new ZXing.Common.DecodingOptions
                {
                    TryHarder = false,
                    PossibleFormats = new[]
                    {
                        BarcodeFormat.QR_CODE
                    }
                }
            };
        }

        public async UniTask<Result[]> DetectMultipleAsync(XRCpuImage image)
        {
            XRCpuImage.AsyncConversion request = default;

            try
            {
                request = image.ConvertAsync(new XRCpuImage.ConversionParams
                {
                    inputRect = new RectInt(0, 0, image.width, image.height),
                    outputDimensions = new Vector2Int(image.width, image.height),
                    outputFormat = TextureFormat.R8,
                });

                await UniTask.WaitUntil(() => request.status.IsDone());

                if (request.status != XRCpuImage.AsyncConversionStatus.Ready)
                {
                    Debug.LogErrorFormat("Request failed with status {0}", request.status);
                    return Array.Empty<Result>();
                }

                await UniTask.SwitchToThreadPool();

                var rawData = request.GetData<byte>();
                var width = request.conversionParams.outputDimensions.x;
                var height = request.conversionParams.outputDimensions.y;
                var luminanceSource = new NativeArrayLuminanceSource(rawData, width, height);

                var results = barcodeReader.DecodeMultiple(luminanceSource);

                await UniTask.SwitchToMainThread();

                return results ?? Array.Empty<Result>();
            }
            finally
            {
                request.Dispose();
            }
        }
    }
}
