using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloLab.ARFoundationQRTracking.Samples
{
    public class TrackedQRVisualizer : MonoBehaviour
    {
        [SerializeField]
        private GameObject borderQuad;

        [SerializeField]
        private TextMesh textMesh;

        private ARTrackedQRImage arTrackedQRImage;

        private void Awake()
        {
            arTrackedQRImage = GetComponentInParent<ARTrackedQRImage>();
            if (arTrackedQRImage != null)
            {
                textMesh.text = arTrackedQRImage.Text;
            }

            UpdateGameObject();
        }

        private void Update()
        {
            UpdateGameObject();
        }

        private void UpdateGameObject()
        {
            if (arTrackedQRImage == null)
            {
                return;
            }

            var trackingReliable = arTrackedQRImage.TrackingReliable;
            borderQuad.SetActive(trackingReliable);
            textMesh.gameObject.SetActive(trackingReliable);

            var size = arTrackedQRImage.Size;
            borderQuad.transform.localScale = new Vector3(size.x, size.y, 1);

            textMesh.transform.localPosition = new Vector3(-size.x / 2, 0, size.y / 2 + 0.004f);
        }
    }
}
