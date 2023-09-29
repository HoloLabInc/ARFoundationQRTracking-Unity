// Code referenced from the following link:
// https://github.com/Unity-Technologies/arfoundation-samples/issues/1065#issuecomment-1631739237

#import <ARKit/ARKit.h>

bool ARKit_EnableImageScaleEstimation(void* sessionPtr) {
    if (!sessionPtr || !@available(iOS 13, *)) {
        return false;
    }

    ARSession* session = (__bridge ARSession*)sessionPtr;
    if (![session.configuration isKindOfClass:[ARWorldTrackingConfiguration class]]) {
        return false;
    }

    ARWorldTrackingConfiguration * config = (ARWorldTrackingConfiguration *)session.configuration;
    if (config.automaticImageScaleEstimationEnabled) {
        return true;
    }

    config.automaticImageScaleEstimationEnabled = true;
    [session runWithConfiguration:config];
    return true;
}


double ARKit_GetImageAnchorEstimatedScaleFactor(void* imageAnchorPtr) {
    if (@available(iOS 13, *)) {
        ARImageAnchor* anchor = (__bridge ARImageAnchor*)imageAnchorPtr;
        return anchor.estimatedScaleFactor;
    }
    return (double)1.0;
}