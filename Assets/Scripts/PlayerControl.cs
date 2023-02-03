using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    public float sitSpeed;
    public float jumpForce;

    public float dashSpeed;
    public float dashDuration;

    private float horizontal;
    private float dashTime;
    private bool isDashing;
    private bool isSitting;
    private float dashDirection;
    private bool isFacingRight = false;
    private Rigidbody2D playerRig;

    public Transform groundCheck;
    public LayerMask groundLayer;

    Vector2 lookDir;
    private float AngleDeg;
    public Transform weaponAim;

    public Weapon currentWeapon;

    public GameManager _GameManager;
    [SerializeField] private float closetRange = 1f;
    [SerializeField] private SpriteRenderer renderer;
    private bool isPlayerHidden;

    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        Hidden();
        //jump
        if (!isDashing && Input.GetButtonDown("Jump") && isGrounded())
        {
            playerRig.velocity += new Vector2(playerRig.velocity.x, jumpForce);
        }

        if (!isDashing && Input.GetButtonDown("Jump") && playerRig.velocity.y > 0f)
        {
            playerRig.velocity += new Vector2(playerRig.velocity.x, playerRig.velocity.y * 0.5f);
        }
        Flip();

        //sitting
        if (!isDashing && Input.GetKeyDown(KeyCode.S) && isGrounded())
        {
            isSitting = !isSitting;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isSitting)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashDirection = horizontal;
        }

        //aiming
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDir = mouseScreenPosition;
        float AngleRad = Mathf.Atan2(lookDir.y - transform.position.y, lookDir.x - transform.position.x);
        AngleDeg = (180 / Mathf.PI) * AngleRad;

        //lookDir = new Vector2(Input.GetAxis("LookX"), Input.GetAxis("LookY"));
        //Debug.Log(lookDir.ToString("0.0"));
        if (lookDir != Vector2.zero)
        {
            weaponAim.rotation = Quaternion.Euler(0, 0, AngleDeg);
            weaponAim.localScale = new Vector3(transform.localScale.x, 1, 1);
        }

        _GameManager.debugText.text = "Angle Deg: " + AngleDeg.ToString("0");
        _GameManager.debugText.text += "\nfacing right: " + isFacingRight;
        //pick
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Weapon droppedWp in _GameManager.droppedWeapons)
            {
                Vector2 dir = droppedWp.transform.position - transform.position;
                if (dir.magnitude < 1)
                {
                    if (currentWeapon == null)
                    {
                        Destroy(droppedWp.rg);
                        droppedWp.transform.SetParent(weaponAim);
                        droppedWp.transform.localPosition = Vector3.zero;
                        droppedWp.transform.localEulerAngles = Vector3.zero;
                        droppedWp.col.enabled = false;
                        currentWeapon = droppedWp;
                    }
                    else
                    {
                        currentWeapon.transform.SetParent(null);
                        currentWeapon.col.enabled = true;
                        Rigidbody2D wpRg = currentWeapon.gameObject.AddComponent<Rigidbody2D>();
                        wpRg.velocity = playerRig.velocity;
                        currentWeapon.rg = wpRg;
                        currentWeapon = null;
                    }
                    break;
                }
            }
        }

        //shoot
        if (currentWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }

    }
    void FixedUpdate()
    {
        if (isPlayerHidden)
        {
            isDashing = false;
            playerRig.velocity = Vector2.zero;
            return;
        }
        //Dash
        if (isDashing)
        {
            if (dashTime <= 0)
            {
                isDashing = false;
                playerRig.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                //playerRig.velocity +=  dashDirection;
                playerRig.velocity = new Vector2(dashDirection * dashSpeed, playerRig.velocity.y);
            }
        }
        //Move
        else if (!isSitting)
        {
            playerRig.velocity = new Vector2(horizontal * speed, playerRig.velocity.y);
        }
        else
        {
            playerRig.velocity = new Vector2(horizontal * sitSpeed, playerRig.velocity.y);
        }

    }

    private void Hidden()
    {
        if (!isDashing && Input.GetKeyDown(KeyCode.H) && isGrounded())
        {
            Collider2D closet = Physics2D.OverlapCircle(transform.position, closetRange, LayerMask.GetMask("Closet"));
            if (closet != null)
            {
                if (isPlayerHidden)
                {
                    gameObject.layer = LayerMask.NameToLayer("Player");
                    renderer.color = Color.white;
                }
                else
                {
                    transform.position = new Vector2(closet.transform.position.x, transform.position.y);
                    gameObject.layer = LayerMask.NameToLayer("Hidden");
                    renderer.color = Color.gray;
                }

                isPlayerHidden = !isPlayerHidden;
            }
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        if (currentWeapon)
        {
            currentWeapon.transform.localScale = new Vector3(1, Mathf.Abs(AngleDeg) > 90 ? -1 : 1, 1);
        }

        //aim based
        //isFacingRight = Mathf.Abs(AngleDeg) > 90;

        //Vector3 localScale = transform.localScale;
        //localScale.x = isFacingRight ? -1 : 1;
        //transform.localScale = localScale;
    }

    void Shoot()
    {
        if (currentWeapon.ammo > 0)
        {
            GameObject bullet = Instantiate(currentWeapon.bulletType, weaponAim.transform.position + weaponAim.transform.right, weaponAim.transform.rotation * Quaternion.Euler(0, 0, 90));
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.rg.AddForce(weaponAim.right * 10, ForceMode2D.Impulse);
            currentWeapon.ammo--;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.name.Contains("Weapon"))
    //    {

    //    }
    //}
}
