using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Projectile
{

    protected override void Update()
    {
        base.Update();

        if(this.transform.position.y < -GameManager.Instance.playSessionManager.worldSize.y - 1)
        {
            this.gameObject.SetActive(false);
        }
    }

}
