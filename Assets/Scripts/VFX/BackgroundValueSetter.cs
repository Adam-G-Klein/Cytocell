using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundValueSetter : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer rend;
    [SerializeField]
    float dimmness ;
    [SerializeField]
    float size= -11.68f;
    [SerializeField]
    Color backgroundColor;
    [SerializeField]
    Color mainColor;

    public List<Color> colors = new List<Color>();
    private int ltidColor = -1;
    [SerializeField]
    float colorCycleTime = 10f;
    int colorIndex = 0;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.material.SetFloat("_colorDimmer", dimmness);
        rend.material.SetFloat("_Size", size);
        rend.material.SetColor("_BackColor", backgroundColor);
        mainColor = colors[0];
        rend.material.SetColor("_Color", mainColor);
        StartCoroutine(cycleThroughColors());
    }

    void Update() {
        rend.material.SetFloat("_colorDimmer", dimmness);
        rend.material.SetFloat("_Size", size);
        rend.material.SetColor("_BackColor", backgroundColor);
    }

    IEnumerator cycleThroughColors() {
        int nextColorIndex = 1;
        while(true) {
            nextColorIndex = (nextColorIndex + 1) % colors.Count;
            yield return new WaitUntil(() => ltidColor == -1 || LeanTween.isTweening(ltidColor) == false);
            ltidColor = LeanTween.value(gameObject, colors[colorIndex], colors[nextColorIndex], colorCycleTime).setOnUpdate((Color val) => {
                mainColor = val;
                rend.material.SetColor("_Color", mainColor);
            }).id;
            colorIndex = nextColorIndex;
            yield return new WaitForSeconds(colorCycleTime);
        }
    }
}
