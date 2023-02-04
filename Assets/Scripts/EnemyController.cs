using UnityEngine;

public enum EnemyState { Patrol, Offensive, Attacking };
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float sight = 3;
    [SerializeField] private float sightAngle = 15f;
    [SerializeField] private Transform eyes;
    private Vector3 lastPos;
    private EnemyState enemyState;
    private Vector3 initPos;
    [SerializeField] private float patrolRange, partolSpeed, offensiveSpeed;
    private bool flipSide = false;
    private bool goingToinitPos;
    [SerializeField] private Rigidbody2D rig;
    private float activeSpeed;
    private Vector3 targetPosition;
    [SerializeField] private float attackRange = 1;
    private bool moving = true;
    private bool playerInSight;
    private Collider2D player;
    private bool isBackToInit;

    void Start()
    {
       lastPos = initPos = transform.position;
        enemyState = EnemyState.Patrol;
    }

    void FixedUpdate()
    {
        DetectingPlayer();
        if (enemyState == EnemyState.Patrol)
        {
            if (playerInSight)
            {
                enemyState = EnemyState.Offensive;
                return;
            }
            if (!isBackToInit && DistanceTo(initPos) >= patrolRange - 1)
            {
                isBackToInit = true;
                GoToPosition(initPos, partolSpeed);
            }
            else if (DistanceTo(initPos) < patrolRange)
            {
                isBackToInit = false;
                Patrol();
            }

            UpdatePosition();
        }
        else if (enemyState == EnemyState.Offensive)
        {
            if (playerInSight)
            {
                if (IsPlayerHidden())
                    enemyState = EnemyState.Patrol;
                else if (IsPlayerInAttackRange())
                {
                    Debug.Log("Set Moving False");
                    moving = false;
                    enemyState = EnemyState.Attacking;
                }
                else
                {
                    GoToPosition(player.transform.position, offensiveSpeed);
                    UpdatePosition();
                }
            }
            else
                enemyState = EnemyState.Patrol;
        }
        else if (enemyState == EnemyState.Attacking)
        {
            if (IsPlayerHidden())
            {
                Debug.Log("Set Moving TRUE");
                moving = true;
                enemyState = EnemyState.Patrol;
            }
            else if (!IsPlayerInAttackRange())
            {
                Debug.Log("Set Moving TRUE");
                moving = true;
                enemyState = EnemyState.Offensive;
            }
            else
            {
                Debug.Log("ATTACKING");
            }
        }
    }

    private void UpdatePosition()
    {
        if (moving)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, transform.position.y), activeSpeed * Time.deltaTime);
    }

    private void GoToPosition(Vector3 targetPos, float speed)
    {
        targetPosition = targetPos;
        activeSpeed = speed;
    }

    private void Patrol()
    {
        Vector3 destination = initPos + Vector3.right * patrolRange * (flipSide ? -1 : 1);
        if (DistanceTo(destination) < 0.5f)
            flipSide = !flipSide;
        GoToPosition(destination, partolSpeed);
    }

    private bool IsPlayerHidden()
    {
        if (player == null || player.gameObject.layer == LayerMask.NameToLayer("Hidden"))
            return true;
        else
            return false;
    }

    private bool IsPlayerInAttackRange()
    {
        if (player.Distance(GetComponent<Collider2D>()).distance <= attackRange)
        {
            return true;
        }
        else
        {
            Debug.Log(player.Distance(GetComponent<Collider2D>()).distance);
            return false;
        }
    }

    private float DistanceTo(Vector3 target)
    {
        return Vector3.Magnitude(transform.position - target);
    }

    private void DetectingPlayer()
    {
        player = Physics2D.OverlapCircle(eyes.position, sight, LayerMask.GetMask("Player"));
        if(player != null && playerInSight)
        {       
            if((transform.localScale.x > 0 && transform.position.x > player.transform.position.x) || (transform.localScale.x < 0 && transform.position.x < player.transform.position.x))
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        if ((transform.localScale.x > 0 && lastPos.x > transform.position.x) || (transform.localScale.x < 0 && lastPos.x < transform.position.x)) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        lastPos = transform.position;
        if (player != null)
        {
            var dir = (player.transform.position - eyes.position).normalized;
            float angle = Vector3.Angle(eyes.right, dir);
            if (transform.localScale.x == -1)
            {
                angle = 180 - angle;
            }
            if (playerInSight || angle < sightAngle)
            {
                playerInSight = true;
                if (enemyState == EnemyState.Attacking) return;
                //Debug.Log("player in enemy sight range");
                enemyState = EnemyState.Offensive;
            }
            else
            {
                player = null;
                playerInSight = false;
            }
        }
        else
            playerInSight = false;
    }
}
