using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public GameUI GameUIObject;
	public GameObject TorpedoPrefab;
	public GameObject ExplodePrefab;

	public static GameObject PlayerObject;

	public UILabel HealthBox;

	public CameraScript PrimaryCamera;

	public GameObject EnemiesPrefab;
	public GameObject CurrentEnemies;


	private const float speedforce = 5000f;

	public Vector2 RespawnLocation = new Vector2(-16, 0f);
	public float RespawnRotation = -90f;

	private float rotation;
	private Vector2 forwarddir = Vector2.up;

	public int PlayerHealth = 1000;
	private int LastHealth = 1000;

	public int EnemyStartHealth = 100;

	public int EnemyHealth = 100;
	public int LastEnemyHealth = 5100;

	private bool isDead = false;
	public bool IsAtBoss = false;

	private float refiretime = 0f;

	private IEnumerator Die()
	{
		isDead = true;

		var time = 1.5f;

		while (time > 0f)
		{
			time -= 0.1f;

			Instantiate(ExplodePrefab, gameObject.transform.position, Quaternion.identity);

			yield return new WaitForSeconds(0.1f);
		}

		var sr = GetComponent<SpriteRenderer>();

		sr.enabled = false;

		yield return new WaitForSeconds(0);

		GameUIObject.BlackOut(1f, 3f);

		yield return new WaitForSeconds(1f);

		PlayerHealth = 1000;
		sr.enabled = true;
		HealthBox.text = "Hull Integrity: " + (PlayerHealth/10) + "%";
		transform.position = RespawnLocation;
		transform.rotation = Quaternion.AngleAxis(RespawnRotation, Vector3.forward);
		Camera.main.gameObject.transform.position = RespawnLocation;

		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.angularVelocity = 0f;

		EnemyHealth = EnemyStartHealth;

		bool wasatboss = IsAtBoss;
		IsAtBoss = false;

		Destroy(CurrentEnemies);
		CurrentEnemies = Instantiate(EnemiesPrefab) as GameObject;

		yield return new WaitForSeconds(1f);

		IsAtBoss = wasatboss;
		isDead = false;
	}

	private IEnumerator Intro()
	{
		yield return new WaitForSeconds(20);

		while (GameManager.Instance.IsInIntro)
		{
			rigidbody2D.AddForce(transform.rotation*Vector3.up*speedforce*Time.deltaTime*1f);
			yield return new WaitForFixedUpdate();
		}

		HealthBox.gameObject.SetActive(true);
	}

	// Use this for initialization
	private void Start()
	{
		PlayerObject = gameObject;

		if (GameManager.Instance.UseIntro)
		{
			HealthBox.gameObject.SetActive(false);
			StartCoroutine(Intro());
		}
	}

	// Update is called once per frame
	private void Update()
	{
		if (GameManager.Instance.IsInIntro)
			return;

		if (isDead)
			return;

		refiretime -= Time.deltaTime;

		rigidbody2D.AddTorque(-Input.GetAxis("Horizontal")*Time.deltaTime);

		transform.Rotate(new Vector3(0, 0, -Input.GetAxis("Horizontal")*Time.deltaTime*150));

		var force = Input.GetAxis("Vertical");
		if (force < 0)
			force *= 0.5f;

		rigidbody2D.AddForce(transform.rotation*Vector3.up*speedforce*Time.deltaTime*force);

		if (Input.GetButton("Fire1") && refiretime < 0f)
		{
			refiretime = 0.3f;
			var o = Instantiate(TorpedoPrefab, transform.position, transform.rotation) as GameObject;
			o.layer = LayerMask.NameToLayer("PlayerProjectile");
		}

		if (EnemyHealth <= 0f)
		{
			HealthBox.text = "";
			return;
		}

		if (LastHealth != PlayerHealth || LastEnemyHealth != EnemyHealth)
		{
			var lost = LastHealth - PlayerHealth;
			//Debug.Log(lost);
			if (lost > 0)
			{
				PrimaryCamera.Shake(lost/100f, 1f);
			}

			LastHealth = PlayerHealth;
			if (PlayerHealth < 0)
			{
				PlayerHealth = 0;
				StartCoroutine(Die());
			}

			if (!IsAtBoss)
			{
				if (PlayerHealth > 200)
					HealthBox.text = "\nHull Integrity: " + (PlayerHealth/10) + "%";
				else
					HealthBox.text = "\n[ff0000]Hull Integrity: " + (PlayerHealth/10) + "%";
			}
			else
			{
				var txt = "";

				//if (EnemyHealth > 2000)
					txt = "[00ff00]Enemy Health: " + (EnemyHealth / 50) + "%[ffffff]";
				//else
					//txt = "Enemy Health: " + (EnemyHealth / 50) + "%[ffffff]";

				if (PlayerHealth > 200)
					txt += "\nHull Integrity: " + (PlayerHealth / 10) + "%";
				else
					txt += "\n[ff0000]Hull Integrity: " + (PlayerHealth / 10) + "%";

				HealthBox.text = txt;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		foreach (var contact in col.contacts)
		{
			if (isDead)
				return;

			if (contact.collider.tag == "Tentacle" || contact.otherCollider.tag == "Tentacle")
				PlayerHealth -= 5;

			if (contact.collider.tag == "EnemyActive")
			{
				var n = contact.collider.gameObject.GetComponent<Enemy>().EnemyType;

				if (n == "Boss")
					PlayerHealth -= 10;
			}

			if (contact.otherCollider.tag == "EnemyActive")
			{
				var n = contact.otherCollider.gameObject.GetComponent<Enemy>().EnemyType;

				if (n == "Boss")
					PlayerHealth -= 10;
			}
		}


	}

	private void OnCollisionStay2D(Collision2D col)
	{
		foreach (var contact in col.contacts)
		{
			if (isDead)
				return;

			if (contact.collider.tag == "Tentacle" || contact.otherCollider.tag == "Tentacle")
				PlayerHealth -= 5;

			if (contact.collider.tag == "EnemyActive")
			{
				var n = contact.collider.gameObject.GetComponent<Enemy>().EnemyType;

				if (n == "Boss")
					PlayerHealth -= 10;
			}

			if (contact.otherCollider.tag == "EnemyActive")
			{
				var n = contact.otherCollider.gameObject.GetComponent<Enemy>().EnemyType;

				if (n == "Boss")
					PlayerHealth -= 10;
			}
		}
	}
}