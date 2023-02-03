using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFader : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float duration = 1) {
        StartCoroutine(ChangeCanvasGroupAlpha(1, duration));
    }

    public void FadeOut(float duration = 1) {
        StartCoroutine(ChangeCanvasGroupAlpha(0, duration));
    }

    private IEnumerator ChangeCanvasGroupAlpha(float targetAlpha, float duration) {
        float t = 0;
        float initialAlpha = canvasGroup.alpha;
        while (t < duration) {
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
