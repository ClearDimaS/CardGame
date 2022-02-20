using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestWrapper
{
    public UnityWebRequest Request { private get; set; }

    public float Timeout;
    private string _uri;

    public string Uri
    {
        get { return _uri; }
        set { _uri = value; }
    }

    private int _retries;

    public int Retries
    {
        get { return _retries; }
        set { _retries = value; }
    }

    private float _retriesPause;

    public float RetriesPause
    {
        get { return _retriesPause; }
        set { _retriesPause = value; }
    }
}
