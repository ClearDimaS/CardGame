using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPInteractor : MonoBehaviour
{
    [SerializeField] private NetworkSettings networkSettings;
    private Queue<HTTPRequestSync> requestSyncs = new Queue<HTTPRequestSync>();
    private bool isHandlingRequest = false;

    private class HTTPRequestSync
    {
        public RequestWrapper requestWrapper;
        public Action<ResponseWrapper> successCallback { get; private set; }
        public Action<Exception> errorCallback { get; private set; }

        public HTTPRequestSync(RequestWrapper requestWrapper,
            Action<ResponseWrapper> successCallback,
            Action<Exception> errorCallback)
        {
            this.requestWrapper = requestWrapper;
            this.successCallback = successCallback;
            this.errorCallback = errorCallback;
        }
    }

    public Deferred<Texture2D> DownloadRandomPicture()
    {
        var requestWrapper = CreateRequestWrapper(networkSettings.randomPictureUri);
        return MakeRequestWithResponse<Texture2D>(requestWrapper);
    }

    private void Update()
    {
        if (!isHandlingRequest && requestSyncs.Count > 0)
        {
            isHandlingRequest = true;
            var nextRequestSync = requestSyncs.Dequeue();
            DoRequest(nextRequestSync);
        }
    }

    private void DoRequest(HTTPRequestSync requestSync)
    {
        var responseDeferred = new Deferred<ResponseWrapper>();
        var requestCouroutine = CreateRequestAndRetry(requestSync.requestWrapper, (error, responseWrapper) =>
        {
            if (responseWrapper.Error == null)
            {
                responseDeferred.Resolve(responseWrapper);
                requestSync.successCallback?.Invoke(responseWrapper);
            }
            else
            {
                responseDeferred.Reject(error);
                requestSync.errorCallback?.Invoke(error);
            }
            isHandlingRequest = false;
        });
        StartCoroutine(requestCouroutine);
    }

    private IEnumerator CreateRequestAndRetry(RequestWrapper requestWrapper, Action<Exception, ResponseWrapper> callback)
    {
        int retries = requestWrapper.Retries;
        float startTime = Time.time;
        while (retries >= 0)
        {
            using (var request = CreateWebRequest(requestWrapper))
            {
                yield return request.SendWebRequest();
                var response = new ResponseWrapper(request);

                if (request.result == UnityWebRequest.Result.ConnectionError && Time.time - startTime > requestWrapper.Timeout)
                {
                    yield return new WaitForSeconds(requestWrapper.RetriesPause);
                }
                else
                {
                    callback(new Exception(), response);
                    retries = -10;
                }
                retries--;
            }
        }
    }

    private UnityWebRequest CreateWebRequest(RequestWrapper requestWrapper)
    {
        var url = requestWrapper.Uri;
        return UnityWebRequest.Get(url);
    }

    private RequestWrapper CreateRequestWrapper(string Uri)
    {
        var request = new RequestWrapper()
        {
            Uri = Uri,
            Retries = networkSettings.retries,
            RetriesPause = networkSettings.retries
        };
        return request;
    }

    private Deferred<T> MakeRequestWithResponse<T>(RequestWrapper request) where T : class
    {
        var deferred = new Deferred<T>();
        var requestSync = new HTTPRequestSync(request,
                                             (response) => HandleResponse(request.Uri, response.Data, deferred),
                                             (error) => deferred.Reject(error));
        requestSyncs.Enqueue(requestSync);
        return deferred;
    }

    private void HandleResponse<T>(string path, byte[] rawData, Deferred<T> deferred) where T : class
    {
        T responseObject = null;
        print($"{path} response {rawData}");
        Dictionary<Type, int> typeDict = new Dictionary<Type, int>
        {
            {typeof(Texture2D), 0},
        };
        Debug.Log(rawData.Length);
        typeDict.TryGetValue(typeof(T), out int typeCode);
        switch (typeCode)
        {
            case 0:
                var textureResponse = new Texture2D(2, 2, TextureFormat.BGRA32, false);
                textureResponse.LoadImage(rawData);
                responseObject = textureResponse as T;
                break;
            default:
                Debug.LogError($"conversion from bytes to {typeof(T)} is not implemented");
                break;
        }

        deferred.Resolve(responseObject);
    }
}
