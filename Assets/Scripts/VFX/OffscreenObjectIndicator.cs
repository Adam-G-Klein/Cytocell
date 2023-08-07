using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenObject {
    public Vector2 dir;
    public float dist;
    public GameObject obj;

    public OffscreenObject(Vector2 dir, float dist, GameObject obj)
    {
        this.dir = dir;
        this.dist = dist;
        this.obj = obj;
    }
}

public class OffscreenObjectIndicator : MonoBehaviour
{

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        #region debug
        /*
        foreach(OffscreenObject obj in getOffscreenObjects())
        {
            print("offscreen object: " + obj.obj.name + " at " + obj.dir + " distance: " + obj.dist);
        }
        */
        #endregion
        
    }

    // TODO: make objects this class finds interfaces
    private List<OffscreenObject> getOffscreenObjects()
    {
        List<GameObject> allObjs = new List<GameObject>();
        List<OffscreenObject> offscreenObjects = new List<OffscreenObject>();
        allObjs = gameManager.flits.ConvertAll<GameObject>(x => x.gameObject);
        Vector2 cameraPos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        foreach (GameObject obj in allObjs)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(obj.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if (!onScreen)
            {
                offscreenObjects.Add(new OffscreenObject(screenPoint, 
                    Vector2.Distance(cameraPos, new Vector2(obj.transform.position.x, obj.transform.position.y)), obj));
            }
        }
        return offscreenObjects;
    }



}
