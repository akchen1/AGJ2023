using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodCatchMinigame : MonoBehaviour, IMinigame
{
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	[SerializeField] private List<InventoryItem> neededItems;
	[SerializeField] private int bloodNeeded;

	[SerializeField, Header("UI")] private GameObject panel;
	[SerializeField] private Slider bloodSlider;

	[SerializeField, Header("Vial")] private GameObject vial;

	[SerializeField, Header("Blood")] private GameObject bloodPrefab;
	[SerializeField] private Vector2 minBloodSpawnPos;
	[SerializeField] private Vector2 maxBloodSpawnPos;
	[SerializeField] private float bloodSpawnTimer;
	[SerializeField] private float bloodSpawnTimerOffset;
	[SerializeField] private float bloodFallSpeed;
	[SerializeField] private Transform bloodHolder;

	private bool gameRunning = false;
	private int totalBlood;

	private List<GameObject> bloodList = new List<GameObject>();

	private float timer;

	public void Finish()
	{
		foreach(GameObject blood in bloodList)
		{
			Destroy(blood);
		}

		gameRunning = false;
		panel.SetActive(false);
		eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
	}

	public void Initialize()
	{
		panel.SetActive(true);
		timer = Random.Range(bloodSpawnTimer - bloodSpawnTimerOffset, bloodSpawnTimer + bloodSpawnTimerOffset);

		totalBlood = 0;
		bloodSlider.value = 0;
		gameRunning = true;
	}

	public bool StartCondition()
	{
		bool hasItems = false;
		eventBrokerComponent.Publish(this, new InventoryEvents.HasItem((success) => hasItems = success, neededItems.ToArray()));

		return hasItems;
	}

	private void SpawnBlood()
	{
		Vector3 spawnPos = new Vector3(Random.Range(minBloodSpawnPos.x, maxBloodSpawnPos.x), Random.Range(minBloodSpawnPos.y, maxBloodSpawnPos.y), 0f);
		GameObject blood = Instantiate(bloodPrefab, spawnPos, Quaternion.identity);
		blood.transform.SetParent(bloodHolder);
		blood.GetComponent<BloodDrop>().Initialize(bloodFallSpeed, OnBloodCollected, OnBloodOutOfBounds);
		bloodList.Add(blood);
	}

	private void OnBloodCollected(GameObject blood)
	{
		bloodList.Remove(blood);
		Destroy(blood);

		totalBlood += 1;
		bloodSlider.value += 1;
	}

	private void OnBloodOutOfBounds(GameObject blood)
	{
		bloodList.Remove(blood);
		Destroy(blood);
	}

    // Update is called once per frame
    void Update()
    {
		if (!gameRunning)
		{
			return;
		}

		if (totalBlood == bloodNeeded)
		{
			Finish();
			return;
		}

        if (timer > 0)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			timer = Random.Range(bloodSpawnTimer - bloodSpawnTimerOffset, bloodSpawnTimer + bloodSpawnTimerOffset);
			SpawnBlood();
		}

		vial.transform.position = Input.mousePosition;
    }
}