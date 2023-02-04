using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomTrigger : MonoBehaviour
{
    public string targetTag;
    public UnityEvent action;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.CompareTag(targetTag)) {
            action?.Invoke();
        }
    }
}
