using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Camera cam;

    public PlayerControl player;

    public Vector2 camOffset = new Vector2(1, 0);
    public float camZoom = 2;

    public bool followPlayer = true;

    // [HideInInspector]
    public List<Weapon> droppedWeapons;

    public Text weaponAmmoStat;
    public Text healthStat;

    public Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        Weapon[] allWeapons = FindObjectsOfType<Weapon>();
        foreach (Weapon allWp in allWeapons) {
            droppedWeapons.Add(allWp);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //camera
        if (followPlayer)
        {
            Vector3 playerPos = Vector3.zero;
            playerPos.x = player.transform.position.x;
            playerPos.y = player.transform.position.y;
            playerPos.z = cam.transform.position.z;

            cam.transform.position = Vector3.Lerp(cam.transform.position, playerPos + new Vector3(camOffset.x, camOffset.y), 2f * Time.deltaTime);
        }

        //zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camZoom, 1f * Time.deltaTime);

        //ammo
        if (player.currentWeapon)
        {
            weaponAmmoStat.text = player.currentWeapon.model + ": " + player.currentWeapon.ammo;
        }
        else
        {
            weaponAmmoStat.text = "x";
        }
        //health
        healthStat.text = "+" + player.health;
    }
	
	
    public void AnnouncePlayerInSight()
    {
        Debug.Log("FUCK, CAMERA SAW ME");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
