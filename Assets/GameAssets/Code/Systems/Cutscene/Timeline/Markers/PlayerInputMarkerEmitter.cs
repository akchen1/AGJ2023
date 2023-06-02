using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMarkerEmitter : MarkerEmitter
{
    [field: SerializeField] public bool Enable { get; private set; } = true;
}
