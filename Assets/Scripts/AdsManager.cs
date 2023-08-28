using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowBannerWhenReady());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShowBannerWhenReady()
    {
        /*
        while (!Advertisement.IsReady("Banner"))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show("Banner");
        */
        yield return null;
    }
}
