using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace HoloLab.ARFoundationQRTracking
{
    public readonly struct QRTrackerResult<T>
    {
        public readonly bool Success;
        public readonly T Data;
        public readonly Exception Error;

        public QRTrackerResult(bool success, T data, Exception error)
        {
            Success = success;
            Data = data;
            Error = error;
        }

        public static QRTrackerResult<T> CreateSuccessResult(T data)
        {
            return new QRTrackerResult<T>(true, data, null);
        }

        public static QRTrackerResult<T> CreateErrorResult(Exception error)
        {
            return new QRTrackerResult<T>(false, default, error);
        }
    }

    public class QRReconstructor
    {
        public QRTrackerResult<NativeArray<byte>> ReconstructNativeArrayImage(Result result, int width, int height, int margin = 4)
        {
            var writer = new NativeArrayBarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE
            };

            return ReconstructImage(writer, result, width, height, margin);
        }

        public QRTrackerResult<Color32[]> ReconstructColor32Image(Result result, int width, int height, int margin = 4)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE
            };

            return ReconstructImage(writer, result, width, height, margin);
        }

        private static QRTrackerResult<T> ReconstructImage<T>(BarcodeWriter<T> writer, Result result, int width, int height, int margin)
        {
            var metadata = result.ResultMetadata;
            if (metadata == null)
            {
                var error = new ArgumentException("metadata not found in result");
                return QRTrackerResult<T>.CreateErrorResult(error);
            }

            if (metadata.TryGetValue(ResultMetadataType.ERROR_CORRECTION_LEVEL, out var errorCorrectionLevel) == false)
            {
                var error = new ArgumentException("Error correction level not found in result");
                return QRTrackerResult<T>.CreateErrorResult(error);
            }

            if (metadata.TryGetValue(ResultMetadataType.QR_MASK_PATTERN, out var maskPattern) == false)
            {
                var error = new ArgumentException("Mask pattern not found in result");
                return QRTrackerResult<T>.CreateErrorResult(error);
            }

            var text = result.Text;
            if (IsAscii(text) == false)
            {
                var error = new ArgumentException("Non-ASCII characters not supported");
                return QRTrackerResult<T>.CreateErrorResult(error);
            }

            if (ZXingQRUtility.TryGetVersion(result, out var version) == false)
            {
                var error = new ArgumentException("Not supported version");
                return QRTrackerResult<T>.CreateErrorResult(error);
            }

            var options = new QrCodeEncodingOptions
            {
                Width = width,
                Height = height,
                Margin = margin,
            };

            options.Hints[EncodeHintType.ERROR_CORRECTION] = errorCorrectionLevel;
            options.Hints[EncodeHintType.QR_MASK_PATTERN] = maskPattern;
            options.Hints[EncodeHintType.QR_VERSION] = version;

            writer.Options = options;

            try
            {
                var image = writer.Write(text);
                return QRTrackerResult<T>.CreateSuccessResult(image);
            }
            catch (Exception ex)
            {
                return QRTrackerResult<T>.CreateErrorResult(ex);
            }
        }

        private static bool IsAscii(string text)
        {
            foreach (char c in text)
            {
                if (c > 127)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
