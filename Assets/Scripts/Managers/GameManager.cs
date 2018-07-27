using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager _instance;
	public static GameManager Instance { get {return _instance; }}

	public AudioManager audioManager;
	public LevelManager levelManager;
	public PlaySessionManager playSessionManager;
	public UIManager uiManager;
	public PlayerManager playerManager;
	public EventSystem eventSystem;
    public Camera mainCamera;
    public PoolManager poolManager;
    public EnemyManager enemyManager;
    public CameraControl cameraControl;
	
	void Awake()
	{
		if(_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}

		audioManager = FindObjectOfType<AudioManager>();
		levelManager = FindObjectOfType<LevelManager>();
		playSessionManager = FindObjectOfType<PlaySessionManager>();
		uiManager = FindObjectOfType<UIManager>();
		playerManager = FindObjectOfType<PlayerManager>();
		eventSystem = FindObjectOfType<EventSystem>();
        mainCamera = FindObjectOfType<Camera>();
        poolManager = FindObjectOfType<PoolManager>();
        enemyManager = FindObjectOfType<EnemyManager>();
        cameraControl = FindObjectOfType<CameraControl>();
	}

}
