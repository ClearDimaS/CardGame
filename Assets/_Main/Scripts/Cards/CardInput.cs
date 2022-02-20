using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInput : MonoBehaviour
{
    [SerializeField] private float mouseOverAddHeight;

    private Card card;
    private bool isPressed;
    private bool isMouseOver;
    private Vector3 originalPositionDrag;
    private Vector3 originalPositionMouseEnter;
    public event Action onCardDragged;
    public event Action onCardDragAbborted;

    public void Init(Card card)
    {
        this.card = card;
    }

    private void OnMouseDown()
    {
        if (card.cardState == ECardState.InDeck || card.cardState == ECardState.InGraveYard)
            return;
        isPressed = true;
        card.RotateTo(Quaternion.Euler(card.transform.rotation.eulerAngles.x, card.transform.rotation.eulerAngles.y, 0f), 0.2f);
        originalPositionDrag = card.transform.position;
        onCardDragged?.Invoke();
    }

    private void OnMouseEnter()
    {
        if (card.cardState != ECardState.InHand)
            return;
        originalPositionMouseEnter = card.transform.position;
        card.AddOffset(card.transform.up * mouseOverAddHeight);
    }

    private void OnMouseExit()
    {
        card.RemoveOffset();
    }

    private void OnMouseDrag()
    {
        if(isPressed)
            card.MoveTo(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + originalPositionDrag.z)), 0f);
    }

    private void OnMouseUp()
    {
        isPressed = false;
        var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        foreach (var hit in hits)
        {
            Transform objectHit = hit.transform;
            var cardTarget = objectHit.GetComponent<ICardTarget>();
            if (cardTarget != null)
            {
                if (cardTarget.TryInteract(card))
                {
                    return;
                }
            }
        }
        ResetPos();
    }

    private void ResetPos()
    {
        onCardDragAbborted?.Invoke();
    }
}
