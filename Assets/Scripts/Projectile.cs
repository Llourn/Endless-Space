using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject {

    public float projectileSpeed;

	protected virtual void Update ()
    {
        this.transform.Translate(Vector2.up * projectileSpeed * Time.timeScale);
    }

}
