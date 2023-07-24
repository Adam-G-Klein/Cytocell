using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerMover mover;
    private bool swimset;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponentInChildren<Animator>();
        mover = GetComponent<PlayerMover>();
        swimset = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        
        //anim.SetBool("moving",mover.isPlayerMoving());
        #region animtest
       /*  
        if(swimset){
            anim.SetBool("swimPressed", false);
            swimset=false;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            anim.SetBool("swimPressed", true);
            swimset = true;
            anim.SetBool("moving", true);
        }
        if(Input.GetKeyDown(KeyCode.M)){
            anim.SetBool("moving", false);
            anim.SetBool("swimPressed",false);
        }
        */
        #endregion
    }
    //from when the player had an animator 
    /*public void playerSwim(){
        anim.SetBool("swimPressed",true);
        StartCoroutine("resetSwim");
    }
    IEnumerator resetSwim(){
        yield return new WaitForEndOfFrame();
        anim.SetBool("swimPressed",false);
        

    }*/
}
