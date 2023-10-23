// Code referenced from the following link:
// https://github.com/Unity-Technologies/arfoundation-samples/issues/1065#issuecomment-1631739237

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.ARFoundation;
#endif

namespace HoloLab.ARFoundationQRTracking.iOS
{
    public static class ARKitImageScaleEstimationInterop
    {
#if UNITY_IOS && !UNITY_EDITOR
        public struct NativePtrData
        {
            public int version;
            public IntPtr sessionPtr;
        }

        public static bool EnableScaleEstimation(ARSession session)
        {
            if (session.subsystem is UnityEngine.XR.ARKit.ARKitSessionSubsystem subsystem)
            {
                // Make sure we have a native ptr
                if (subsystem.nativePtr == IntPtr.Zero)
                {
                    return false;
                }

                // Get the session ptr from the native ptr data
                IntPtr ptr = Marshal.PtrToStructure<NativePtrData>(subsystem.nativePtr).sessionPtr;
                if (ptr == IntPtr.Zero)
                {
                    return false;
                }

                var result = ARKitEnableScaleEstimation(ptr);
                return result;
            }

            return false;
        }

        public static float GetEstimatedScale(ARTrackedImage image)
        {
            // Get the session ptr from the native ptr data
            IntPtr ptr = Marshal.PtrToStructure<NativePtrData>(image.nativePtr).sessionPtr;
            if (ptr == IntPtr.Zero)
            {
                return 1;
            }

            return (float)ARKitGetImageAnchorEstimatedScaleFactor(ptr);
        }


        [DllImport("__Internal", EntryPoint = "ARKit_EnableImageScaleEstimation")]
        private static extern bool ARKitEnableScaleEstimation(IntPtr sessionPtr);

        [DllImport("__Internal", EntryPoint = "ARKit_GetImageAnchorEstimatedScaleFactor")]
        private static extern double ARKitGetImageAnchorEstimatedScaleFactor(IntPtr imageAnchorPtr);
#endif
    }
}
