using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{

    private VisualManager visualManager;
    // Start is called before the first frame update
    void Start()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP);
        if(PlayerPrefs.GetInt("adsRemoved", 0) == 1){
            IronSource.Agent.hideBanner();
            Debug.Log("ads disabled");
        } else {
            IronSource.Agent.displayBanner();
            GameObject visualManagerGO = GameObject.FindGameObjectWithTag("Canvas");
            if(visualManagerGO) {
                visualManager = visualManagerGO.GetComponent<VisualManager>();
                visualManager.applyBannerOffset();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


/*
    private IEnumerator ShowBannerWhenReady()
    {
        Debug.Log("unity-script: ShowBannerWhenReady");
        yield return new WaitUntil(() => initComplete);
        Debug.Log("unity-script: initComplete");

        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.TOP);
        if(PlayerPrefs.GetInt("adsRemoved", 0) == 1){
            IronSource.Agent.hideBanner();
            Debug.Log("ads disabled");
        } else {
            IronSource.Agent.displayBanner();
            GameObject visualManagerGO = GameObject.FindGameObjectWithTag("Canvas");
            if(visualManagerGO) {
                visualManager = visualManagerGO.GetComponent<VisualManager>();
                visualManager.applyBannerOffset();
            }
        }
        yield return null;
    }
    */

}
