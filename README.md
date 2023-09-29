# ARFoundationQRTracking-Unity

ARFoundationQRTracking-Unity is a module for QR Code tracking with AR Foundation.

<img alt="Demo video of ARFoundationQRTracking-Unity" src="https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/assets/4415085/3b0f2488-ac65-4f5b-90a7-84e4c6c29cce" width="320px">

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

<img width="480" alt="AR Session Origin settings" src="https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/assets/4415085/88cbd96d-0bff-42f3-b909-4a33bee9b687">

## License

Apache License 2.0

## Acknowledgements

QR Code is a registered trademark of DENSO WAVE INCORPORATED.
