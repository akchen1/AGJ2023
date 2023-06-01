using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Character : ScriptableObject
{
    [field: SerializeField] public string CharacterName { get; private set; }
}
