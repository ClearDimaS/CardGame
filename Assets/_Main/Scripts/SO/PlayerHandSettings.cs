using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHandSettings", menuName = "GameSettings/PlayerHand")]
public class PlayerHandSettings : ScriptableObject
{
    [field: SerializeField] public int maxCardsCount { get; private set; }
    [field: SerializeField] public int minCardsCount { get; private set; }
    [field: SerializeField] public float fromHandToTableMoveTime { get; private set; }
    [field: SerializeField] public float maxZEuler { get; private set; }
    [field: SerializeField] public float cardsRepositionTime { get; private set; }
    [field: SerializeField] public float minCardViewportPosX { get; private set; } // for different screen ratios, so cards always are on the screen
    [field: SerializeField] public float desiredCardViewportPosY { get; private set; } // for different screen ratios, so cards always are on the screen
}
