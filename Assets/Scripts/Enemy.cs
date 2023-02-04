using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    bool died = false;

    public float attackRate = 0.25f;
    float attackTimer;
    public float attackRange = 4f;

    public float patrolSpeed = 1f;
    float patrolTimer;
    float patrolDestination;

    private bool isFacingRight = false;

    private Vector2 initalPos;
    public enum EnemyState { Patrol, Offensive, Attacking };
    public EnemyState enemyState;
    public Rigidbody2D rig;
    public Animator anim;

    public GameObject deathParticle;

    public Transform weaponAim;

    public GameObject bulletPrefab;
    public GameObject bulletHit;

    GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        initalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //death
        if (health < 0)
        {
            Death();
            return;
        }

        //animation
        anim.SetFloat("Speed", Mathf.Abs(rig.velocity.x) > 0.1f ? 1 : 0);

        //patrol
        patrolTimer -= Time.deltaTime;
        if (patrolTimer < 0)
        {
            patrolTimer = Random.Range(2, 6);
            patrolDestination = patrolDestination != -1 ? -1 : 1;

        }

        var dir = (_gameManager.player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.right, dir);

        //facing
        isFacingRight = patrolDestination > 0;
        transform.localScale = new Vector2(isFacingRight ? 1 : -1, 1);

        //is in view
        if (enemyState == EnemyState.Attacking)
        {
            //shoot
            if ((angle < 45 && isFacingRight) || (angle > 165 && !isFacingRight))
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer < 0)
                {
                    attackTimer = attackRate;
                    GameObject bullet = Instantiate(bulletPrefab, weaponAim.transform.position + (weaponAim.transform.right * transform.localScale.x), weaponAim.transform.rotation * Quaternion.Euler(0, 0, 90));
                    Bullet bulletScript = bullet.GetComponent<Bullet>();
                    bulletScript.rg.AddForce(weaponAim.right * transform.localScale.x * bulletScript.speed, ForceMode2D.Impulse);
                }
            }
        }
        //range
        Vector2 dist = _gameManager.player.transform.position - transform.position;
        if (dist.magnitude < attackRange && _gameManager.player.isPlayerHidden == false)
        {
            enemyState = EnemyState.Attacking;
        }
        else
        {
            Vector2 initDist = initalPos - (Vector2)transform.position;
            if (initDist.magnitude > attackRange * 2)
            {
                enemyState = EnemyState.Patrol;
            }
            else
            {
                enemyState = EnemyState.Offensive;
            }
        }
    }


    private void FixedUpdate()
    {
        rig.velocity = new Vector2(patrolDestination * patrolSpeed, rig.velocity.y);
    }

    public void Death()
    {
        if (died == false)
        {
            died = true;
        }
        else
        {
            return;
        }
        if (deathParticle)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 0.25f);
    }
}
