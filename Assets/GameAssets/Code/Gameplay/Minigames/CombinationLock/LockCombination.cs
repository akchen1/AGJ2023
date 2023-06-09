using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockCombination : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private string number1;
    private string number2;
    private string number3;
    private string number4;

    public string CorrectLockCombination = "1234";
    public string EnteredLockCombination;

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
            GameObject.Find("Box").GetComponent<BoxController>().OpenBox();
        }
    }
}
