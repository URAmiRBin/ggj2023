using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float Health;

    float fireRate;

    public Transform weaponAim;
    public GameObject bowPrefab;

    public Animator animator;

    PlayerControl _player;

    public Image healthImg;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerControl>();
    }

    private void Update()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene("EnginScreen");
            return;
        }

        Vector2 playerDir = _player.transform.position - transform.position;
        //attack
        if (playerDir.magnitude < 15)
        {
            healthImg.transform.parent.gameObject.SetActive(true);
            animator.SetBool("isAttacking", true);
            fireRate -= Time.deltaTime;
            if (fireRate < 0)
            {
                fireRate = Random.Range(0.25f, 1);
                RandomShoot();
            }
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        //ui
        healthImg.fillAmount = Health / 100f;
    }

    void RandomShoot()
    {
        GameObject bullet = Instantiate(bowPrefab, weaponAim.transform.position, weaponAim.transform.rotation * Quaternion.Euler(0, 0, 90));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.rg.AddForce(-weaponAim.right * 15 + (weaponAim.up * Random.Range(-20, 10)), ForceMode2D.Impulse);
        Destroy(bullet, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }

        if (collision.transform.name.Contains("Player"))
        {
            Health -= 10;
        }
    }
}
