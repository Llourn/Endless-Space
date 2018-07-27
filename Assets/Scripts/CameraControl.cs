using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Camera myCamera;

    private void Awake()
    {
        myCamera = GetComponent<Camera>();
    }


}
