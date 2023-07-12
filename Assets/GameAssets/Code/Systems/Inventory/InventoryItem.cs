using DS.ScriptableObjects;
using DS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItem : ScriptableObject, IInteractableVirtual
{
    [field: Header("Item Information")]
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public Sprite ItemIcon { get; private set; }
    [field: SerializeField] public string ItemDescription { get; private set; }
    [field: SerializeField] public string ItemObtainedSFX { get; private set; }
    [field: SerializeField] public InventoryInteraction InventoryInteraction { get; private set; }


    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void OnSelectInteraction()
    {
        if (InventoryInteraction != null && InventoryInteraction.OverrideDefaultClickInteraction)
        {
            InventoryInteraction.OnClickInteraction();
            return;
        }
        Interact();
    }

    public void Interact()
    {
        DSDialogueSO itemDescription = CreateInstance<DSDialogueSO>();
        itemDescription.Text = ItemDescription;
        itemDescription.Interact(this, Constants.Interaction.InteractionType.Virtual);
    }
}
