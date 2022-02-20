using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardGFX : MonoBehaviour
{
    [SerializeField] private GameObject[] glows;

    private void Start()
    {
        StopShining();
    }

    public void Shine()
    {
        foreach (var glow in glows)
        {
            glow.gameObject.SetActive(true);
        }
    }

    public void StopShining()
    {
        foreach (var glow in glows)
        {
            glow.gameObject.SetActive(false);
        }
    }

    public void RemoveOffset()
    {
        transform.DOLocalMove(Vector3.zero, 0.3f);
    }

    public void AddOffset(Vector3 lPositionOffset)
    {
        transform.DOLocalMove(lPositionOffset, 0.3f);
    }
}
