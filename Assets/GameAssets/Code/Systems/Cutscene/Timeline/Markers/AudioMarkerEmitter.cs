using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMarkerEmitter : MarkerEmitter
{
    [field: SerializeField] public string AudioName { get; private set; }
    [field: SerializeField] public bool IsMusic { get; private set; } = false;
}
