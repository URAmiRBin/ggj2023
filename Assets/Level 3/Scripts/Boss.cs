using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform weaponAim;
    public Weapon currentWeapon;
    public float powerShot;

    public Animator animator;
    public List<Transform> directions;

    public float Health;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartShooting());
    }

    void Shoot(Vector2 direction)
    {
            GameObject bullet = Instantiate(currentWeapon.bulletType, weaponAim.transform.position + (weaponAim.transform.right * 1f), weaponAim.transform.rotation * Quaternion.Euler(0, 0, 90));
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.rg.AddForce(direction * bulletScript.speed * powerShot, ForceMode2D.Impulse);
    }

    private IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(3);

        foreach (Transform direction in directions)
        {
            Shoot(direction.right);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
