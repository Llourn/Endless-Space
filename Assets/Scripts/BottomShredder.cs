using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomShredder : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Enemy Projectile") ||
            collision.gameObject.CompareTag("Obstacle"))
        {
            collision.transform.parent.gameObject.SetActive(false);
        }
    }
}
