using UnityEngine;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARFoundation;
#endif

namespace HoloLab.ARFoundationQRTracking.iOS
{
    public class EnableScaleEstimationForARKit : MonoBehaviour
    {
#if UNITY_IOS && !UNITY_EDITOR
        private ARSession session;
        private bool isScaleEstimationEnabled = false;

        private void Awake()
        {
            session = FindObjectOfType<ARSession>();
            if (session == null)
            {
                Debug.LogError("ARSession not found in scene.");
            }
        }

        private void Update()
        {
            if (isScaleEstimationEnabled == false && session != null)
            {
                isScaleEstimationEnabled = ARKitImageScaleEstimationInterop.EnableScaleEstimation(session);
            }
        }
#endif
    }
}
