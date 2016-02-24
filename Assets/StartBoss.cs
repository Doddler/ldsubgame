using UnityEngine;
using System.Collections;

public class StartBoss : MonoBehaviour
{

	public GameObject Monsters1;
	public GameObject Monsters2;
	public GameObject Monsters3;

	public GameObject Zone3;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;

		Destroy(Monsters1);
		Destroy(Monsters2);
		Destroy(Monsters3);

		Zone3.SetActive(false);

		Destroy(this);
	}
}
