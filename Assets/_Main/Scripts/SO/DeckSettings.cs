using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckSettings", menuName = "GameSettings/DeckSettings")]
public class DeckSettings : ScriptableObject
{
    [field: SerializeField] public float fromDeckToHandMoveTime;
    [field: SerializeField] public int cardsStartCount;
}
