using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//TODO cards fill the deck in the start of the game and can return nicely
public class Deck : CardHolder
{
    [SerializeField] private HTTPInteractor httpInteractor;
    [SerializeField] private CardsDB cardsDB;
    [SerializeField] private DeckSettings deckSettings;
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private GameObject deckGFX;

    private Queue<Card> spawnedCards = new Queue<Card>();
    private CardFactory cardFactory;

    private int currentCardsCount;
    private int startCardsCount;

    private void Awake()
    {
        currentCardsCount = deckSettings.cardsStartCount;
        startCardsCount = deckSettings.cardsStartCount;
        cardFactory = new CardFactory(cardsDB);
    }

    public override void AddCard(Card card)
    {
        base.AddCard(card);
        UpdateCardsPositions();
        card.ChangeCardState(ECardState.InDeck);
    }

    public Deferred<Card> DrawACard(Vector2 finishMovingViewportPos, Vector2 cardViewportBBSize)
    {
        var deferred = new Deferred<Card>();
        if (currentCardsCount > 0)
        {
            httpInteractor.DownloadRandomPicture().Done((cardTexture) =>
            {
                var cardModel = DrawACard(cardTexture);
                var card = GetCard();
                card.Init(cardModel);
                UpdateCardsPositions();
                var toPos = GetPosMoveFromDeck(card.boxCollider.size, finishMovingViewportPos, cardViewportBBSize);
                card.MoveTo(toPos, deckSettings.fromDeckToHandMoveTime, () => deferred.Resolve(card));
            });
        }
        return deferred;
    }

    private CardModel DrawACard(Texture2D cardTexture)
    {
        currentCardsCount--;
        return cardFactory.Create(cardTexture);
    }

    private Card GetCard()
    {
        if (spawnedCards.Count == 0)
            spawnedCards.Enqueue(Instantiate(cardPrefab));
        var card = spawnedCards.Dequeue();

        card.gameObject.SetActive(true);
        card.transform.position = cardSpawnPoint.position;

        return card;
    }

    protected override void UpdateCardsPositions()
    {
        deckGFX.transform.localScale = new Vector3(1, currentCardsCount / (float)startCardsCount, 1);
        foreach (var item in cards)
        {
            item.MoveTo(transform.position, 0.4f);
            item.RotateTo(Quaternion.Euler(90f, 0f, 0f), 0.4f);
        }
    }

    private Vector3 GetPosMoveFromDeck(Vector3 cardPhysicalSize, Vector2 finishMovingViewportPos, Vector2 cardViewportBBSize)
    {
        float z = cardPhysicalSize.x / (2.0f * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad)) / cardViewportBBSize.x;
        Vector3 toPos = Camera.main.ViewportToWorldPoint(new Vector3(finishMovingViewportPos.x, finishMovingViewportPos.y, z));
        return toPos;
    }
}
