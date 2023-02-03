using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rg.velocity.magnitude > 1)
        {
            float AngleRad = Mathf.Atan2(rg.velocity.y, rg.velocity.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            rg.transform.rotation = Quaternion.Euler(0, 0, AngleDeg) * Quaternion.Euler(0, 0, 90);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
