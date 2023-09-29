using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace HoloLab.ARFoundationQRTracking.Samples
{
    public class WebCamTextureQRReconstructionSample : MonoBehaviour
    {
        [SerializeField]
        private int webCamTextureWidth = 640;

        [SerializeField]
        private int webCamTextureHeight = 480;

        [SerializeField]
        private RawImage cameraImage = null;

        [SerializeField]
        private RawImage reconstructedImage = null;

        private BarcodeReader barcodeReader;
        private WebCamTexture webCamTexture;

        private Texture2D reconstructedTexture;

        private readonly QRReconstructor qrReconstructor = new QRReconstructor();

        private void Start()
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

            webCamTexture = new WebCamTexture(webCamTextureWidth, webCamTextureHeight);
            webCamTexture.Play();

            cameraImage.texture = webCamTexture;
            var cameraImageTransform = cameraImage.GetComponent<RectTransform>();
            var cameraImageHeight = cameraImageTransform.sizeDelta.y;
            cameraImageTransform.sizeDelta = new Vector2(webCamTexture.width * cameraImageHeight / webCamTexture.height, cameraImageHeight);

            reconstructedTexture = new Texture2D(256, 256);
            reconstructedImage.texture = reconstructedTexture;
        }

        private void Update()
        {
            var result = Decode();
            if (result != null)
            {
                Reconstruct(result);
            }
        }

        private Result Decode()
        {
            var pixels = webCamTexture.GetPixels32();
            var w = webCamTexture.width;
            var h = webCamTexture.height;
            var result = barcodeReader.Decode(pixels, w, h);

            return result;
        }

        private void Reconstruct(Result result)
        {
            var reconstructResult = qrReconstructor.ReconstructColor32Image(result, reconstructedTexture.width, reconstructedTexture.height, margin: 2);
            if (reconstructResult.Success)
            {
                reconstructedTexture.SetPixels32(reconstructResult.Data);
                reconstructedTexture.Apply();
            }
        }
    }
}
