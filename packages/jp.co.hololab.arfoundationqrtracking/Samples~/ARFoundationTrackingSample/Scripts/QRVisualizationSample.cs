using HoloLab.ARFoundationQRTracking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLab.ARFoundationQRTracking.Samples
{
    public class QRVisualizationSample : MonoBehaviour
    {
        [SerializeField]
        private GameObject qrVisualizerPrefab;

        private ARFoundationQRTracker qrTracker;

        private void Awake()
        {
            qrTracker = FindObjectOfType<ARFoundationQRTracker>();

            qrTracker.OnTrackedQRImagesChanged += QRTracker_OnTrackedQRImagesChanged;
        }

        private void QRTracker_OnTrackedQRImagesChanged(ARTrackedQRImagesChangedEventArgs eventArgs)
        {
            foreach (var addedQR in eventArgs.Added)
            {
                Debug.Log($"Marker detected: {addedQR.Text}");
                Instantiate(qrVisualizerPrefab, addedQR.transform);
            }

            foreach (var removedQR in eventArgs.Removed)
            {
                Debug.Log($"Marker lost: {removedQR.Text}");
            }
        }
    }
}
