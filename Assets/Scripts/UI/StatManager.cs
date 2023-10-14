using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatManager : MonoBehaviour
{
    private List<StatView> statViews = new List<StatView>();

    // Start is called before the first frame update
    void Start()
    {
        statViews.AddRange(GetComponentsInChildren<StatView>());
    }

    public void updateStats(){
        foreach(StatView view in statViews){
            view.updateStat();
        }
    }


}
