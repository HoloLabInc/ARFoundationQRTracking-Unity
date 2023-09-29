using UnityEngine;
#if UNITY_IOS
using UnityEngine.XR.ARFoundation;
#endif

namespace HoloLab.ARFoundationQRTracking.iOS
{
    public class EnableScaleEstimationForARKit : MonoBehaviour
    {
#if UNITY_IOS
        private ARSession session;
        private bool isScaleEstimationEnabled = false;

        private void Awake()
        {
            session = FindObjectOfType<ARSession>();
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
