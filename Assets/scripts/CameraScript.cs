using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public GameObject Player;
	public GameObject Shadow;
	public GameObject IntroSkull;

	public GameObject IntroArea;

	public GameUI UI;

	private bool followPlayer = false;

	IEnumerator ShakeCamera(float power, float time)
	{
		yield return new WaitForFixedUpdate();

		while (time > 0f)
		{
			var move = new Vector3(Random.Range(-power, power), Random.Range(-power, power), 0f);

			gameObject.transform.position += move;

			yield return new WaitForFixedUpdate();

			time -= Time.deltaTime;
			power /= (1 + (1 / time * Time.deltaTime));
			gameObject.transform.position -= move;
		}
	}

	public void Shake(float power, float time)
	{
		StartCoroutine(ShakeCamera(power, time));
	}

	public void PlayEnding()
	{
		UI.PlayEnding();
	}

	IEnumerator IntroSequence()
	{
		yield return new WaitForSeconds(16);

		Shadow.SetActive(true);

		yield return new WaitForSeconds(4);

		iTween.MoveTo(gameObject, iTween.Hash("y", 0, "time", 5, "easetype", "easeOutCubic"));

		yield return new WaitForSeconds(5);

		followPlayer = true;

		yield return new WaitForSeconds(9f);

		iTween.RotateTo(IntroSkull, new Vector3(0f, 0f, 370f), 5f);
		StartCoroutine(ShakeCamera(1.5f, 1f));

		yield return new WaitForSeconds(1.2f);

		StartCoroutine(ShakeCamera(3f, 1f));
		
		GameManager.Instance.IsInIntro = false;
		iTween.ScaleTo(Shadow, new Vector3(4f, 4f, 1f), 10f);


		IntroArea.SetActive(false);
	}

	// Use this for initialization
	void Start()
	{
		if (GameManager.Instance.UseIntro)
		{
			gameObject.transform.position = GameManager.Instance.IntroStartPosition;
			iTween.MoveTo(gameObject, iTween.Hash("y", 100, "time", 16, "easetype", "easeInCubic"));
			StartCoroutine(IntroSequence());
		}
		else
		{
			Player.transform.position = new Vector3(40f, -125f, 0f);
			transform.position = new Vector3(40f, -125f, -10f);
			followPlayer = true;
			IntroArea.SetActive(false);
		}
	}

	void Update()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

		if (Player.GetComponent<Player>().IsAtBoss)
			camera.backgroundColor = Color.Lerp(camera.backgroundColor, Color.red, Time.deltaTime/50);
		else
			camera.backgroundColor = new Color(0.18359375f, 0.30078125f, 0.48828125f);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!followPlayer)
		{
			
		}
		else
		{
			var v3 = Vector3.Lerp(gameObject.transform.position, Player.transform.position, Time.deltaTime);
			v3.z = -10f;

			transform.position = v3;	
		}

		if(!GameManager.Instance.IsInIntro)
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 6f, Time.deltaTime);

	}
}
