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
- [UniTask](https://github.com/Cysharp/UniTask#upm-package)

### Install

Open `Packages\manifest.json` and add the following lines to the "dependencies" section.

```json
"jp.co.hololab.arfoundationqrtracking": "https://github.com/HoloLabInc/ARFoundationQRTracking-Unity.git?path=packages/jp.co.hololab.arfoundationqrtracking",
"jp.co.hololab.arfoundationqrtracking.libraries": "https://github.com/HoloLabInc/ARFoundationQRTracking-Unity.git?path=packages/jp.co.hololab.arfoundationqrtracking.libraries",
```

## Usage

Add the following components to the "AR Session Origin" object.

- `AR Tracked Image Manager`
- `AR Foundation QR Tracker`

Then set the `Max Number Of Moving Images` value in the `AR Tracked Image Manager` to more than 0.

<img width="480" alt="AR Session Origin setting" src="https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/assets/4415085/5af7a2e0-65eb-4c28-81e3-85b7f0a448de">
<br>
<br>

### QR tracking

If you want to make a GameObject follow a specific QR code, please use the QRTracker prefab or the QRTracker script.

<img width="357" alt="image" src="https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/assets/4415085/476ce0cc-c001-4075-801e-8af22a381468">

### Event handling

If you want to implement more complex behavior, you can use recognition events.
When the QR Code is detected, the `OnTrackedQRImagesChanged` event of `ARFoundationQRTracker` is called.
For more details, please refer to the sample scripts.

- [QRVisualizationSample.cs](https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/blob/doc/readme/packages/jp.co.hololab.arfoundationqrtracking/Samples~/ARFoundationTrackingSample/Scripts/QRVisualizationSample.cs)
- [TrackedQRVisualizer.cs](https://github.com/HoloLabInc/ARFoundationQRTracking-Unity/blob/doc/readme/packages/jp.co.hololab.arfoundationqrtracking/Samples~/ARFoundationTrackingSample/Scripts/TrackedQRVisualizer.cs)

## Sample scenes

Open "Package Manager" and select "AR Foundation QR Tracking" from the package list.
Then click the "Import" button of ARFoundationTrackingSample.

### `ARFoundationQRTrackerSample.unity`

This is a sample scene that displays a cube object following a specific QR code using QRTracker prefab.

### `ARFoundationQRVisualizationSample.unity`

This is a sample scene for event handling.

## Limitations

QR codes which contain non-ASCII characters are not supported.

## License

Apache License 2.0

## Acknowledgements

QR Code is a registered trademark of DENSO WAVE INCORPORATED.
