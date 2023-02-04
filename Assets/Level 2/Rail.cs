using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour {
    [SerializeField] private Transform carriedObject;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private float duration;

    private void Awake() {
        carriedObject.transform.position = p1.transform.position;
    }

    private void Start() {
        StartCoroutine(MoveRail());
    }

    private IEnumerator MoveRail() {
        float t = 0;
        while (t < duration) {
            carriedObject.transform.position = Vector3.Lerp(p1.transform.position, p2.transform.position, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        t = 0;
        while (t < duration) {
            carriedObject.transform.position = Vector3.Lerp(p2.transform.position, p1.transform.position, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        yield return MoveRail();
    }
}
