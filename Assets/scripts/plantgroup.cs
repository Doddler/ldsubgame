using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class plantgroup : MonoBehaviour
{
	public List<GameObject> Plants;

	private bool flip = false;

	IEnumerator MovePlants()
	{
		while (true)
		{
			var t = Random.Range(2f, 5f);

			var rot = Random.Range(0, 20f);

			if (flip)
				rot *= -1;

			flip = !flip;

			foreach (var plant in Plants)
			{

				iTween.RotateTo(plant, iTween.Hash("z", rot, "time", t, "easetype", "easeInOutSine"));
			}
			

			yield return new WaitForSeconds(t);
		}
	}

	// Use this for initialization
	void Start()
	{
		StartCoroutine(MovePlants());
	}

	// Update is called once per frame
	void Update()
	{

	}
}
