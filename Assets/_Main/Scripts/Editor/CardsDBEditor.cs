using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(CardsDB))]
public class CardsDBEditor : DBEditor<CardData>
{
    private void Awake()
    {
        database = (CardsDB)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
