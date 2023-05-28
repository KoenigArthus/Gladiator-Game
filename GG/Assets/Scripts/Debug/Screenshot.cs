using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    private int screenshotCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            string screenshotName = string.Format("DebugScreenshot{0}.png", screenshotCount);
            ScreenCapture.CaptureScreenshot(screenshotName);
            Debug.Log("Took Screenshot: " + screenshotName);
            screenshotCount++;
        }
    }
}
