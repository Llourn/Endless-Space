using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : PoolObject
{


    private float rotationSpeed;
    private float moveSpeed;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private float zRotation = 0.0f;

    private void Start()
    {
        rotationSpeed = Random.Range(8.0f, 15.0f);
        moveSpeed = Random.Range(0.5f, 1.5f);
        xRotation = Random.Range(0.1f, 1.0f);
        yRotation = Random.Range(0.1f, 1.0f);
        zRotation = Random.Range(0.1f, 1.0f);
    }

    private void Update()
    {
        this.transform.Rotate(xRotation * rotationSpeed * Time.deltaTime,
            yRotation * rotationSpeed * Time.deltaTime,
            zRotation * rotationSpeed * Time.deltaTime);

        this.transform.Translate(-Vector2.up * moveSpeed * Time.deltaTime, Space.World);
        
    }

    public override void OnObjectReuse()
    {
        base.OnObjectReuse();

        rotationSpeed = Random.Range(8.0f, 15.0f);
        moveSpeed = Random.Range(0.5f, 1.5f);
        xRotation = Random.Range(0.1f, 1.0f);
        yRotation = Random.Range(0.1f, 1.0f);
        zRotation = Random.Range(0.1f, 1.0f);

    }


}
