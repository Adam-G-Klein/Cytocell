using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRecycler : MonoBehaviour {

    public Queue<GameObject> trails = new Queue<GameObject>();
    public Queue<GameObject> flits = new Queue<GameObject>();
    public Queue<GameObject> particleSystems = new Queue<GameObject>();
    public GameObject trail;
    public GameObject flit;
    public GameObject psTemplate;
    public GameObject killZone; 

    [SerializeField]
    public float flitRecylceWaitTime;
    [SerializeField]
    public float psRecycleWaitTime;

    public GameManager manager;
    [SerializeField]
    private string flitName = "Flit";
    [SerializeField]
    private int flitNameCount = 4;

    void Start(){
    }

    T FindComponent<T>() where T : class
    {
        T res = gameObject.GetComponentInChildren<T>();
        if (res != null && !res.Equals(null))
            return res;
        return gameObject.GetComponentInParent<T>();
    }

    public GameObject RecyclePS(Vector3 position, Quaternion rotation){
        GameObject ps;
        if (particleSystems.Count < 1){
            ps = Instantiate(psTemplate,position,rotation);
        }
        else{
            ps = particleSystems.Dequeue().gameObject;
        }
        Transform tran = ps.transform;
        tran.position = position;
        tran.rotation = rotation;
        tran.gameObject.SetActive(true);
        return tran.gameObject;

    }
    public void dumpPS(GameObject dumpedPS){
        StartCoroutine("psRecycleWait", dumpedPS);
    }
    public GameObject RecycleTrail(Vector3 position, Quaternion rotation)
    {
        /**/
        if (trails.Count < 1)
        {
            GameObject newTrail = Instantiate(trail, position, rotation);
            //print("returned a new trail");
            return newTrail;
        }
        //print("len of trails: " + trails.Count);
        Transform tran = trails.Dequeue().transform;
        tran.position = position;
        tran.rotation = rotation;
        tran.gameObject.SetActive(true);
        return tran.gameObject;

    }

    public GameObject RecycleFlit(Vector3 position, Quaternion rotation)
    {
        //Debug.Log("entered recycleFlit");
        if (flits.Count < 1)
        {
            //Debug.Log("got past flit count check");
            GameObject newFlit = Instantiate(flit, position, rotation);
            //Debug.Log("got past flit instantiate");
            newFlit.name = flitName + flitNameCount;
            flitNameCount += 1;
            /*
            Debug.Log("manager is null: " + manager == null);
            Debug.Log("trying to pring manager: ");
            Debug.Log("manager is: " + manager.ToString());
            */
            manager.flitCreated(newFlit.GetComponent<FlitController>());
            return newFlit;
        }
        else
        {
            //print("len of trails: " + trails.Count);
            Transform tran = flits.Dequeue().transform;
            tran.position = position;
            tran.rotation = rotation;
            tran.gameObject.SetActive(true);
            tran.GetComponent<FlitController>().respawnReset();
            manager.flitCreated(tran.GetComponent<FlitController>());
            return tran.gameObject;
        }
    }
    public void dumpFlit(FlitController flit){
        if(!(gameObject == null))
            StartCoroutine("flitRecycleWait", flit);
    }
    private IEnumerator flitRecycleWait(FlitController flit){
        yield return new WaitForSeconds(flitRecylceWaitTime);
        if(!(gameObject == null))
            flits.Enqueue(flit.gameObject);
    }
    private IEnumerator psRecycleWait(GameObject ps){
        yield return new WaitForSeconds(psRecycleWaitTime);
        if(!(gameObject == null)){
            foreach(ParticleSystem p in ps.GetComponentsInChildren<ParticleSystem>()){
                p.Clear();
            }
            ps.SetActive(false);
            particleSystems.Enqueue(ps);
        }
    }

}
