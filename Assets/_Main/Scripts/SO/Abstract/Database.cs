using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public abstract class Database<T> : ScriptableObject where T : class, new()
{
#if UNITY_EDITOR
    [ReadOnly]
#endif
    [SerializeField] private int currentIndex = 0;

    [SerializeField, HideInInspector] protected List<T> elementsList = new List<T>();

    public virtual List<T> GetElements()
    {
        return elementsList;
    }

    [SerializeField] protected T currentElement;

    public int GetElementsCount()
    {
        return elementsList.Count;
    }

    public virtual void AddElement()
    {
        if (elementsList == null)
            elementsList = new List<T>();

        currentElement = new T();
        elementsList.Add(currentElement);
        currentIndex = elementsList.Count - 1;
    }

    public virtual T GetNext()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif
        if (currentIndex < elementsList.Count - 1)
            currentIndex++;
        currentElement = this[currentIndex];
        return currentElement;
    }

    public virtual T GetPrev()
    {
        if (currentIndex > 0)
            currentIndex--;
        currentElement = this[currentIndex];
        return currentElement;
    }

    public virtual T GetCurrent()
    {
        if (elementsList.Count == 0)
            return null;

        currentElement = this[currentIndex];
        return currentElement;
    }

    public virtual void ClearDatabase()
    {
        elementsList.Clear();
        elementsList.Add(new T());
        currentElement = elementsList[0];
        currentIndex = 0;
    }

    public virtual void GoToIndex(int newInd)
    {
        newInd = Mathf.Clamp(newInd, 0, elementsList.Count - 1);
        currentIndex = newInd;
        currentElement = this[currentIndex];
    }

    public virtual T GetRandomElement()
    {
        int random = UnityEngine.Random.Range(0, elementsList.Count);
        return elementsList[random];
    }

    public virtual void RemoveCurrentElement()
    {
        if (elementsList.Count > 0)
        {
            elementsList.RemoveAt(currentIndex);
            currentIndex = (int)Mathf.Clamp(--currentIndex, 0, Mathf.Infinity);
            currentElement = elementsList[currentIndex];
        }
        else
        {
            ClearDatabase();
        }
    }

    public virtual T this[int index]
    {
        get
        {
            if (elementsList != null && index >= 0 && index < elementsList.Count)
                return elementsList[index];
            return null;
        }

        set
        {
            if (elementsList == null)
                elementsList = new List<T>();

            if (index >= 0 && index < elementsList.Count && value != null)
                elementsList[index] = value;
        }
    }
}
