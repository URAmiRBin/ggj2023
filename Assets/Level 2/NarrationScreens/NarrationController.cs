using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationController : MonoBehaviour {
    [SerializeField] NarrationSequence beginningSequence;

    private void Start() {
        StartCoroutine(ShowBeginningSequennce());
    }

    private IEnumerator ShowBeginningSequennce() {
        yield return new WaitForSeconds(.5f);
        BlackScreenNarrationController.Instance.ShowNarrationScreen(beginningSequence);
    }
}
