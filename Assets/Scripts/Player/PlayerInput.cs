using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	PlayerController playerController;

	void Start()
	{
		playerController = GetComponent<PlayerController>();
		
	}

}
