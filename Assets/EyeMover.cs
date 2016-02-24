using UnityEngine;
using System.Collections;

public class EyeMover : MonoBehaviour
{
	private bool isDead;

	public void StartDie()
	{
		isDead = true;
		StartCoroutine(Die());
	}

	IEnumerator Die()
	{
		
		while (true)
		{
			transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), transform.forward);
			yield return new WaitForSeconds(0.2f);
		}
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (isDead)
			return;

		var player = Player.PlayerObject;

		var dir = player.transform.position - transform.position;

		var angle = Mathf.Atan2(dir.x, dir.y)*Mathf.Rad2Deg;

		//Debug.Log(angle);

		transform.rotation = Quaternion.AngleAxis(-angle, transform.forward);
	}
}
