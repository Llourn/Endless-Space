using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOne : PoolObject {

    public float speed;


    private void Start()
    {
    }

    void Update ()
    {
        this.transform.Translate(Vector2.up * speed);
	}

    public override void DisableComponents()
    {
        
    }

    public override void OnObjectReuse()
    {

    }


    


}
