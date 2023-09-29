using System.Collections;
using System.Collections.Generic;
using HoloLab.ARFoundationQRTracking;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ZXing;
using ZXing.QrCode;

public class QRReconstructorTests
{
    private static IEnumerable<TestCaseData> ReconstructImage_RawBytesLengthMatch_TestCases()
    {
        var errorCorrectionLevels = new[] { "L", "M", "Q", "H" };
        foreach (var errorCorrectionLevel in errorCorrectionLevels)
        {
            for (var version = 1; version <= 40; version++)
            {
                yield return new TestCaseData(errorCorrectionLevel, version);
            }
        }
    }

    [TestCaseSource(nameof(ReconstructImage_RawBytesLengthMatch_TestCases))]
    public void ReconstructImage_RawBytesLengthMatch(string errorCorrectionLevel, int version)
    {
        // Create QR result
        var size = 512;
        var qrImage = CreateQRImage(errorCorrectionLevel, version, size, size);
        var qrResult = DecodeQRImage(qrImage, size, size);
        Assert.That(qrResult, Is.Not.Null);

        // Reconstruct image
        var reconstructor = new QRReconstructor();
        var reconstructedImage = reconstructor.ReconstructColor32Image(qrResult, size, size);
        Assert.That(reconstructedImage.Success, Is.True);

        // Decode QR
        var reconstructedResult = DecodeQRImage(reconstructedImage.Data, size, size);
        Assert.That(reconstructedResult, Is.Not.Null);

        // Check if versions match
        Assert.That(reconstructedResult.RawBytes, Is.EquivalentTo(qrResult.RawBytes));
    }

    private Color32[] CreateQRImage(string errorCorrectionLevel, int version, int width, int height)
    {
        var margin = 2;

        var options = new QrCodeEncodingOptions
        {
            Width = width,
            Height = height,
            Margin = margin,
        };

        options.Hints[EncodeHintType.ERROR_CORRECTION] = errorCorrectionLevel;
        options.Hints[EncodeHintType.QR_VERSION] = version;

        var text = "text";

        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = options,
        };
        var pixels = writer.Write(text);
        return pixels;
    }

    private Result DecodeQRImage(Color32[] pixels, int width, int height)
    {
        var barcodeReader = new BarcodeReader
        {
            AutoRotate = false,

            Options = new ZXing.Common.DecodingOptions
            {
                TryHarder = false,
                PureBarcode = true,
                PossibleFormats = new[]
                {
                    BarcodeFormat.QR_CODE
                }
            }
        };

        var result = barcodeReader.Decode(pixels, width, height);
        return result;
    }
}
