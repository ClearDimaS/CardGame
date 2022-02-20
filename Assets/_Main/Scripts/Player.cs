using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    private int playerID;
    private Deck playerDeck;
    private PlayerHand playerHand;
    private PlayerGameField playerGameField;
    private PlayerGraveYard playerGraveYard;
    private CardsDisplaySettings playerHandSettings;

    public Player(int playerID, Deck playerDeck, PlayerHand playerHand, PlayerGameField playerGameField, PlayerGraveYard playerGraveYard,
        CardsDisplaySettings playerHandSettings)
    {
        this.playerID = playerID;

        this.playerDeck = playerDeck;
        this.playerHand = playerHand;
        this.playerGameField = playerGameField;
        this.playerGraveYard = playerGraveYard;
        this.playerHandSettings = playerHandSettings;

        playerDeck.OwnerId = playerID;
        playerHand.OwnerId = playerID;
        playerGameField.OwnerId = playerID;
        playerGraveYard.OwnerId = playerID;

        playerHand.onCardDeath += playerGraveYard.AddCard;
    }

    internal void DrawCards(object p)
    {
        throw new NotImplementedException();
    }

    public void DrawCards(int cardsCount)
    {
        if (cardsCount <= 0)
            return;
        playerDeck.DrawACard(playerHandSettings.cardDrawViewportPoint, playerHandSettings.cardDrawViewportSize)
            .Done((card) =>
            {
                playerHand.AddCard(card);
                DrawCards(--cardsCount);
            });
    }
}
