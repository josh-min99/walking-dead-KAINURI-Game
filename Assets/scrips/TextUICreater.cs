using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TextUICreater : MonoBehaviour
{
    public TMP_FontAsset myFontAsset;
    private Canvas FindOrCreateCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }
        return canvas;
    }

    public GameObject CreateTextUI(Vector2 anchoredPosition, string text, int size = 30, Color? color = null)
    {
        Canvas canvas = FindOrCreateCanvas();

        GameObject go = new GameObject("DynamicTextUI");
        go.transform.SetParent(canvas.transform);

        RectTransform rectTransform = go.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(250, 50);

        TMP_Text tmpText = go.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.fontSize = size;
        tmpText.font = myFontAsset;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = color ?? Color.white; // 색상 지정, 없으면 기본 하얀색
        tmpText.enableWordWrapping = false;
        return go;
    }


}

