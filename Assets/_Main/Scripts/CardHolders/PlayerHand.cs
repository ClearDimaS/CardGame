using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PlayerHand : CardHolder
{
    [SerializeField] private int renderIndexOffset;
    [SerializeField] private PlayerHandSettings playerHandSettings;
    [SerializeField] private float maxHandWidth;
    [SerializeField] private float cardsSpacing;
    [SerializeField] private float middleCardYAdd;

    public event Action<Card> onCardDeath;

    void Start()
    {
        RepositionCardsInViewportBounds();
    }

    public override void AddCard(Card card)
    {
        base.AddCard(card);
        card.SetIndex(cards.Count + renderIndexOffset);
        UpdateCardsPositions();
        card.ChangeCardState(ECardState.InHand);
        card.OwnerId = ownerId;
        card.onDeath += OnCardDeath;

        Action<Card> onDragAborted = null;
        onDragAborted = (card) =>
        {
            card.onCardDragged -= RemoveCard;
            card.onCardDragAborted -= onDragAborted;
            AddCard(card);
        };
        card.onCardDragged += RemoveCard;
        card.onCardDragAborted += onDragAborted;
    }

    public override void RemoveCard(Card card)
    {
        base.RemoveCard(card);
        card.onDeath -= OnCardDeath;
    }

    protected override void UpdateCardsPositions()
    {
        int index = 0;
        int tottal = cards.Count;
        foreach (var card in cards)
        {
            GetPositionAndRotationAtIndex(index, tottal, out Vector3 position, out Quaternion rotation);
            card.MoveTo(position, playerHandSettings.cardsRepositionTime);
            card.RotateTo(rotation, playerHandSettings.cardsRepositionTime);
            index++;
        }
    }

    public void ChangeRandomCardRandomStat()
    {
        if (cards.Count == 0)
            return;
        int randomIndex = UnityEngine.Random.Range(0, cards.Count);
        cards.ElementAt(randomIndex).ChangeRandomStat();
    }

    private void OnCardDeath(Card card)
    {
        RemoveCard(card);
        onCardDeath?.Invoke(card);
    }

    private void GetPositionAndRotationAtIndex(int index, int tottal, out Vector3 position, out Quaternion rotation)
    {
        float normalizedIndex = (index + 1) / (float)(tottal + 1);
        if (tottal * cardsSpacing > maxHandWidth)
        {
            position = transform.position + 2 * (normalizedIndex - 0.5f) * maxHandWidth / 2f * Vector3.right;
        }
        else
        {
            position = transform.position + (index - tottal / 2) * cardsSpacing * Vector3.right;
        }
        position += Vector3.up * middleCardYAdd * (0.5f - Mathf.Abs(2 * (normalizedIndex - 0.5f)));

        rotation = Quaternion.Euler(0, 180f, Mathf.Lerp(-playerHandSettings.maxZEuler, playerHandSettings.maxZEuler, normalizedIndex));
    }


    private void RepositionCardsInViewportBounds()
    {
        Vector3 leftCardPos = transform.position + Vector3.left * maxHandWidth / 2f;
        var vieportEdgePosLeft = Camera.main.WorldToViewportPoint(leftCardPos);
        while (Mathf.Abs(vieportEdgePosLeft.x - playerHandSettings.minCardViewportPosX) > 0.1f)
        {
            vieportEdgePosLeft.x = playerHandSettings.minCardViewportPosX;
            var leftCardDesiredWorldPos = Camera.main.ViewportToWorldPoint(vieportEdgePosLeft);
            transform.position += Vector3.forward * (leftCardDesiredWorldPos.x - leftCardPos.x) * (2.0f * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad));
            leftCardPos = transform.position + Vector3.left * maxHandWidth / 2f;
            vieportEdgePosLeft = Camera.main.WorldToViewportPoint(leftCardPos);
        }

        Vector3 middleCardPos = transform.position;
        var vieportEdgePosMiddle = Camera.main.WorldToViewportPoint(middleCardPos);
        if (Mathf.Abs(vieportEdgePosMiddle.x - playerHandSettings.desiredCardViewportPosY) > 0.1f)
        {
            vieportEdgePosMiddle.y = playerHandSettings.desiredCardViewportPosY;
            var middleCardDesiredWorldPos = Camera.main.ViewportToWorldPoint(vieportEdgePosMiddle);
            transform.position = middleCardDesiredWorldPos;
            leftCardPos = transform.position + Vector3.left * maxHandWidth / 2f;
            vieportEdgePosMiddle = Camera.main.WorldToViewportPoint(leftCardPos);
        }
    }
}
