using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_2 : Enemy {

    public float rateOfFire = 1.0f;
    public Transform projectileOneSpawnPostion;
    public Transform projectileTwoSpawnPostion;

    private bool inRange = false;

    private float firingTimer = 0.0f;
    private float rateOfFireVariant = 0.0f;

    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        base.Update();

        if (this.transform.position.y < GameManager.Instance.playSessionManager.worldSize.y && !inRange)
        {
            EnemyInRange();
        }

        if (inRange) FireProjectiles();
    }

    private void EnemyInRange()
    {
        inRange = true;
        firingTimer = Time.time + Mathf.Abs(rateOfFireVariant);
    }

    void FireProjectiles()
    {
        if (Time.time > firingTimer)
        {
            GameManager.Instance.audioManager.Play("SFX Enemy laser");
            rateOfFire = rateOfFire - GameManager.Instance.playSessionManager.difficultyOffset + rateOfFireVariant;
            if (rateOfFire < 0.5f) rateOfFire = 0.5f;
            firingTimer = Time.time + rateOfFire;
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.enemyManager.enemyProjectiles[0], projectileOneSpawnPostion.position, Quaternion.identity);
            GameManager.Instance.poolManager.ReuseObject(GameManager.Instance.enemyManager.enemyProjectiles[0], projectileTwoSpawnPostion.position, Quaternion.identity);
        }
    }

    public override void OnObjectReuse()
    {
        base.OnObjectReuse();
        inRange = false;
        rateOfFireVariant = Random.Range(-0.5f, 0.5f);
    }
}
