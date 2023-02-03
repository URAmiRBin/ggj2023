using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Camera cam;

    public PlayerControl player;

    public List<Weapon> droppedWeapons;

    public Text weaponAmmoStat;

    public Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //camera
        Vector3 playerPos = Vector3.zero;
        playerPos.x = player.transform.position.x;
        playerPos.y = player.transform.position.y;
        playerPos.z = cam.transform.position.z;

        float PlayerHeight = 0;
        cam.transform.position = Vector3.Lerp(cam.transform.position, playerPos + (Vector3.up * PlayerHeight), 2f * Time.deltaTime);

        //ammo
        if (player.currentWeapon)
        {
            weaponAmmoStat.text = player.currentWeapon.model + " - " + player.currentWeapon.ammo;
        }
        else
        {
            weaponAmmoStat.text = "-";
        }
    }
}
