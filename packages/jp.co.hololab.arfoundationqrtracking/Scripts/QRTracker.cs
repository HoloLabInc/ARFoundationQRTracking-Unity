using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLab.ARFoundationQRTracking
{
    public class QRTracker : MonoBehaviour
    {
        [SerializeField]
        private string targetQRText;

        private ARFoundationQRTracker qrTracker;

        private void Awake()
        {
            qrTracker = FindObjectOfType<ARFoundationQRTracker>();

            if (qrTracker == null)
            {
                Debug.LogError($"{nameof(ARFoundationQRTracker)} not found in scene.");
            }
            else
            {
                qrTracker.OnTrackedQRImagesChanged += QRTracker_OnTrackedQRImagesChanged;
            }

            HideObject();
        }

        private void OnDestroy()
        {
            if (qrTracker != null)
            {
                qrTracker.OnTrackedQRImagesChanged -= QRTracker_OnTrackedQRImagesChanged;
            }
        }

        private void QRTracker_OnTrackedQRImagesChanged(ARTrackedQRImagesChangedEventArgs eventArgs)
        {
            foreach (var addedQR in eventArgs.Added)
            {
                if (IsTarget(addedQR))
                {
                    if (addedQR.TrackingReliable)
                    {
                        ShowObject(addedQR);
                    }
                    return;
                }
            }

            foreach (var updatedQR in eventArgs.Updated)
            {
                if (IsTarget(updatedQR))
                {
                    if (updatedQR.TrackingReliable)
                    {
                        ShowObject(updatedQR);
                    }
                    return;
                }
            }

            foreach (var removedQR in eventArgs.Removed)
            {
                if (IsTarget(removedQR))
                {
                    HideObject();
                    return;
                }
            }
        }

        private void ShowObject(ARTrackedQRImage image)
        {
            var imageTransform = image.transform;
            transform.SetPositionAndRotation(imageTransform.position, imageTransform.rotation);
            gameObject.SetActive(true);
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }

        private bool IsTarget(ARTrackedQRImage image)
        {
            if (string.IsNullOrEmpty(targetQRText))
            {
                return true;
            }

            return image.Text == targetQRText;
        }
    }
}
