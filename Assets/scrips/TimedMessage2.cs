using UnityEngine;
using TMPro;    // TextMeshPro를 쓸 경우 필요

public class TimedMessage2 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject Textback;

    private Coroutine currentCoroutine; // 현재 코루틴 추적

    // 텍스트 상자를 N초 동안 표시하는 함수
    public void ShowMessage(string message, float duration)
    {
        // 기존 동작 중인 코루틴이 있다면 중단
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ShowMessageCoroutine(message, duration));
    }

    private System.Collections.IEnumerator ShowMessageCoroutine(string message, float duration)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        Textback.SetActive(true);

        yield return new WaitForSeconds(duration);

        messageText.gameObject.SetActive(false);
        Textback.SetActive(false);
        currentCoroutine = null;
    }

    // 메시지를 즉시 숨기는 함수
    public void HideMessage()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        messageText.gameObject.SetActive(false);
        Textback.SetActive(false);
    }
}


