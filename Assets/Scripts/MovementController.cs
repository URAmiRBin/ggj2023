using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform Player;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            Player.position += Vector3.right*speed;

        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)){
Player.position += Vector3.left*speed;
        }
    }
}
