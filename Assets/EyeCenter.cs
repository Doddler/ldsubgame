using UnityEngine;
using System.Collections;

public class EyeCenter : MonoBehaviour
{
	private Vector2 startPosition;

	public void Die()
	{
		StartCoroutine(MoveRandomly());
		StartCoroutine(MoveRandomly());
	}

	IEnumerator MoveRandomly()
	{
		while (true)
		{
			var v = new Vector2(startPosition.x + Random.Range(-0.2f, 0.2f), startPosition.y + Random.Range(-0.2f, 0.2f));
			var time = Random.Range(0.2f, 0.2f);

			iTween.MoveTo(gameObject, iTween.Hash("x", v.x, "y", v.y, "time", time, "islocal", true, "easetype", "easeinoutsine"));

			yield return new WaitForSeconds(time);

			yield return new WaitForSeconds(Random.Range(0f, 0.5f));
		}
	}

	// Use this for initialization
	void Start()
	{
		startPosition = gameObject.transform.localPosition;

		StartCoroutine(MoveRandomly());
	}

	// Update is called once per frame
	void Update()
	{

	}
}
