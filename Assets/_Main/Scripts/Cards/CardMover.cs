using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardMover : MonoBehaviour
{
    private Queue<Tween> moveQueue = new Queue<Tween>();
    private Queue<Tween> rotateQueue = new Queue<Tween>();
    private Queue<Tween> scaleQueue = new Queue<Tween>();

    private bool isMoving, isRotating, isScaling;

    public void MoveTo(Vector3 positionW, float time, Action onComplete = null)
    {
        var tween = transform.DOMove(positionW, time).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (moveQueue.Count > 0)
            {
                var nextTween = moveQueue.Dequeue();
                nextTween.Play();
            }
            else
                isMoving = false;

        });
        if (isMoving)
        {
            tween.Pause();
            moveQueue.Enqueue(tween);
        }
        else
        {
            isMoving = true;
            tween.Play();
        }
    }

    public void RotateTo(Quaternion rotation, float time, Action onComplete = null)
    {
        var tween = transform.DORotate(rotation.eulerAngles, time).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (rotateQueue.Count > 0)
            {
                var nextTween = rotateQueue.Dequeue();
                nextTween.Play();
            }
            else
                isRotating = false;

        });
        if (isRotating)
        {
            tween.Pause();
            rotateQueue.Enqueue(tween);
        }
        else
        {
            isRotating = true;
            tween.Play();
        }
    }

    public void ScaleTo(Vector3 scale, float time, Action onComplete = null)
    {
        var tween = transform.DOScale(scale, time).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (scaleQueue.Count > 0)
            {
                var nextTween = scaleQueue.Dequeue();
                nextTween.Play();
            }
            else
                isScaling = false;

        });
        if (isScaling)
        {
            tween.Pause();
            scaleQueue.Enqueue(tween);
        }
        else
        {
            isScaling = true;
            tween.Play();
        }
    }
}
