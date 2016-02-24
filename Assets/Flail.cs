using UnityEngine;
using System.Collections;

public class Flail : MonoBehaviour
{
	IEnumerator FlailRandomly(float min, float max)
	{
		var baserotation = transform.localRotation.eulerAngles.z;
		//Debug.Log(baserotation);

		while (true)
		{
			var t = Random.Range(min, max);
			//Debug.Log(t);
			iTween.RotateTo(gameObject, iTween.Hash("z", Random.Range(baserotation -60f, baserotation+60f), "time", t, "islocal", true, "easetype", "easeinoutsine"));
			yield return new WaitForSeconds(t);
		}
	}

	// Use this for initialization
	void Start()
	{
		StartCoroutine(FlailRandomly(0.5f, 1f));
	}

	// Update is called once per frame
	void Update()
	{

	}
}
