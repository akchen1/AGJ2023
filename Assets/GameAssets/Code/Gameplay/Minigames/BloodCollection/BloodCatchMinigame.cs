using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodCatchMinigame : MonoBehaviour, IMinigame
{
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	[SerializeField] private List<InventoryItem> neededItems;
	[SerializeField] private InventoryItem vialOfBlood;
	[SerializeField] private int bloodNeeded;

	[SerializeField, Header("UI")] private GameObject panel;
	[SerializeField] private Slider bloodSlider;
	[SerializeField] private RectTransform destroyZone;

	[SerializeField, Header("Vial")] private GameObject vial;

	[SerializeField, Header("Blood")] private GameObject bloodPrefab;
	[SerializeField] private RectTransform bloodSpawnBounds;
	[SerializeField] private RectTransform bloodBounds;
	private Vector2 minBloodSpawnPos;
	private Vector2 maxBloodSpawnPos;
	[SerializeField] private float bloodSpawnTimer;
	[SerializeField] private float bloodSpawnTimerOffset;
	[SerializeField] private float bloodFallSpeed;
	[SerializeField] private Transform bloodHolder;

	[SerializeField, Header("Starting Dialogue Options")] private DSDialogueSO negativeDialogue;
	[SerializeField] private DSDialogueSO neutralDialogue;
	[SerializeField] private DSDialogueSO positiveDialogue;

	private bool gameRunning = false;
	private int totalBlood;

	private HashSet<RectTransform> bloodList = new HashSet<RectTransform>();

	private float timer;

	public void Finish()
	{
		eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);

		foreach(RectTransform blood in bloodList)
		{
			Destroy(blood.gameObject);
		}

        eventBrokerComponent.Publish(this, new InventoryEvents.RemoveItem(neededItems.ToArray()));
		eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(vialOfBlood));

        gameRunning = false;
		panel.SetActive(false);
		this.EndMinigame();
		Destroy(this.gameObject);
	}

	public void Initialize()
	{
		Vector3[] worldCorners = new Vector3[4];
		bloodSpawnBounds.GetWorldCorners(worldCorners);
		minBloodSpawnPos = worldCorners[0];
		maxBloodSpawnPos = worldCorners[2];

        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
		StartStartingDialogue();
	}

    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> obj)
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
		GameObject blood = Instantiate(bloodPrefab, spawnPos, Quaternion.identity, bloodHolder);
		//blood.transform.SetParent(bloodHolder);
		blood.GetComponent<BloodDrop>().Initialize(bloodFallSpeed, OnBloodCollected);
		bloodList.Add(blood.GetComponent<RectTransform>());
	}

	private void OnBloodCollected(GameObject blood)
	{
		eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BloodDrop));
		bloodList.Remove(blood.GetComponent<RectTransform>());
		Destroy(blood.gameObject);

		totalBlood += 1;
		bloodSlider.value += 1;
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

		CheckBloodBounds();
    }

	private void StartStartingDialogue()
	{
		eventBrokerComponent.Publish(this, new Scene7Events.GetBloodSanityResult(sanityType =>
		{
			DSDialogueSO dialogue = neutralDialogue;
			if (sanityType == Constants.Sanity.SanityType.Negative)
			{
				dialogue = negativeDialogue;
			}
			else if (sanityType == Constants.Sanity.SanityType.Positive)
			{
				dialogue = positiveDialogue;
			}
			eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(dialogue));
		}));
	}

	private void CheckBloodBounds()
	{
		bloodList.RemoveWhere(blood =>
		{
			if (destroyZone.RectOverlapsPoint(blood))
			{
				Destroy(blood.gameObject);
				return true;
			}
			return false;
		});
	}
}