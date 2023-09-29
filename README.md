# ARFoundationQRTracking-Unity

ARFoundationQRTracking-Unity is a module for QR Code tracking with AR Foundation.

<img alt="Demo video of ARFoundationQRTracking-Unity" src="https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/assets/4415085/033aadad-53cd-46d3-9cc1-111be88c7e4c" width="320px">

## Target Devices

- ARKit supported devices
- ARCore supported devices

## Getting Started

### Prerequisites

Import the following packages into your Unity project.

- AR Foundation
- ARKit XR Plugin (iOS)
- ARCore XR Plugin (Android)
- [UniTask](https://github.com/Cysharp/UniTask)

### Install

Open `Packages\manifest.json` and add the following lines to the "dependencies" section.

```json
"jp.co.hololab.arfoundationqrtracking": "https://github.com/HoloLabInc/ARFoundationQRTracking-Unity.git?path=packages/jp.co.hololab.arfoundationqrtracking",
"jp.co.hololab.arfoundationqrtracking.libraries": "https://github.com/HoloLabInc/ARFoundationQRTracking-Unity.git?path=packages/jp.co.hololab.arfoundationqrtracking.libraries",
```

## Usage

### a. Use sample scene

Open "Package Manager" and select "AR Foundation QR Tracking" from the package list.  
Then click the "Import" button of ARFoundationTrackingSample.

`ARFoundationQRVisualizationSample.unity` is a sample scene for QR Code tracking.

### b. Use your scene

Add the following components to the "AR Session Origin" object.

- `AR Tracked Image Manager`
- `AR Foundation QR Tracker`
- `Enable Scale Estimation for AR Kit`

Then set `Max Number Of Moving Images` of `AR Tracked Image Manager` to a value other than 0.

<img width="480" alt="AR Session Origin settings" src="https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/assets/4415085/86a19b74-0bdb-43fe-8b60-a042df34f6e9">
<br>
<br>

When the QR Code is detected, the `OnTrackedQRImagesChanged` event of `ARFoundationQRTracker` is called.  
For more details, please refer to the sample scripts.

- [QRVisualizationSample.cs](https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/blob/doc/readme/packages/jp.co.hololab.arfoundationqrtracking/Samples~/ARFoundationTrackingSample/Scripts/QRVisualizationSample.cs)
- [TrackedQRVisualizer.cs](https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/blob/doc/readme/packages/jp.co.hololab.arfoundationqrtracking/Samples~/ARFoundationTrackingSample/Scripts/TrackedQRVisualizer.cs)

## License

Apache License 2.0

## Acknowledgements

QR Code is a registered trademark of DENSO WAVE INCORPORATED.
