using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructParticleSystem : PoolObject {

    void OnEnable()
    {
        StartCoroutine(CheckIfAlive());
    }

    IEnumerator CheckIfAlive()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                this.gameObject.SetActive(false);
                break;
            }
        }
    }

}
