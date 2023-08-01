using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resolution {
    public int width;
    public int height;

    public Resolution(int width, int height){
        this.width = width;
        this.height = height;
    }
}
// An attempt at manipulating the JS container Unity WebGL runs inside of
// Was able to contact the "set" API but ran into issues querying the browser's
// current window size.
public class ResolutionSetter : MonoBehaviour
{

    public GameObject scoreTextObj;
    private TextMeshProUGUI outText;
    // Start is called before the first frame update
    void Start()
    {
        outText = scoreTextObj.GetComponent<TextMeshProUGUI>();
    }
        

    public void setResolution(string newVal){
        outText.SetText("Resolution Set To: " + newVal);
        Resolution newRes = parseRes(newVal);
        Screen.SetResolution(newRes.width, newRes.height, true);
    }

    private Resolution parseRes(string val){
        string[] tokens = val.Split();
        return new Resolution(int.Parse(tokens[0]), int.Parse(tokens[1]));
    }


}
