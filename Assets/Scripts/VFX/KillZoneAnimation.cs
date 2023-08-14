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
    private float collapseTime = 1.5f;
    [SerializeField]
    private Color lineStartColor = new Color(0, 0, 0, 1);
    [SerializeField]
    private Color lineEndColor = new Color(0, 0, 0, 0);
    private List<int> animTweens = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numLines; i++){
            GameObject line = Instantiate(linePrefab, transform);
            line.SetActive(false);
            lines.Add(line.GetComponent<LineRenderer>());
        }
    }
    
    public void animate(List<TrailController> trails){
        //TODO: make this look good
        // StartCoroutine(createLines(trails));
        /* 
        Get the midpoint of all of the trails
        tween scale to 0
        tween position to midpoint
        */
        Vector2 midpoint = trailMidpoint(trails);

        foreach(TrailController trail in trails){
            //LeanTween.scale(trail.gameObject, Vector3.zero, fadeTime);
            trail.dead = true;
            LeanTween.value(trail.gameObject, trail.transform.localScale.x, 0, trail.trailCollapseAnimTime).setOnUpdate((float val) => {
                trail.transform.localScale = new Vector3(val, trail.transform.localScale.y, trail.transform.localScale.z);
            }).setEaseInOutSine();
            LeanTween.move(trail.gameObject, midpoint, trail.trailCollapseAnimTime).setEaseInOutSine();
        }

    }

    private Vector2 trailMidpoint(List<TrailController> trails){
        Vector2 midpoint = Vector2.zero;
        foreach(TrailController trail in trails){
            midpoint += (Vector2) trail.transform.position;
        }
        midpoint /= trails.Count;
        return midpoint;
    }

    

    // Can use this to add "bolts" representing the killzone. Descoped making it really look good for now
    private IEnumerator createLines(List<TrailController> trails){
        for(int i = 0; i < lines.Count; i++){
            print("placing line " + i);
            StartCoroutine(placeLine(lines[i], trails[i % trails.Count], trails[Mathf.FloorToInt(i+trails.Count/2) % trails.Count]));
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

    private IEnumerator placeLine(LineRenderer line, TrailController trail1, TrailController trail2){
        line.SetPosition(0, trail1.endpoints[Random.Range(0, 2)]);
        line.SetPosition(1, trail2.endpoints[Random.Range(0, 2)]);
        print("line start: " + trail1.endpoints[0] + " line end: " + trail2.endpoints[1]);
        line.gameObject.SetActive(true);
        LeanTween.alpha(line.gameObject, 0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        line.gameObject.SetActive(false);
    }

}
