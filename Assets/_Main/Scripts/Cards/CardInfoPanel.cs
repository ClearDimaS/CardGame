using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour
{
    public struct InitData
    {
        public Texture2D texture;
        public int attack;
        public int currentHealth;
        public int maxHealth;
        public int manaCost;
        public string title;
        public string description;
    }

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image cardIcon;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private TMP_Text maxHealthText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private TMP_Text attackText;

    [SerializeField] private Color debuffColor;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private float oneStatChangeTime = 0.3f;

    public void SetRenderIndex(int index)
    {
        canvas.sortingOrder = index;
    }

    public void Init(InitData initData)
    {
        cardIcon.sprite = Texture2DToSprite(initData.texture);
        titleText.text = initData.title.ToString();
        healthText.text = initData.currentHealth.ToString();
        maxHealthText.text = initData.maxHealth.ToString();
        manaText.text = initData.manaCost.ToString();
        attackText.text = initData.attack.ToString();
        descriptionText.text = initData.description;
    }

    public void SetCurrentHealth(int oldValue, int currentValue, int originalValue, Action onComplete)
    {
        SetTextColor(healthText, currentValue, originalValue);
        ChangeStatText(healthText, oldValue, currentValue, onComplete);
    }

    public void SetMaxHealth(int oldValue, int currentValue, int originalValue)
    {
        SetTextColor(maxHealthText, currentValue, originalValue);
        ChangeStatText(maxHealthText, oldValue, currentValue);
    }

    public void SetMana(int oldValue, int currentValue, int originalValue)
    {
        SetTextColor(manaText, currentValue, originalValue);
        ChangeStatText(manaText, oldValue, currentValue);
    }

    public void SetAttack(int oldValue, int currentValue, int originalValue)
    {
        SetTextColor(attackText, currentValue, originalValue);
        ChangeStatText(attackText, oldValue, currentValue);
    }

    private void SetTextColor(TMP_Text text, int currentValue, int originalValue)
    {
        if (currentValue < originalValue)
            text.color = debuffColor;
        else if (currentValue > originalValue)
            text.color = buffColor;
        else
            text.color = normalColor;
    }

    private void ChangeStatText(TMP_Text text, int oldStat, int newStat, Action onComplete = null)
    {
        onComplete += () => text.transform.DOScale(Vector3.one, 0.5f);
        text.transform.DOScale(Vector3.one * 3f, 0.5f).OnComplete(() =>
        {
            var cor = StartCoroutine(ChangeStatTextCoroutine(text, oldStat, newStat, onComplete));
        });
    }

    private IEnumerator ChangeStatTextCoroutine(TMP_Text text, int oldStat, int newStat, Action onComplete)
    {
        int current = oldStat;
        int add = newStat > oldStat ? 1 : -1;

        while (current != newStat)
        {
            current += add;
            text.text = current.ToString();
            yield return new WaitForSeconds(oneStatChangeTime);
        }
        onComplete?.Invoke();
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.one / 2f);
    }
}
