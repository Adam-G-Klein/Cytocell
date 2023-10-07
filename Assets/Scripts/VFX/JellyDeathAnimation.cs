using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class JellyDeathAnimation : MonoBehaviour
{

    [SerializeField]
    private float longestDeathParticleLifetime;
    [SerializeField]
    private float minDeathAnimationTime = 0.3f;
    [SerializeField]
    private float maxDeathAnimationTime = 0.7f;

    private float deathAnimationTime{
        get{
            return Random.Range(minDeathAnimationTime, maxDeathAnimationTime);
        }
    }
    [SerializeField]
    private float deathAnimationPopAlpha = 0.2f;
    [SerializeField]
    private float deathAnimationPopScale = 1.5f;

    [SerializeField]
    private Color deathAnimationFinalColor = Color.white;
    private Color locMembraneColor = Color.red;

    private float defaultShaderAlpha = 1;
    [SerializeField]
    private float defaultScale = 0.5f;
    [SerializeField]
    private Color defaultMembraneColor = new Color();
    private float locShaderAlpha = 1;
    private float shaderAlpha{
        get{return locShaderAlpha;}
        set{
            locShaderAlpha = value;
            mat.SetFloat("_publicAlpha", locShaderAlpha);
        }
    }
    private float membraneThickness{
        get{return locMembraneThickness;}
        set{
            locMembraneThickness = value;
            mat.SetFloat("_membraneThickness", locMembraneThickness);
        }
    }
    private Color membraneColor{
        get{return locMembraneColor;}
        set{
            locMembraneColor = value;
            mat.SetColor("_membraneColor", locMembraneColor);
        }
    }
    private Material mat;
    private GameManager manager;
    private FlitController fController;
    private NucleusMovement nucleusMovement;
    private ObjectRecycler recycler;
    private float locMembraneThickness = 0.8f;
    [SerializeField]
    private float endMembraneThickness = 0.1f;
    [SerializeField]
    private float defaultMembraneThickness = 0.8f;
    [SerializeField]
    private GameObject ps;
    // Start is called before the first frame update
    private int SPRITE_CHILD_INDEX = 0;

    private DeathNotes dnotes;
    void Start()
    {

        fController = GetComponent<FlitController>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mat = transform.GetChild(SPRITE_CHILD_INDEX).GetComponent<SpriteRenderer>().material;
        nucleusMovement = GetComponent<NucleusMovement>();
        recycler = manager.GetComponent<ObjectRecycler>();
        defaultMembraneColor = mat.GetColor("_membraneColor");
        defaultShaderAlpha = mat.GetFloat("_publicAlpha");
        if(!manager.menuMode)
            dnotes = GameObject.Find("Audio").GetComponentInChildren<DeathNotes>();
        
    }
    public delegate void deathDelegate();
    
    public void resetForRespawn(){
        nucleusMovement.enabled = true;
        membraneColor = defaultMembraneColor;
        shaderAlpha = defaultShaderAlpha;
        transform.localScale = new Vector2(defaultScale,defaultScale);//cuz its a circle
    }


    public void playDeathAnimation(deathDelegate doneCallBack, bool particles = true, bool note = true){

        if (gameObject.activeSelf)
            StartCoroutine(deathAnimation(doneCallBack, particles, note));
        else
        {
            doneCallBack();
        }
    }
    private IEnumerator deathAnimation(deathDelegate deathCallback, bool particles = true, bool note = true){
        nucleusMovement.enabled = false;
        int ltidAlpha = LeanTween.value(gameObject, updateAlpha, shaderAlpha, deathAnimationPopAlpha, deathAnimationTime).id;
        int ltidScale = LeanTween.value(gameObject, updateScale, transform.localScale.x, deathAnimationPopScale, deathAnimationTime).id;
        int ltidThickness = LeanTween.value(gameObject, updateThickness, membraneThickness, endMembraneThickness, deathAnimationTime).id;
        //all this mess because I don't want to get the spriteRenderer's color into my shader,
        // which is the way that LeanTween applies color tweening
        int ltidR = LeanTween.value(gameObject, updateR, 1, deathAnimationFinalColor.r, deathAnimationTime).id;
        int ltidG = LeanTween.value(gameObject, updateG, 1, deathAnimationFinalColor.g, deathAnimationTime).id;
        int ltidB = LeanTween.value(gameObject, updateB, 1, deathAnimationFinalColor.b, deathAnimationTime).id;
        yield return new WaitForSeconds(deathAnimationTime);
        if(note) dnotes.playNote(dnotes.currNote);
        //ps = recycler.RecyclePS(transform.position, transform.rotation);
        if(particles) Instantiate(ps, transform.position, transform.rotation);
        deathCallback();

    }
    private void updateScale(float value){
        transform.localScale = new Vector2(value, value);
    }

    private void updateThickness(float value){
        membraneThickness = value;
    }
    private void updateAlpha(float value){
        shaderAlpha = value;
    }
    private void updateR(float value){
        membraneColor = new Color(value, membraneColor.g, membraneColor.b,shaderAlpha);
    }
    private void updateG(float value){
        membraneColor = new Color(membraneColor.r , value , membraneColor.b,shaderAlpha);
    }
    private void updateB(float value){
        membraneColor = new Color(membraneColor.r, membraneColor.g, value,shaderAlpha);
    }
}
