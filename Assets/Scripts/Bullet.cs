using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public Rigidbody2D rg;

    public bool reusable = false;
    public bool sticks = false;

    [HideInInspector]
    public float tention;

    public GameObject hitImpact;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rg.velocity.magnitude > 3 && rg.isKinematic == false)
        {
            float AngleRad = Mathf.Atan2(rg.velocity.y, rg.velocity.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            rg.transform.rotation = Quaternion.Euler(0, 0, AngleDeg) * Quaternion.Euler(0, 0, 90);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (reusable == false)
        {
            Destroy(gameObject);
            //impact
            if (hitImpact != null)
            {
                GameObject impact = Instantiate(hitImpact, transform.position, Quaternion.identity);
                impact.transform.localScale = Vector3.one * 0.25f;
                Destroy(impact, 3f);
            }
        }
        //damage
        if (collision.transform.tag == "Enemy")
        {
            Enemy enmy = collision.transform.GetComponent<Enemy>();
            if (enmy)
            {
                enmy.health -= 25;
            }
        }
        //
        if (sticks)
        {
            if (collision.transform.name.Contains("Arrow") ||
                collision.transform.tag == "Enemy" ||
                collision.transform.tag == "Player")
            {
                return;
            }
            if (rg)
            {
                rg.angularVelocity = 0;
                rg.velocity = Vector2.zero;
                rg.isKinematic = true;
            }
        }
    }
}
