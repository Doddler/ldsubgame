using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{
	public UITexture Logo;
	public UITexture IntroText1;
	public UITexture Blackout;
	public UITexture OutroText1;

	public UITexture IntroText2;

	private float time = 0f;


	IEnumerator Ending()
	{
		Blackout.gameObject.SetActive(true);
		var ta = Blackout.gameObject.AddComponent<TweenAlpha>();
		ta.from = 0f;
		ta.to = 1f;
		ta.duration = 2f;

		ta.PlayForward();
		yield return new WaitForSeconds(2f);

		OutroText1.gameObject.SetActive(true);
		ta = OutroText1.gameObject.AddComponent<TweenAlpha>();
		ta.from = 0f;
		ta.to = 1f;
		ta.duration = 2f;

		ta.PlayForward();
	}

	public void PlayEnding()
	{
		StartCoroutine(Ending());
	}

	IEnumerator PlayIntro()
	{
		yield return new WaitForSeconds(8);

		Logo.gameObject.GetComponent<UITweener>().PlayReverse();

		yield return new WaitForSeconds(3f);

		Blackout.gameObject.SetActive(true);
		var ta = Blackout.gameObject.AddComponent<TweenAlpha>();
		ta.from = 0f;
		ta.to = 1f;
		ta.duration = 2f;

		ta.PlayForward();

		yield return new WaitForSeconds(3f);

		IntroText1.gameObject.SetActive(true);
		var ta2 = IntroText1.gameObject.AddComponent<TweenAlpha>();
		ta2.from = 0f;
		ta2.to = 1f;
		ta2.duration = 1f;

		ta2.PlayForward();


		yield return new WaitForSeconds(2f);

		ta2.PlayReverse();

		yield return new WaitForSeconds(2f);

		ta.PlayReverse();

		yield return new WaitForSeconds(2f);

		Destroy(IntroText1);
		Destroy(ta);

		yield return new WaitForSeconds(15f);

		IntroText2.gameObject.SetActive(true);

		var ta3 = IntroText2.GetComponent<UITweener>();
		ta3.PlayForward();

		yield return new WaitForSeconds(5f);

		ta3.PlayReverse();

		yield return new WaitForSeconds(2f);

		Destroy(IntroText2);

		//while (true)
		//{
		//	Debug.Log(time);
		//	yield return new WaitForSeconds(1f);
		//	time += 1f;
		//}
	}

	IEnumerator BlackOutForSeconds(float speed, float runtime)
	{
		Blackout.gameObject.SetActive(true);
		var ta = Blackout.gameObject.AddComponent<TweenAlpha>();
		ta.from = 0f;
		ta.to = 1f;
		ta.duration = speed;

		ta.PlayForward();

		yield return new WaitForSeconds(speed + runtime);

		ta.PlayReverse();

		yield return new WaitForSeconds(speed);

		Destroy(ta);
	}

	public void BlackOut(float speed, float runtime)
	{
		StartCoroutine(BlackOutForSeconds(speed, time));
	}

	// Use this for initialization
	void Start()
	{
		if (!GameManager.Instance.UseIntro)
		{
			Destroy(IntroText1.gameObject);
			Destroy(Logo.gameObject);
		}
		else
		{
			StartCoroutine(PlayIntro());
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
