using UnityEngine;
using System.Collections;

public class BallShot : MonoBehaviour
{
	private bool isDead = false;

	public ParticleSystem Trailsystem;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (isDead)
			return;

		isDead = true;

		var player = Player.PlayerObject;

		foreach (var contact in col.contacts)
		{
			if (contact.collider.tag == "Player" || contact.otherCollider.tag == "Player")
			{
				player.rigidbody2D.AddForce(-contact.normal*1000);
				player.GetComponent<Player>().PlayerHealth -= 40;
			}
		}

		Trailsystem.Stop();

		GetComponent<SpriteRenderer>().enabled = false;
		Destroy(rigidbody2D);
		Destroy(gameObject, 3f);
	}
}
