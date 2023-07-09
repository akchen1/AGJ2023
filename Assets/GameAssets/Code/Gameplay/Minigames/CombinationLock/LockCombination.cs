using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class LockCombination : MonoBehaviour, IMinigame
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private string number1;
    private string number2;
    private string number3;
    private string number4;
    private bool solved;
    [SerializeField] private InventoryItem gemItem;
    [SerializeField] private PlayableAsset gemCollectCutscene;
    [SerializeField] private string CorrectLockCombination = "1234";
    [SerializeField] private string EnteredLockCombination;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject gem;
    [SerializeField] private GameObject backgroundButton;
    [SerializeField] private GameObject minigameStarter;
    [SerializeField] private GameObject box;
    [SerializeField] private PlayableDirector playableDirector;
    



   #region IMinigame Methods
    public void Finish()
    {
        if(solved){
            playableDirector.Play(gemCollectCutscene);
            if (!gemItem.CheckInInventory(this))
            {
                gemItem.AddToInventory(this);
            }
            box.SetActive(false);
            minigameStarter.SetActive(false);
        }
        this.EndMinigame();
        panel.SetActive(false);
        eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
    }

    public void Initialize()
    {
        panel.SetActive(true);
    }

    public bool StartCondition()
    {
        return true;
    }
    #endregion

    public void CheckCorrect()
    {
        eventBrokerComponent.Publish(this, new LockEvent.GetCombination((number, id) => 
        {
            if(id == 1)
                number1 = number;
            else if(id == 2)
                number2 = number;
            else if(id == 3)
                number3 = number;
            else if(id == 4)
                number4 = number;
        }));
        EnteredLockCombination = number1 + number2 + number3 + number4;
        if(CorrectLockCombination == EnteredLockCombination)
        {
            solved = true;
            box.GetComponent<BoxController>().OpenBox();
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BoxOpen));
            gem.SetActive(true);
            //Invoke("Finish", 1.5f);
        }
    }
}
