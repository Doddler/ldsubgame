using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public bool IsInIntro = true;
	public bool UseIntro = true;

	public Vector3 IntroStartPosition = new Vector3(-50f, 1000f, 0f);


	private static GameManager _Instance;
	public static GameManager Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = GameObject.Find("GameManager").GetComponent<GameManager>();

			return _Instance;
		}
	}

	// Use this for initialization
	void Start()
	{
		if (!UseIntro)
			IsInIntro = false;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
