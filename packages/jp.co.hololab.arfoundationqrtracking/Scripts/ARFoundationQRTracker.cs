using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using ZXingResult = ZXing.Result;

#if UNITY_IOS
using HoloLab.ARFoundationQRTracking.iOS;
#endif

namespace HoloLab.ARFoundationQRTracking
{
    public struct ARTrackedQRImagesChangedEventArgs
    {
        public List<ARTrackedQRImage> Added { get; private set; }
        public List<ARTrackedQRImage> Updated { get; private set; }
        public List<ARTrackedQRImage> Removed { get; private set; }

        public ARTrackedQRImagesChangedEventArgs(
                   List<ARTrackedQRImage> added,
                   List<ARTrackedQRImage> updated,
                   List<ARTrackedQRImage> removed)
        {
            this.Added = added;
            this.Updated = updated;
            this.Removed = removed;
        }
    }

    public class ARFoundationQRTracker : MonoBehaviour
    {
        [SerializeField]
        private int qrDetectionIntervalMilliseconds = 1000;

        public int QRDetectionIntervalMilliseconds
        {
            set => qrDetectionIntervalMilliseconds = value;
            get => qrDetectionIntervalMilliseconds;
        }

        private ARCameraManager cameraManager;
        private ARTrackedImageManager arTrackedImageManager;
        private MutableRuntimeReferenceImageLibrary imageLibrary;

        private bool qrDetectionProcessing;
        private float latestQRDetectionTime;

        private readonly ARFoundationQRDetector qrDetector = new ARFoundationQRDetector();
        private readonly QRReconstructor qrReconstructor = new QRReconstructor();

        private Dictionary<string, ZXingResult> trackedZXingResults = new Dictionary<string, ZXingResult>();
        private Dictionary<ARTrackedImage, ARTrackedQRImage> trackedImageToTrackedQRDictionary
            = new Dictionary<ARTrackedImage, ARTrackedQRImage>();

        private const int qrImageMargin = 1;

        public event Action<ARTrackedQRImagesChangedEventArgs> OnTrackedQRImagesChanged;

        private void Awake()
        {
            cameraManager = FindObjectOfType<ARCameraManager>();
            if (cameraManager == null)
            {
                Debug.LogError("ARCameraManager not found in scene.");
                return;
            }

            arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
            if (arTrackedImageManager == null)
            {
                Debug.LogError("ARTrackedImageManager not found in scene.");
                return;
            }

#if UNITY_IOS
            if (arTrackedImageManager.requestedMaxNumberOfMovingImages == 0)
            {
                Debug.LogError("Max Number Of Moving Images of ARTrackedImagaManager should be more than 0.");
            }
#endif

            try
            {
                imageLibrary = arTrackedImageManager.CreateRuntimeLibrary() as MutableRuntimeReferenceImageLibrary;
                arTrackedImageManager.referenceLibrary = imageLibrary;
            }
            catch (NotSupportedException ex)
            {
                Debug.LogWarning(ex.Message);
            }

#if UNITY_IOS
            var enableScaleEstimationForARKit = FindObjectOfType<EnableScaleEstimationForARKit>();
            if (enableScaleEstimationForARKit == null)
            {
                gameObject.AddComponent<EnableScaleEstimationForARKit>();
            }
#endif
        }

        void OnEnable()
        {
            if (cameraManager != null)
            {
                cameraManager.frameReceived += OnCameraFrameReceived;
            }

            if (arTrackedImageManager != null)
            {
                arTrackedImageManager.trackedImagesChanged += ARTrackedImageManager_trackedImagesChanged;
            }
        }

        void OnDisable()
        {
            if (cameraManager != null)
            {
                cameraManager.frameReceived -= OnCameraFrameReceived;
            }

            if (arTrackedImageManager != null)
            {
                arTrackedImageManager.trackedImagesChanged -= ARTrackedImageManager_trackedImagesChanged;
            }
        }

        private void ARTrackedImageManager_trackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            var added = new List<ARTrackedQRImage>();
            var updated = new List<ARTrackedQRImage>();
            var removed = new List<ARTrackedQRImage>();

            foreach (var newImage in eventArgs.added)
            {
                var referenceImage = newImage.referenceImage;

                if (trackedZXingResults.TryGetValue(referenceImage.name, out var result) == false)
                {
                    continue;
                }

                if (ZXingQRUtility.TryGetCellSize(result, out var cellSize) == false)
                {
                    continue;
                }

                var marginRatio = (float)qrImageMargin * 2 / (cellSize + qrImageMargin * 2);

                var arTrackedQRInfo = newImage.gameObject.AddComponent<ARTrackedQRImage>();
                arTrackedQRInfo.Initialize(newImage, result.Text, marginRatio);
                trackedImageToTrackedQRDictionary[newImage] = arTrackedQRInfo;

                added.Add(arTrackedQRInfo);
            }

            foreach (var updatedImage in eventArgs.updated)
            {
                if (trackedImageToTrackedQRDictionary.TryGetValue(updatedImage, out var trackedQR))
                {
                    updated.Add(trackedQR);
                }
            }

            foreach (var removedImage in eventArgs.removed)
            {
                if (trackedImageToTrackedQRDictionary.TryGetValue(removedImage, out var trackedQR))
                {
                    removed.Add(trackedQR);
                }
            }

            if (added.Count > 0 || updated.Count > 0 || removed.Count > 0)
            {
                var qrEventArgs = new ARTrackedQRImagesChangedEventArgs(added, updated, removed);
                try
                {
                    OnTrackedQRImagesChanged?.Invoke(qrEventArgs);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        private async void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            if (qrDetectionProcessing)
            {
                return;
            }

            if (Time.realtimeSinceStartup < latestQRDetectionTime + qrDetectionIntervalMilliseconds * 0.001f)
            {
                return;
            }

            if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image) == false)
            {
                return;
            }

            qrDetectionProcessing = true;
            latestQRDetectionTime = Time.realtimeSinceStartup;

            var results = await qrDetector.DetectMultipleAsync(image);
            image.Dispose();

            foreach (var result in results)
            {
                if (IsTracked(result))
                {
                    continue;
                }

                var success = AddTrackingTarget(result);
                if (success)
                {
                    arTrackedImageManager.enabled = true;
                }
            }

            qrDetectionProcessing = false;
        }

        private bool AddTrackingTarget(ZXingResult result)
        {
            if (imageLibrary == null)
            {
                Debug.LogWarning("Image tracking is not available.");
                return false;
            }

            if (ZXingQRUtility.TryGetCellSize(result, out var cellSize) == false)
            {
                return false;
            }

            var markerSize = 256;
            if (cellSize > 64)
            {
                markerSize = 512;
            }

            var reconstructResult = qrReconstructor.ReconstructNativeArrayImage(result, markerSize, markerSize, margin: qrImageMargin);
            if (reconstructResult.Success == false)
            {
                return false;
            }

            var qrImageTexture = new Texture2D(markerSize, markerSize, TextureFormat.RGBA32, false);
            qrImageTexture.LoadRawTextureData(reconstructResult.Data);
            qrImageTexture.Apply();
            reconstructResult.Data.Dispose();

            var id = GenerateUniqueId();
            trackedZXingResults[id] = result;

#if UNITY_IOS
            float? widthInMeters = 1f;
#else
            float? widthInMeters = null;
#endif
            imageLibrary.ScheduleAddImageWithValidationJob(qrImageTexture, id, widthInMeters);

            return true;
        }

        private bool IsTracked(ZXingResult result)
        {
            return trackedZXingResults.Values.Any(x => IsSameZXingResult(x, result));
        }

        private string GenerateUniqueId()
        {
            while (true)
            {
                var id = Guid.NewGuid().ToString();
                if (trackedZXingResults.ContainsKey(id) == false)
                {
                    return id;
                }
            }
        }

        private static bool IsSameZXingResult(ZXingResult result1, ZXingResult result2)
        {
            if (result1.Text != result2.Text)
            {
                return false;
            }

            if (result1.RawBytes.Length != result2.RawBytes.Length)
            {
                return false;
            }

            object errorCorrectionLevel1 = null;
            object errorCorrectionLevel2 = null;
            result1.ResultMetadata?.TryGetValue(ResultMetadataType.ERROR_CORRECTION_LEVEL, out errorCorrectionLevel1);
            result2.ResultMetadata?.TryGetValue(ResultMetadataType.ERROR_CORRECTION_LEVEL, out errorCorrectionLevel2);
            if (errorCorrectionLevel1.Equals(errorCorrectionLevel2) == false)
            {
                return false;
            }

            object qrMaskPattern1 = null;
            object qrMaskPattern2 = null;
            result1.ResultMetadata?.TryGetValue(ResultMetadataType.QR_MASK_PATTERN, out qrMaskPattern1);
            result2.ResultMetadata?.TryGetValue(ResultMetadataType.QR_MASK_PATTERN, out qrMaskPattern2);
            if (qrMaskPattern1.Equals(qrMaskPattern2) == false)
            {
                return false;
            }

            return true;
        }
    }
}
