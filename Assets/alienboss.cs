using UnityEngine;
using System.Collections;

public class alienboss : MonoBehaviour
{
	private GameObject player;
	private Player playerObj;

	public GameObject EnemyPrefab1;
	public GameObject EnemyPrefab2;

	public EyeCenter EyeCenter;
	public EyeMover EyeMover;

	public GameObject Poof;
	public GameObject BigSplosion;

	private GameObject childrenBox;

	private float curangle = 0f;

	private bool isDead = false;

	IEnumerator Spawner()
	{
		yield return new WaitForSeconds(5f);

		while (true)
		{
			if (player == null)
			{
				yield return new WaitForSeconds(1f);
				continue;
			}

			if (isDead)
				yield break;

			var waittime = Random.Range(1f, 8f) + 5f*(playerObj.EnemyHealth/5000f);
			GameObject g;

			if (Random.Range(0, 3) == 0)
				g = Instantiate(EnemyPrefab2, new Vector3(Random.Range(35f, 45f), Random.Range(-126, -138), 0f), Quaternion.identity) as GameObject;
			else
				g = Instantiate(EnemyPrefab1, new Vector3(Random.Range(35f, 45f), Random.Range(-126, -138), 0f), Quaternion.identity) as GameObject;

			g.transform.parent = childrenBox.transform;

			Instantiate(Poof, g.transform.position, Quaternion.identity);

			yield return new WaitForSeconds(waittime);

			if (isDead)
				yield break;
		}
	}

	// Use this for initialization
	void Awake()
	{
		player = Player.PlayerObject;
		childrenBox = new GameObject("AlienBabies");
		childrenBox.transform.position = Vector3.zero;

		StartCoroutine(Spawner());
	}

	IEnumerator Die()
	{
		EyeCenter.Die();
		EyeMover.StartDie();
		isDead = true;

		Destroy(childrenBox);

		for (var i = 0; i < 20; i++)
		{
			Instantiate(BigSplosion,
				new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f), -2f),
				Quaternion.identity);
			yield return new WaitForSeconds(0.2f);
		}


		for (var i = 0; i < 40; i++)
		{
			Instantiate(BigSplosion,
				new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f), -2f),
				Quaternion.identity);

			yield return new WaitForSeconds(0.05f);
		}

		Camera.main.GetComponent<CameraScript>().PlayEnding();

		for (var i = 0; i < 40; i++)
		{
			Instantiate(BigSplosion,
				new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f), -2f),
				Quaternion.identity);

			yield return new WaitForSeconds(0.05f);
		}

		yield return new WaitForSeconds(1f);
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (isDead)
			return;


		if (player == null)
			player = Player.PlayerObject;

		if (playerObj == null)
			playerObj = player.GetComponent<Player>();

		if (playerObj.EnemyHealth <= 0)
		{
			StartCoroutine(Die());
			isDead = true;
			return;
		}




		var dir = player.transform.position - transform.position;
		var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

		curangle = Mathf.MoveTowardsAngle(curangle, angle, Time.deltaTime*10);

		//Debug.Log(curangle);
		

		transform.rotation = Quaternion.AngleAxis(-curangle, transform.forward);

		var speedbonus = (5000 - playerObj.EnemyHealth)/5000f;

		transform.position += transform.up * Time.deltaTime * (0.5f + speedbonus);
	}

	void OnDestroy()
	{
		//Debug.Log("DESTROY");
		Destroy(childrenBox);
	}
}
