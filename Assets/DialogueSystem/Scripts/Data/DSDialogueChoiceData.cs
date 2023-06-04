using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Data
{
    using ScriptableObjects;
    [System.Serializable]
    public class DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DSDialogueSO NextDialogue { get; set; }

        [field: SerializeField] public bool HasSanityThreshold { get; set; }
        [field: SerializeField] public float SanityThreshold { get; set; }
        [field: SerializeField] public Constants.Sanity.SanityType SanityType { get; set; }
    }
}

