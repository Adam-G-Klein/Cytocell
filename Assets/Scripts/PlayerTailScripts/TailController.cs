using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A prototype for a procedurally animated chain of cells that
// would follow the player around, as an indication of how many
// red cells had been consumed
public class TailController : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> nodeLst;

    [SerializeField]
    public float nodeDist;

    [SerializeField]
    public float nodeMovTime;

    private List<Vector2> currPosLst = new List<Vector2>();
    private List<Vector2> targPosLst= new List<Vector2>();
    //equal to {targPosLst + currPosLst}
    private List<Vector2> targAndCurrLst= new List<Vector2>();

    private List<TailNodeController> nodeControllerLst = new List<TailNodeController>();

    //the position in the list {targPosLst + currPosLst} 
    //that the rootNode is at. if != 0, tail is moving
    private int rootNodePos = 0;


    private Vector2 oldPlayerLoc;
    private GameObject player;
    private int numNodes;

    //need to have each node follow the previous one, step through prev loc lst on every move
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject node in nodeLst){
            currPosLst.Add(node.transform.position);
            nodeControllerLst.Add(node.GetComponent<TailNodeController>());
        }
        player = GameObject.FindGameObjectWithTag("Player");
        oldPlayerLoc = player.transform.position;
        numNodes = nodeLst.Count;

    }


    public void move(Vector2 newPlayerLoc){

        //has to be done at end
        oldPlayerLoc = newPlayerLoc;
    }
    private void updateCurrPosLst(){
        currPosLst.Clear();
        foreach(GameObject node in nodeLst){
            currPosLst.Add(node.transform.position);
        }
    }
    private List<Vector2> listCombine(List<Vector2> lst1, List<Vector2> lst2 ){
        List<Vector2> retLst = new List<Vector2>();

        foreach(Vector2 vect in lst1){
            retLst.Add(vect);
        }
        foreach(Vector2 vect in lst2){
            retLst.Add(vect);
        }
        return retLst;
    }

    //should incorporate some local x and y axis pos randomization at some point
    private List<Vector2> genNewPositions(Vector2 newPlayerLoc){
        float angleDif = Vector2.Angle(Vector2.right, newPlayerLoc);
        print("genPos:");
        print("\tangle found: " + angleDif.ToString());
        //start at 1 to work with placement algorithm
        for(int i = 1; i < numNodes + 1; i++){
            //tan(theta) = y/x; y = xtan(theta)
            float hypotenuse = (i * nodeDist);
            float xDiff = hypotenuse*Mathf.Cos(Mathf.Deg2Rad*angleDif);
            float yDiff = hypotenuse*Mathf.Sin(Mathf.Deg2Rad * angleDif);
            Vector2 newPos = new Vector2(xDiff + newPlayerLoc.x, yDiff + newPlayerLoc.y);

            targPosLst.Add(newPos);
        }
        print("\ttargLst: " );
        pList(targPosLst);
        return targPosLst;
    }
    private void pList(List<Vector2> lst){
        foreach(Vector2 v in lst){
            print(string.Format("\t\t" + v.ToString()));
        }
    }

    private void shiftNodes(){
        if(rootNodePos < 0){
            print("somethings wrong, shift nodes called too many times");
            print("this should only be called with rootNodePos >= 1");
            Debug.LogError("sorry i gotta do this");
            return;
        }
        //i is index into nodeControllerLst
        print("nodeShift:");
        print("\ttargLst: " );
        pList(targAndCurrLst);
        for(int i = 0; i < numNodes; i++){
            //idx is index into targAnndPosLst
            int idx = rootNodePos + i;
            nodeControllerLst[i].move(targAndCurrLst[idx], nodeMovTime);
            print(string.Format("\tmoved node {0} to pos {1}", i, idx));
        }
    }

    private IEnumerator moveCorout(){
        //set the rootNodePos to be the index in targAndCurPos
        // that will be the first position in the currPos list
        rootNodePos = numNodes;
        while(rootNodePos > 0){
            rootNodePos -= 1;
            shiftNodes();
            yield return new WaitForSeconds(nodeMovTime);
        }
    }
}
