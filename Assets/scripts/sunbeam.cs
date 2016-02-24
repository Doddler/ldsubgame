using UnityEngine;
using System.Collections;

public class sunbeam : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;

	private Vector3 startPosition;

	IEnumerator MoveLightshaft()
	{
		spriteRenderer.enabled = true;

		//var r = Random.Range(0, 15f);
		//yield return new WaitForSeconds(r);

		while (true)
		{
			var move = new Vector3(Random.Range(-4f, 4f), 0f, 0f);

			var t = Random.Range(10f, 20f);

			iTween.MoveTo(gameObject, iTween.Hash("position", startPosition + move, "time", t, "easetype", "easeInOutSine"));
			yield return new WaitForSeconds(t + 1f);
		}
	}


	// Use this for initialization
	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		startPosition = gameObject.transform.position;

		spriteRenderer.enabled = false;

		StartCoroutine(MoveLightshaft());
	}

	// Update is called once per frame
	void Update()
	{

	}
}
