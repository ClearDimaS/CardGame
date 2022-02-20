using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory 
{
    private CardsDB cardsDB;

    public CardFactory(CardsDB cardsDB)
    {
        this.cardsDB = cardsDB;
    }

    public CardModel Create(Texture2D texture2D)
    {
        var cardData = cardsDB.GetRandomElement();
        return new CardModel()
        {
            cardTexture = texture2D,
            attack = new IntStat(cardData.attack),
            maxHealth = new IntStat(cardData.maxHealth),
            currentHealth = new IntStat(cardData.maxHealth),
            manaCost = new IntStat(cardData.manaCost),
            description = cardData.description,
            title = cardData.description
        };
    }
}
