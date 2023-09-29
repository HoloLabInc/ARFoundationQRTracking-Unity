using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace HoloLab.ARFoundationQRTracking
{
    public class ARTrackedQRImage : MonoBehaviour
    {
        private float marginRatio;

        public ARTrackedImage ARTrackedImage { get; private set; }

        public bool TrackingReliable
        {
            get
            {
#if UNITY_IOS
                var estimatedScale = HoloLab.ARFoundationQRTracking.iOS.ARKitImageScaleEstimationInterop.GetEstimatedScale(ARTrackedImage);
                return estimatedScale != 1.0;
#endif
                return true;
            }
        }

        public string Text { get; private set; }

        public Vector2 Size
        {
            get
            {
                var imageSize = ARTrackedImage.size;
#if UNITY_IOS
                imageSize *= HoloLab.ARFoundationQRTracking.iOS.ARKitImageScaleEstimationInterop.GetEstimatedScale(ARTrackedImage);
#endif
                return imageSize * (1 - marginRatio);
            }
        }

        public void Initialize(ARTrackedImage arTrackedImage, string text, float marginRatio)
        {
            ARTrackedImage = arTrackedImage;
            Text = text;
            this.marginRatio = marginRatio;
        }
    }
}
