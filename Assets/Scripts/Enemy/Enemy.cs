using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameStates;

public class Enemy : PoolObject
{   [Range(0.0f, 1.0f)]
    public float pickupSpawnRate = 0.1f;
    public int pointValue = 100;
    private float verticalSpeed = 0.01f;
    private float frequency = 3.0f;
    private float magnitude = 0.3f;

    private bool offsetSpawn = false;

    private bool sleeper = false;
    private float timeToWake = 0.0f;
    private float transitionSpeed = 0.0f;
    private float wokeVerticalSpeed = 0.0f;
    private float wokeMagnitude = 0.0f;
    private float wokeFrequency = 0.0f;

    private bool clamp = false;
    private float minClamp = 0.0f;
    private float maxClamp = 0.0f;

    private float xMovement = 0.0f;
    private Vector3 originalPosition;
    private Vector3 newPosition;
    private float startTime = 0.0f;

    private float maxXRotation = 35.0f;
    private float cosineValue = 0.0f;
    private float sleepInterpolator = 0.0f;
    private float originalVerticalSpeed = 0.0f;
    private float originalMagnitude = 0.0f;
    private float originalFrequency = 0.0f;


    protected virtual void Start ()
    {
        newPosition = this.transform.position;
        startTime = Time.time;
    }
	
	protected virtual void Update ()
    {
        if (!offsetSpawn) startTime = 0.0f;
        cosineValue = Mathf.Cos((Time.time - startTime) * frequency);

        xMovement = (cosineValue * magnitude);
        newPosition.y -= verticalSpeed * Time.deltaTime;
        newPosition.x += (originalPosition.x + xMovement) * Time.deltaTime;
        if(clamp) newPosition.x = Mathf.Clamp(newPosition.x, originalPosition.x + minClamp, originalPosition.x + maxClamp);
        this.transform.position = newPosition;

        if(Time.time - startTime > timeToWake && sleeper)
        {
            SleeperUpdate();
        }

        HandleEnemyRoll();

        AutoCleanup();
    }

    private void SleeperUpdate()
    {
        if (wokeVerticalSpeed != verticalSpeed) verticalSpeed = Mathf.Lerp(originalVerticalSpeed, wokeVerticalSpeed, sleepInterpolator);
        if (wokeMagnitude != magnitude) magnitude = Mathf.Lerp(originalMagnitude, wokeMagnitude, sleepInterpolator);
        if (wokeFrequency != frequency) frequency = Mathf.Lerp(originalFrequency, wokeFrequency, sleepInterpolator);
        sleepInterpolator += transitionSpeed * Time.deltaTime;
    }

    private void AutoCleanup()
    {
        if(this.transform.position.y < -GameManager.Instance.playSessionManager.worldSize.y - 1.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public override void OnObjectReuse(FlightPattern fp, int index)
    {
        sleeper = false;
        newPosition = this.transform.position;
        startTime = Time.time;

        cosineValue = 0.0f;
        sleepInterpolator = 0.0f;
        offsetSpawn = fp.offsetSpawn;
        verticalSpeed = fp.verticalSpeed;

        originalMagnitude = magnitude = (index % 2 != 0 && fp.alternatingMagnitude) ? fp.alternateMagnitude : fp.magnitude;
        originalFrequency = frequency = (index % 2 != 0 && fp.alternatingFrequency) ? fp.alternateFrequency : fp.frequency;
        
        if(fp.sleeper)
        {
            sleeper = fp.sleeper;
            timeToWake = fp.timeToWake;
            transitionSpeed = fp.transitionSpeed;
            wokeVerticalSpeed = fp.wokeVerticalSpeed;
            wokeFrequency = (index % 2 != 0 && fp.wokeAlternatingFrequency) ? fp.wokeAlternateFrequency : fp.wokeFrequency;
            wokeMagnitude = (index % 2 != 0 && fp.wokeAlternatingMagnitude) ? fp.wokeAlternateMagnitude : fp.wokeMagnitude;
            originalVerticalSpeed = verticalSpeed;
        }
        if(fp.clamp)
        {
            clamp = fp.clamp;
            minClamp = fp.minClampValue;
            maxClamp = fp.maxClampValue;
        }

    }

    void HandleEnemyRoll()
    {
        float xRotation = 0.0f;
        xRotation = maxXRotation * cosineValue * Mathf.Sign(magnitude);
        if (magnitude == 0) xRotation *= magnitude;
        xRotation = Mathf.Clamp(xRotation, -maxXRotation, maxXRotation);
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -xRotation, 0.0f));
    }

    public override void OnDestroy()
    {
        GameManager.Instance.playSessionManager.UpdateScore(pointValue);
        GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.enemyManager.enemyEffects[0],
                this.transform.position,
                Quaternion.identity);
        if (!GameManager.Instance.playSessionManager.playerDead)
        {
            SpawnPickup();
            GameManager.Instance.audioManager.Play("SFX Enemy explosion");
            GameManager.Instance.playSessionManager.killCount++;
        }
            
        base.OnDestroy();
    }

    private void SpawnPickup()
    {
        if(Random.Range(0.0f, 1.0f) < pickupSpawnRate)
        {
            if(Random.Range(0.0f, 1.0f) < GameManager.Instance.playSessionManager.shieldPickupSpawnrate)
            {
                GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playSessionManager.shieldPickup,
                this.transform.position,
                Quaternion.identity);
            }
            else
            {
                GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.playSessionManager.weaponPickup,
                this.transform.position,
                Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player Projectile"))
        {
            OnDestroy();
        }
    }



}
