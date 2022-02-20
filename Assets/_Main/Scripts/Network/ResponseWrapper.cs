using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ResponseWrapper
{
    public UnityWebRequest Request { get; private set; }

    public ResponseWrapper(UnityWebRequest request)
    {
        Request = request;
    }

    public long StatusCode
    {
        get { return Request.responseCode; }
    }

    public byte[] Data
    {
        get
        {
            byte[] _data;
            try
            {
                _data = Request.downloadHandler.data;
            }
            catch (Exception)
            {
                _data = null;
            }
            return _data;
        }
    }

    public string Text
    {
        get
        {
            string _text;
            try
            {
                _text = Request.downloadHandler.text;
            }
            catch (Exception)
            {
                _text = string.Empty;
            }
            return _text;
        }
    }

    public string Error
    {
        get { return Request.error; }
    }
}
