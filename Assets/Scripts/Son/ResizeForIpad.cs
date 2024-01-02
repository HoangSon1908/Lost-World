using System.Collections.Generic;
using UnityEngine;

public class ResizeRectTransformForIPadByName : MonoBehaviour
{
    // List of RectTransform objects to resize
    public List<RectTransform> rectTransformsToResize;

    void Start()
    {
        // Detect device by name and resize for iPad
        DetectAndResizeForIPadByName();
    }

    void DetectAndResizeForIPadByName()
    {
        // Get device model or name
        string deviceModel = SystemInfo.deviceModel;

        // Check if the device name contains "iPad"
        if (deviceModel.Contains("iPad"))
        {
            ResizeRectTransforms(300f, 320f); // Resize for iPad
        }
    }

    void ResizeRectTransforms(float newWidth, float newHeight)
    {
        foreach (RectTransform rectTransform in rectTransformsToResize)
        {
            if (rectTransform != null)
            {
                // Set the width and height of the RectTransform
                rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
            }
        }
    }
}
