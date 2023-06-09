using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DS.Data.Save
{
    using Enumerations;

    [Serializable]
    public class DSNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Text { get; set; }

        #region Case Specific
        [field: SerializeField] public Character Character { get; set; }
        [field: SerializeField] public InventoryItem Item { get; set; }
        [field: SerializeField] public bool HasSceneTransition { get; set; }
        [field: SerializeField] public int NextSceneIndex { get; set; }
        [field: SerializeField] public string NextSceneName { get; set; }
        [field: SerializeField] public AudioClip AudioClip { get; set; }
        #endregion
        [field: SerializeField] public List<DSChoiceSaveData> Choices { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public DSDialogueType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}
