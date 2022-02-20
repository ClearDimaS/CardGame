using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsDisplaySettings", menuName = "GameSettings/CardsDisplay")]
public class CardsDisplaySettings : ScriptableObject
{
    [field: SerializeField] public Vector2 cardDrawViewportSize { get; private set; }
    [field: SerializeField] public Vector2 cardDrawViewportPoint { get; private set; }
}
