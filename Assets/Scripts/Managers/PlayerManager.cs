using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject playerPrefab;
    public Transform playerSpawnLocation;
    public GameObject playerExplosion;

    [Header("Player Weapon Prefabs")]
    public GameObject playerLaser;

    Quaternion playerSpawnRotation = Quaternion.identity;

	void Start ()
    {
        GameManager.Instance.poolManager.CreatePool(playerPrefab, 2);
        GameManager.Instance.poolManager.CreatePool(playerLaser, 30);
        GameManager.Instance.poolManager.CreatePool(playerExplosion, 1);
	}

    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Respawn"))
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        GameManager.Instance.poolManager.ReuseObject(playerPrefab, playerSpawnLocation.position, playerSpawnRotation);
    }
}
