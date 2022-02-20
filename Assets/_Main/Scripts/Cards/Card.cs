using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Card : MonoBehaviour, IOwnable
{
    [field: SerializeField] public BoxCollider boxCollider { get; private set; }
    [SerializeField] private CardMover cardMover;
    [SerializeField] private CardInfoPanel cardInfoPanel;
    [SerializeField] private CardGFX cardGFX;
    [SerializeField] private CardInput cardInput;

    private CardModel cardModel;
    private int ownerId;

    public event Action<Card> onCardDragged;
    public event Action<Card> onCardDragAborted;
    public event Action<Card> onDeath;

    public int OwnerId { get => ownerId; set => ownerId = value; }
    public ECardState cardState { get; private set; }
    public IHaveCards cardHolder { get; private set; }

    private void Awake()
    {
        cardInput.Init(this);
        cardInput.onCardDragged += () =>
        {
            cardGFX.Shine();
            onCardDragged?.Invoke(this);
        };
        cardInput.onCardDragAbborted += () =>
        {
            onCardDragAborted?.Invoke(this);
            cardGFX.StopShining();
        };
    }

    public void Init(CardModel cardModel)
    {
        this.cardModel = cardModel;
        cardInfoPanel.Init(new CardInfoPanel.InitData
        {
            texture = cardModel.cardTexture,
            attack = cardModel.attack.CurrentValue,
            currentHealth = cardModel.currentHealth.CurrentValue,
            maxHealth = cardModel.maxHealth.CurrentValue,
            manaCost = cardModel.manaCost.CurrentValue,
            title = cardModel.title,
            description = cardModel.description
        });

        cardModel.attack.onValueChanged += (oldValue, newValue) => cardInfoPanel.SetAttack(oldValue, cardModel.attack.CurrentValue, cardModel.attack.originalValue);
        cardModel.currentHealth.onValueChanged += (oldValue, newValue) => cardInfoPanel.SetCurrentHealth(oldValue, cardModel.currentHealth.CurrentValue, cardModel.currentHealth.originalValue, CheckIfDead);
        cardModel.maxHealth.onValueChanged += (oldValue, newValue) =>
        {
            if (cardModel.currentHealth.CurrentValue > newValue)
                cardModel.currentHealth.CurrentValue = newValue;
            cardInfoPanel.SetMaxHealth(oldValue, cardModel.maxHealth.CurrentValue, cardModel.maxHealth.originalValue);
        };
        cardModel.manaCost.onValueChanged += (oldValue, newValue) => cardInfoPanel.SetMana(oldValue, cardModel.manaCost.CurrentValue, cardModel.manaCost.originalValue);
    }

    public void SetIndex(int index)
    {
        cardInfoPanel.SetRenderIndex(index);
    }

    public void SetCardHolder(IHaveCards cardHolder)
    {
        this.cardHolder = cardHolder;
    }

    public void AddOffset(Vector3 lPositionOffset) => cardGFX.AddOffset(lPositionOffset);

    public void RemoveOffset() => cardGFX.RemoveOffset();

    public void MoveTo(Vector3 positionW, float time, Action onComplete = null) => cardMover.MoveTo(positionW, time, onComplete); 

    public void RotateTo(Quaternion rotation, float time, Action onComplete = null) => cardMover.RotateTo(rotation, time, onComplete);

    public void ScaleTo(Vector3 scale, float time, Action onComplete = null) => cardMover.ScaleTo(scale, time, onComplete);

    public void ChangeCardState(ECardState newCardState)
    {
        cardState = newCardState;
        onCardDragAborted = null;
        onCardDragged = null;
        RemoveOffset();
        cardGFX.StopShining(); 
    }

    public void ChangeRandomStat()
    {
        int randomVal = UnityEngine.Random.Range(0, 4);
        int minChange = -2;
        int maxChange = 9;

        switch (randomVal)
        {
            case 0:
                cardModel.attack.CurrentValue = UnityEngine.Random.Range(minChange, maxChange+1);
                break;
            case 1:
                cardModel.currentHealth.CurrentValue = UnityEngine.Random.Range(minChange, maxChange + 1);
                break;
            case 2:
                cardModel.maxHealth.CurrentValue = UnityEngine.Random.Range(minChange, maxChange + 1);
                break;
            case 3:
                cardModel.manaCost.CurrentValue = UnityEngine.Random.Range(minChange, maxChange + 1);
                break;
            default:
                break;
        }
    }

    private void CheckIfDead()
    {
        if (cardModel.currentHealth.CurrentValue <= 0)
        {
            ChangeCardState(cardState);
            onDeath?.Invoke(this);
        }
    }
}
