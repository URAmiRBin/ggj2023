using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxo : MonoBehaviour {
    [SerializeField] private Rigidbody2D boxRb;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.CompareTag("Bullet")) {
            DropBox();
        }
    }

    private void DropBox() {
        boxRb.isKinematic = false;
        boxRb.transform.SetParent(null);
    }
}
