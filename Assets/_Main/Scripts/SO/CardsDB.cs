using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "CardsDB", menuName = "DBs/Cards", order = 0)]
public class CardsDB : Database<CardData>
{

}