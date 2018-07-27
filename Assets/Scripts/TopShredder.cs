using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopShredder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * if (collision.gameObject.CompareTag("Player Projectile") ||
            collision.gameObject.CompareTag("Enemy Projectile"))
        {
            collision.gameObject.GetComponentInParent<PoolObject>().OnDestroy();
        }
        */
    }
}
