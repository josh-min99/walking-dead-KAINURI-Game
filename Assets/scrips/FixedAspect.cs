using UnityEngine;

public class FixedAspect : MonoBehaviour
{
    int prevWidth;
    int prevHeight;

    void Start()
    {
        SetResolution();
        prevWidth = Screen.width;
        prevHeight = Screen.height;
    }

    void Update()
    {
        if (Screen.width != prevWidth || Screen.height != prevHeight)
        {
            SetResolution();
            prevWidth = Screen.width;
            prevHeight = Screen.height;
        }
    }

    void SetResolution()
    {
        int setWidth = 1920; // 원하는 기준 너비
        int setHeight = 1080; // 원하는 기준 높이
        float targetAspect = (float)setWidth / setHeight;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}

