using UnityEngine;
using System.Collections;

public class ZoneTrigger : MonoBehaviour
{
	public enum TriggerType
	{
		Zone2Entrance,
		Zone3Entrance,
		ZoneFinal
	}

	public TriggerType ZoneTriggerType;
	public GameObject HideZone;
	public GameObject ShowZone;
	public GameObject EnemiesPrefab;
	public GameObject EnemiesObject;

	public GameObject DestroyGroup;

	public bool UpdateRespawn;

	public Vector2 RespawnPoint;
	public float RespawnRotation;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;

		if(ZoneTriggerType == TriggerType.ZoneFinal)
			Player.PlayerObject.GetComponent<Player>().IsAtBoss = true;

		var player = Player.PlayerObject.GetComponent<Player>();

		if(HideZone != null)
			HideZone.SetActive(false);

		if(ShowZone != null)
			ShowZone.SetActive(true);

		if (EnemiesPrefab != null)
			player.EnemiesPrefab = EnemiesPrefab;

		if (EnemiesObject != null)
		{
			player.CurrentEnemies = EnemiesObject;
			EnemiesObject.SetActive(true);
		}

		if(DestroyGroup != null)
			Destroy(DestroyGroup);

		if (UpdateRespawn)
		{
			player.RespawnLocation = RespawnPoint;
			player.RespawnRotation = RespawnRotation;
		}

		Destroy(this);
	}
}
