using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsInitialization : MonoBehaviour
{
    [SerializeField]
    private string ironSourceIosAppKey = "1b4b0e1dd";
    private bool initComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        string appKey;
        #if UNITY_IPHONE
                appKey = ironSourceIosAppKey;
        #else
                appKey = "unexpected_platform";
        #endif

        IronSource.Agent.validateIntegration();
        Debug.Log("unity-script: IronSource.Agent.init");
        IronSource.Agent.init(appKey);
    }
}
