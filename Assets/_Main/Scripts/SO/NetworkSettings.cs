using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkSettings", menuName = "GameSettings/Network")]
public class NetworkSettings : ScriptableObject
{
    [field: SerializeField] public string randomPictureUri;
    [field: SerializeField] public int retries;
    [field: SerializeField] public int retryPause;
}
