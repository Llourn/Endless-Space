using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || 
            collision.gameObject.CompareTag("Enemy Projectile") ||
            collision.gameObject.CompareTag("Obstacle"))
        {
            collision.GetComponentInParent<PoolObject>().OnDestroy();
        }
    }

}
