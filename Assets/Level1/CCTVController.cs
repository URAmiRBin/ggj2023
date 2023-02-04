using UnityEngine;

public class CCTVController : MonoBehaviour
{
    [SerializeField] private Transform lightPath;
    //private EnemyState state = EnemyState.Patrol;
    private bool playerInSight;
    private Collider2D player;
    [SerializeField] private float rotateSpeed;
    private Vector3 targetPosition;
    private Quaternion initRotatation;
    private bool flipSide = true;
    [SerializeField] private GameManager gameManager;
    private bool announcePlayerInSight;
    private float sight = 6.5f;
    [SerializeField] private float sightAngle = 15;
    private float currentDiff;

    // Start is called before the first frame update
    void Start()
    {
        initRotatation = transform.rotation;
        announcePlayerInSight = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!announcePlayerInSight)
            DetectingPlayer();

        Patrol();
        if (!announcePlayerInSight && playerInSight)
        {
            gameManager.AnnouncePlayerInSight();
            announcePlayerInSight = true;
        }
    }

    private float DistanceTo(Vector3 target)
    {
        float diff = Mathf.Abs(lightPath.localRotation.eulerAngles.z - target.z);
        //Debug.Log("Deg : " + diff);
        return diff;
    }
    private void GoToPosition(Vector3 targetPos, float speed)
    {
        targetPosition = targetPos;
        rotateSpeed = speed;
    }

    private void Patrol()
    {
        Vector3 destination = initRotatation.eulerAngles + (Vector3.forward * sightAngle * (flipSide ? -1 : 1));
        Vector3 tmpDest = destination; ;
        if (destination.z < 0) tmpDest.z = 360 + destination.z;
        currentDiff = DistanceTo(tmpDest);

        //if (flipSide && (int)currentDiff > 180)
        //{
        //    currentDiff -= 360;
        //    Debug.Log("Current Diff : " + currentDiff);
        //    if ((int)currentDiff <= 0)
        //    {
        //        flipSide = false;
        //        return;
        //    }
        //}
        //else if(!flipSide)
        //{
        Debug.Log("Current Diff : " + currentDiff);
        if ((int)currentDiff == 0)
        {

            flipSide = !flipSide;
            return;
        }
        //}

        GoToPosition(destination, rotateSpeed);
        UpdateRotation();
    }
    private bool IsPlayerHidden()
    {
        if (player == null || player.gameObject.layer == LayerMask.NameToLayer("Hidden"))
            return true;
        else
            return false;
    }

    private void DetectingPlayer()
    {
        player = Physics2D.OverlapCircle(transform.position, sight, LayerMask.GetMask("Player"));
        if (player != null && playerInSight)
        {
            if ((transform.localScale.x > 0 && transform.position.x > player.transform.position.x) || (transform.localScale.x < 0 && transform.position.x < player.transform.position.x))
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        if (player != null)
        {
            float angle = GetAngleWithPosition(player.transform.position);
            if (transform.localScale.x == -1)
            {
                angle = 180 - angle;
            }
            if (!IsPlayerHidden() && angle < sightAngle)
            {
                playerInSight = true;
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

    private float GetAngleWithPosition(Vector3 target)
    {
        target.x -= lightPath.position.x;
        target.y -= lightPath.position.y;

        return Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
    }

    private void UpdateRotation()
    {
        //float angle = GetAngleWithPosition(targetPosition);
        // if (moving)
        Vector3 from = lightPath.rotation.eulerAngles;
        // lightPath.rotation = Quaternion.RotateTowards(lightPath.rotation, Quaternion.Euler(0, 0, angle), rotateSpeed * Time.deltaTime);
        if (lightPath.rotation.eulerAngles.z > 180) from.z = 360 - lightPath.rotation.eulerAngles.z;
        lightPath.rotation = Quaternion.RotateTowards(Quaternion.Euler(from), Quaternion.Euler(targetPosition), rotateSpeed * Time.deltaTime);
    }
}
