using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenNarrationController : MonoBehaviourSingletion<BlackScreenNarrationController> {
    [SerializeField] private Camera cam;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private CanvasGroupFader textFader;
    [SerializeField] private CanvasGroupFader panelFader;
    [SerializeField] private Text narrationText;

    public void ShowNarrationScreen(NarrationSequence sequence) {
        StartCoroutine(ShowNarrationScreenCoroutine(sequence));
    }

    private IEnumerator ShowNarrationScreenCoroutine(NarrationSequence sequence) {
        NarrationData[] narrationData = sequence.narrationDatas;
        backgroundImage.sprite = sequence.image;
        cam.backgroundColor = sequence.imageColorMultiplier;
        backgroundImage.color = sequence.imageColorMultiplier;
        narrationText.color = sequence.textColor;
        panelFader.FadeIn();
        for (int i = 0; i < narrationData.Length; i++) {
            narrationText.text = narrationData[i].text;
            textFader.FadeIn(.5f);
            yield return new WaitForSeconds(narrationData[i].duration);
            textFader.FadeOut(.5f);
            yield return new WaitForSeconds(.5f);
        }
        panelFader.FadeOut();
        sequence.onFinished?.Invoke();
    }
}
