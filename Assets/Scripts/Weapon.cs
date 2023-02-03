using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string model;
    public int ammo;

    public GameObject bulletType;


    public Rigidbody2D rg;
    public Collider2D col;
}
