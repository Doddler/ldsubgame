using UnityEngine;
using System.Collections;

public class HealWrench : MonoBehaviour
{
	public GameObject ExplosionPrefab;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			var p = Player.PlayerObject.GetComponent<Player>();

			if (p.PlayerHealth >= 1000)
				return;

			p.PlayerHealth = 1000;

			Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
