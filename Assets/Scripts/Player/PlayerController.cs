using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerController : PoolObject {

    
    private int _weaponLevel = 1;
    public int weaponLevel {
        get { return _weaponLevel; }
        set {
            if (value > 5)
            {
                _weaponLevel = 5;
            }
            else
            {
                _weaponLevel = value;
            }
        }
    }

    public GameObject shield;
    public float shieldDuration = 3.0f;
    private float shieldTimer = 0.0f;
    private float xRotationSmoothing = 5.0f;
    private float currentFireRate = 0.3f;
    private float maxXRotation = 40.0f;
    private float screenConstraintOffset = 0.3f;
    private Vector2 moveLeft = new Vector2(-30.0f, 0.0f);
    private Vector2 moveRight = new Vector2(30.0f, 0.0f);


    [Header("Laser Source Locations")]
    public Transform centerLaser;
    [Space(5)]
    public Transform leftLaserOne;
    public Transform rightLaserOne;
    [Space(5)]
    public Transform leftLaserTwo;
    public Transform rightLaserTwo;

	private Rigidbody2D rbody;
    private BoxCollider2D col;
    private Vector3 worldSize;
    private float nextFire = 0.0f;
    private Animator animator;
    private bool playerReady = false;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        worldSize = GameManager.Instance.cameraControl.myCamera.ScreenToWorldPoint(new Vector3(Screen.width,
            Screen.height, 0.0f));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            weaponLevel++;
        }

        if(CrossPlatformInputManager.GetButton("MoveLeft") || Input.GetKey(KeyCode.A))
        {
            rbody.AddForce(moveLeft, ForceMode2D.Force);
        }
        if (CrossPlatformInputManager.GetButton("MoveRight") || Input.GetKey(KeyCode.D))
        {
            rbody.AddForce(moveRight, ForceMode2D.Force);
        }
        float xPos = Mathf.Clamp(this.transform.position.x, 
            -GameManager.Instance.playSessionManager.worldSize.x + screenConstraintOffset,
            GameManager.Instance.playSessionManager.worldSize.x - screenConstraintOffset);
        this.transform.position = new Vector2(xPos, this.transform.position.y);

        HandlePlayerRotation();
        if(playerReady) ActiveWeaponsSystem();
        ShieldSystem();
        animator.enabled = !playerReady;
    }
    private void OnEnable()
    {
        if(animator != null)
        {
            animator.enabled = true;
        }
    }

    void HandlePlayerRotation()
    {
        float xRotation = 0.0f;
        xRotation = maxXRotation * (rbody.velocity.x / xRotationSmoothing);
        xRotation = Mathf.Clamp(xRotation, -maxXRotation, maxXRotation);
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -xRotation, 0.0f));
    }

    void ActiveWeaponsSystem()
    {
        switch (_weaponLevel)
        {
            case 1:
                LevelOneLasers();
                break;

            case 2:
                LevelTwoLasers();
                break;

            case 3:
                LevelThreeLasers();
                break;

            case 4:
                LevelFourLasers();
                break;

            case 5:
                LevelFiveLasers();
                break;

            default:
                Debug.LogWarning("This is not a valid weapon level!!!");
                break;
        }
    }

    void ShieldSystem()
    {
        if(Time.time < shieldTimer && !shield.activeSelf)
        {
            shield.SetActive(true);
        }
        else if(shield.activeSelf && Time.time > shieldTimer)
        {
            GameManager.Instance.audioManager.Play("SFX Shield end");
            shield.SetActive(false);
        }
    }

    void LevelOneLasers()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + currentFireRate;

            GameManager.Instance.audioManager.Play("SFX Player laser");

            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                centerLaser.position,
                Quaternion.identity);
        }
    }

    void LevelTwoLasers()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + currentFireRate;

            GameManager.Instance.audioManager.Play("SFX Player laser");

            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                leftLaserOne.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                rightLaserOne.position,
                Quaternion.identity);
        }
    }

    void LevelThreeLasers()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + currentFireRate;

            GameManager.Instance.audioManager.Play("SFX Player laser");

            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                leftLaserOne.position,
                Quaternion.Euler(new Vector3(0.0f, 0.0f, 25f)));
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                centerLaser.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                rightLaserOne.position,
                Quaternion.Euler(new Vector3(0.0f, 0.0f, -25f)));
        }
    }

    void LevelFourLasers()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + currentFireRate;

            GameManager.Instance.audioManager.Play("SFX Player laser");

            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                leftLaserOne.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                leftLaserTwo.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                rightLaserOne.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                rightLaserTwo.position,
                Quaternion.identity);
        }
    }

    void LevelFiveLasers()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + currentFireRate;

            GameManager.Instance.audioManager.Play("SFX Player laser");

            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                leftLaserOne.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                leftLaserTwo.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                rightLaserOne.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                rightLaserTwo.position,
                Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerLaser,
                centerLaser.position,
                Quaternion.identity);
        }
    }

    public override void OnDestroy()
    {
        GameManager.Instance.audioManager.Play("SFX Player explosion");
        GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playerManager.playerExplosion,
                this.transform.position,
                Quaternion.identity);
        base.OnDestroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Enemy Projectile") ||
            collision.gameObject.CompareTag("Obstacle")) && !shield.activeSelf)
        {
            GameManager.Instance.playSessionManager.PlayerDead();
            OnDestroy();
        }

        if (collision.gameObject.CompareTag("Shield Pickup"))
        {
            if(shieldTimer < Time.time)
            {
                shieldTimer = Time.time + shieldDuration;
            }
            else
            {
                shieldTimer += shieldDuration;
            }
            GameManager.Instance.playSessionManager.UpdateScore(400);
            collision.GetComponentInParent<PoolObject>().OnDestroy();
            GameManager.Instance.audioManager.Play("SFX Pickup shield");
        }

        if (collision.gameObject.CompareTag("Weapon Pickup"))
        {
            weaponLevel++;
            GameManager.Instance.playSessionManager.UpdateScore(300);
            collision.GetComponentInParent<PoolObject>().OnDestroy();
            GameManager.Instance.audioManager.Play("SFX Pickup weapon");
        }
    }
    public override void OnObjectReuse()
    {
        base.OnObjectReuse();
        weaponLevel = 1;
        playerReady = false;
    }

    public void PlayerReady()
    {
        GameManager.Instance.enemyManager.InitiateEnemySpawn();
        GameManager.Instance.uiManager.ReadyBlink();
        GameManager.Instance.playSessionManager.StartTheLevel();
        playerReady = true;

    }

}
