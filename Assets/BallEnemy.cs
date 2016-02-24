using UnityEngine;
using System.Collections;

public class BallEnemy : MonoBehaviour
{
	private GameObject player;

	public bool isShooter = false;
	public GameObject ShotPrefab;

	private float fireDelay = 0f;

	private Enemy self;

	public void Kill()
	{
		rigidbody2D.gravityScale = 1f;
		GetComponent<ParticleSystem>().Stop();
		Destroy(this);
	}

	// Use this for initialization
	void Start ()
	{
		player = Player.PlayerObject;
		self = GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (player == null)
			player = Player.PlayerObject;

		if (self.EnemyType == "Dead")
		{
			Kill();
			return;
		}

		var d = Vector2.Distance(player.transform.position, transform.position);

		if (d < 10f)
		{
			rigidbody2D.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 100);

			var d1 = 10f - d;

			rigidbody2D.AddForce((player.transform.position - transform.position).normalized * d1);
		}

		if (isShooter)
		{
			fireDelay -= Time.deltaTime;

			if (fireDelay > 0f)
				return;

			if (d < 7f)
			{
				if (Random.Range(0, 10) < 2)
				{
					fireDelay = Random.Range(-1f, 4f);

					var pos = player.transform.position - transform.position;

					var go = Instantiate(ShotPrefab, transform.position + pos.normalized/2, Quaternion.identity) as GameObject;

					go.rigidbody2D.AddForce(pos.normalized * 500);
				}
				else
				{
					fireDelay += 1f;
				}
			}
		}
		


	}

	void OnCollisionEnter2D(Collision2D col)
	{
		foreach (var contact in col.contacts)
		{
			if (contact.collider.tag == "Player" || contact.otherCollider.tag == "Player")
			{
				player.rigidbody2D.AddForce(-contact.normal*3000);
				rigidbody2D.AddForce(contact.normal * 2000);
				player.GetComponent<Player>().PlayerHealth -= 60;
			}

			if (contact.collider.tag == "Wall" || contact.otherCollider.tag == "Wall")
			{
				rigidbody2D.AddForce(contact.normal*2000);
			}
		}

	}
}
