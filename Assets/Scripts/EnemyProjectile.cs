using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile {

    protected override void Update()
    {
        base.Update();

        if (this.transform.position.y < -GameManager.Instance.playSessionManager.worldSize.y)
            this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnDestroy();
        }
    }

}
