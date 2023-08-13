using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Clickable))]
public class DisableAdsButton : MonoBehaviour
{
    public UnityEngine.SceneManagement.Scene gameScene;
    private Clickable clickable;
    // Start is called before the first frame update
    void Start()
    {
        clickable = GetComponent<Clickable>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clicked(){
        //TODO
    }
}
