using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    [System.Serializable]
    public class DSChoiceSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }

        [field: SerializeField] public bool HasSanityThreshold { get; set; }
        [field: SerializeField] public float SanityThreshold { get; set; }
        [field: SerializeField] public Constants.Sanity.SanityType SanityType { get; set; }

    }
}

