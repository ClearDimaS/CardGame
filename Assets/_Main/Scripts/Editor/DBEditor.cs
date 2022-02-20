using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class DBEditor<T> : Editor where T : class, new()
{
    protected Database<T> database;

    private string goToNumFieldTxt;

    private void Awake()
    {
        database = (Database<T>)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("RemoveAll"))
        {
            database.ClearDatabase();
        }
        if (GUILayout.Button("Remove"))
        {
            database.RemoveCurrentElement();
        }
        if (GUILayout.Button("Add"))
        {
            database.AddElement();
        }
        if (GUILayout.Button("<="))
        {
            database.GetPrev();
        }
        if (GUILayout.Button("=>"))
        {
            database.GetNext();
        }

        GUILayout.EndHorizontal();

        DrawGoToField();

        //AssetDatabase.SaveAssets();
    }

    private void DrawGoToField()
    {
        goToNumFieldTxt = EditorGUILayout.TextField("Go to: ", goToNumFieldTxt);

        EditorUtility.SetDirty(target);

        if (Int32.TryParse(goToNumFieldTxt, out int result))
        {
            if (result >= database.GetElementsCount() || result < 0)
            {
                EditorGUILayout.LabelField($"Out of bounds!");
            }
            else
            {
                if (GUILayout.Button($"GoTo element {result}"))
                {
                    database.GoToIndex(result);

                    goToNumFieldTxt = "";
                }
            }
        }
    }

#if UNITY_EDITOR

    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;

    }
#endif
}
