using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asansor : MonoBehaviour
{
    public Vector3 initY;
    public Vector3 finalY;
    private bool isDown = true;
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {
        finalY = new Vector3(transform.position.x, 20, transform.position.z);
        initY = new Vector3(transform.position.x, 0, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position == finalY)
        {
            isDown = false;
        }
        else if (transform.position == initY)
            isDown = true;

        if (isDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalY, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initY, speed * Time.deltaTime);
        }
    }
}
