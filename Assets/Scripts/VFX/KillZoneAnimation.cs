using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneAnimation : MonoBehaviour
{
    [SerializeField]
    private int numLines = 20;
    [SerializeField]
    private GameObject linePrefab;
    private List<LineRenderer> lines = new List<LineRenderer>();
    [SerializeField]
    private float fadeDelay = 0.2f;
    [SerializeField]
    private float fadeTime = 1.5f;
    [SerializeField]
    private Color lineStartColor = new Color(0, 0, 0, 1);
    [SerializeField]
    private Color lineEndColor = new Color(0, 0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numLines; i++){
            GameObject line = Instantiate(linePrefab, transform);
            lines.Add(line.GetComponent<LineRenderer>());
        }
    }
    
    public void animate(List<TrailController> trails){
        StartCoroutine(animateLines(trails));
    }

    private void placeLine(LineRenderer line, TrailController trail1, TrailController trail2){
        line.SetPosition(0, trail1.endpoints[Random.Range(0, 2)]);
        line.SetPosition(1, trail2.endpoints[Random.Range(0, 2)]);
        print("line start: " + trail1.endpoints[0] + " line end: " + trail2.endpoints[1]);
    }

    private IEnumerator animateLines(List<TrailController> trails){
        for(int i = 0; i < trails.Count; i++){
            print("placing line " + i);
            placeLine(lines[i], trails[i], trails[Mathf.FloorToInt(i+trails.Count/2) % trails.Count]);
        }
        yield return new WaitForSeconds(fadeDelay);
        foreach(LineRenderer line in lines){
            LeanTween.alpha(line.gameObject, 0, fadeTime);
            print("fading line");
        }
        yield return new WaitForSeconds(fadeTime);
        foreach(LineRenderer line in lines){
            line.startColor = lineStartColor;
            line.endColor = lineEndColor;
        }
    }

}
