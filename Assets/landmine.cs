using UnityEngine;
using System.Collections;

public class landmine : MonoBehaviour
{
	public GameObject Explosion;

	private bool isDead = false;

	private Vector2 startPosition;

	IEnumerator Hover()
	{
		while (true)
		{
			var v = new Vector2(startPosition.x + Random.Range(-3f, 3f), startPosition.y + Random.Range(-3, 3f));
			var time = Random.Range(3f, 6f);

			iTween.MoveTo(gameObject, iTween.Hash("x", v.x, "y", v.y, "time", time, "easetype", "easeinoutsine"));

			yield return new WaitForSeconds(time);
		}
	}


	// Use this for initialization
	void Start()
	{
		startPosition = transform.position;

		StartCoroutine(Hover());
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Kill()
	{
		//StartCoroutine(Explode());
		Explode();
	}

	private void Explode()
	{
		if (isDead)
			return;

		isDead = true;

		var pDistance = Vector2.Distance(Player.PlayerObject.transform.position, transform.position);
		var player = Player.PlayerObject;
		
		if (pDistance < 6f)
		{
			var prox = 6 - pDistance;

			Debug.Log(prox);

			Camera.main.gameObject.GetComponent<CameraScript>().Shake(1f, 0.6f);
			player.GetComponent<Player>().PlayerHealth -= (int)(prox * prox * prox * 4);

			player.rigidbody2D.AddForceAtPosition((player.transform.position - transform.position).normalized * (prox * prox * 1000), transform.position);
		}

		foreach (var o in GameObject.FindGameObjectsWithTag("Projectile"))
		{
			if (o == gameObject)
				continue;

			if (Vector2.Distance(o.transform.position, transform.position) < 4f)
			{
				var t = o.GetComponent<torpedo>();

				t.BlowUp();
			}
		}


		foreach (var o in GameObject.FindGameObjectsWithTag("EnemyActive"))
		{
			if (o == gameObject)
				continue;

			var dist = Vector2.Distance(o.transform.position, transform.position);

			if (dist > 6f)
				continue;

			var e = o.GetComponent<Enemy>();
			var t = e.EnemyType;

			Debug.Log(t);

			switch (t)
			{
				case "Landmine":
					var l = o.GetComponent<landmine>();
					if(dist < 4f)
						l.Kill();
					break;
				case "Ball":
					var b = o.GetComponent<BallEnemy>();
					if(dist < 3f)
						b.Kill();
					b.rigidbody2D.AddForceAtPosition((b.transform.position - transform.position).normalized * 5000, transform.position);
					e.EnemyType = "Dead";
					break;
				case "Dead":
					e.rigidbody2D.AddForceAtPosition((e.transform.position - transform.position).normalized * 5000, transform.position);
					break;
			}
		}

		Instantiate(Explosion, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		foreach (var contact in col.contacts)
		{
			if (contact.collider.tag == "Projectile")
			{
				//var angle = Vector2.Angle(contact.normal, transform.up);

				//if (angle > 120 && angle < 240)
				//	Explode();
				//if (angle < 60 || angle > 300)
				//	Explode();
			}
			else
			{
				Explode();
				return;
			}

		}

	}
}
