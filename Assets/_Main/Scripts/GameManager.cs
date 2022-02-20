using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private PlayerGraveYard playerGraveYard;
    [SerializeField] private PlayerGameField playerGameField;
    [SerializeField] private CardsDisplaySettings cardsDisplaySettings;
    [SerializeField] private PlayerHandSettings playerHandSettings;
    [SerializeField] private Button changeRandomStatButton;

    private Player player;

    private void Awake()
    {
        changeRandomStatButton.onClick.AddListener(() => playerHand.ChangeRandomCardRandomStat());
    }

    private void Start()
    {
        player = new Player(1, deck, playerHand, playerGameField, playerGraveYard, cardsDisplaySettings);
        player.DrawCards(UnityEngine.Random.Range(playerHandSettings.minCardsCount, playerHandSettings.maxCardsCount + 1));
    }
}
