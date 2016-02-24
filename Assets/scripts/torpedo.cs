using System.Runtime.Serialization.Formatters;
using UnityEngine;
using System.Collections;

public class torpedo : MonoBehaviour
{
	public ParticleSystem Emitter;
	public GameObject ExplosionPrefab;

	private GameObject player;
	private bool isChanged = false;
	private bool isDead = false;

	private float time = 10f;

	// Use this for initialization
	void Start()
	{
		player = Player.PlayerObject;

		if (player != null)
		{
			rigidbody2D.velocity = player.rigidbody2D.velocity + (Vector2)player.transform.up * 2f;
			rigidbody2D.angularVelocity = player.rigidbody2D.angularVelocity;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (isDead)
			return;

		rigidbody2D.AddForce(transform.rotation * Vector3.up * 80 * Time.deltaTime);

		if (player == null)
			player = Player.PlayerObject;

		if (!isChanged)
		{
			var d = Vector2.Distance(gameObject.transform.position, player.transform.position);
			if (d > 1f)
			{
				gameObject.layer = LayerMask.NameToLayer("Projectile");
				isChanged = true;
				//Debug.Log("Switched!");
			}
		}

		time -= Time.deltaTime;

		if (time <= 0f)
			StartCoroutine(DestroyTorpedo());
	}

	public void BlowUp()
	{
		StartCoroutine(DestroyTorpedo());
	}

	IEnumerator DestroyTorpedo()
	{
		isDead = true;

		if (Vector2.Distance(Player.PlayerObject.transform.position, transform.position) < 0.7f)
		{
			Player.PlayerObject.GetComponent<Player>().PlayerHealth -= 30;

			player.rigidbody2D.AddForceAtPosition((player.transform.position - transform.position).normalized * 5000, transform.position);
		}

		foreach (var o in GameObject.FindGameObjectsWithTag("Projectile"))
		{
			if (o == gameObject)
				continue;

			if (Vector2.Distance(o.transform.position, transform.position) < 0.7f)
			{
				var t = o.GetComponent<torpedo>();
				if (t.isDead)
					continue;

				t.BlowUp();
			}
		}

		foreach (var o in GameObject.FindGameObjectsWithTag("EnemyActive"))
		{
			if (o == gameObject)
				continue;

			var e = o.GetComponent<Enemy>();
			var t = e.EnemyType;

			var distance = Vector2.Distance(o.transform.position, transform.position);

			if (t == "Boss" && distance < 3f)
			{
				Debug.Log(distance);
				player.GetComponent<Player>().EnemyHealth -= 40;
				continue;
			}

			if (distance > 0.7f)
				continue;
			
			switch (t)
			{
				case "Landmine":
					var l = o.GetComponent<landmine>();
					l.Kill();
					break;
				case "Ball":
					var b = o.GetComponent<BallEnemy>();
					b.Kill();
					b.rigidbody2D.AddForceAtPosition((b.transform.position - transform.position).normalized * 5000, transform.position);
					e.EnemyType = "Dead";
					break;

				case "Dead":
					e.rigidbody2D.AddForceAtPosition((e.transform.position - transform.position).normalized * 5000, transform.position);
					break;
			}
		}

		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		Emitter.Stop();

		GameObject.Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);

		yield return new WaitForSeconds(3f);

		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		foreach(var contact in col.contacts)
		{
			if (isDead)
				return;

			var angle = Vector2.Angle(contact.normal, transform.up);

			if (angle > 120 && angle < 240)
			{
				StartCoroutine(DestroyTorpedo());
				isDead = true;
			}
			if (angle < 60 || angle > 300)
			{
				StartCoroutine(DestroyTorpedo());
				isDead = true;
			}
			//Debug.Log(angle);
			//Debug.DrawRay(contact.point, contact.normal, Color.green, 2, false);
		}
		//Destroy(gameObject);
	}
}
