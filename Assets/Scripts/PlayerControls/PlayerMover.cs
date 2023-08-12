using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Provides functions to different systems to allow consistent
// and centralized player movement
// Called most frequently by PlayerSwiper and PlayerManager
public class PlayerMover : MonoBehaviour
{

    private GameObject player;
    private GameObject cam;
    private PlayerSwiper swiper;
    private PlayerManager playManager;
    private SwipeNotes sNotes;
    private JellyMotionScaler motScale;
    [SerializeField]
    private TailSetController playerTailSet;
    private bool moving;
    public int ltidMov = 0;
    public int ltidCam = 0;
    public int ltidRot = 0;   

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        sNotes = GameObject.FindGameObjectWithTag("AudioManager").GetComponentInChildren<SwipeNotes>();
        swiper = GetComponent<PlayerSwiper>();
        motScale = GetComponent<JellyMotionScaler>();

    }
    //uses mov dir to find angle
    public int[] movePlayer(Vector2 movDir, float movDist,
      float movTime, float rotTime){
            /*returns an int[3] array with all the new ltid's in it
            0: ltidMov
            1: ltidRot
            2: ltidCam */
        float targAngle = FindAngle(movDir);
        Vector2 movTarg = (movDir.normalized * movDist) + (Vector2)player.transform.position;
        motScale.move(movTime, movDist);
        sNotes.swipeSound();


        LeanTween.cancel(ltidMov);
        LeanTween.cancel(ltidRot);
        LeanTween.cancel(ltidCam);
        //set the public ltids
        ltidMov = LeanTween.move(player.gameObject, movTarg, movTime)
                    .setEaseOutSine().id;
        ltidCam = moveCamera(cam, (Vector3)movTarg, ltidCam, movTime); 
        ltidRot = LeanTween.rotate(player.gameObject,
                    new Vector3(player.transform.rotation.x, player.transform.rotation.y, targAngle), rotTime)
                    .setEaseInQuad().setEaseOutSine().id;
        return new int[] {ltidMov,ltidRot,ltidCam};
    }

    public void stopPlayer() {
        LeanTween.cancel(ltidMov);
        LeanTween.cancel(ltidRot);
        LeanTween.cancel(ltidCam);
    }
    //takes explicit angle, used for movement after collisions
    public int[] movePlayer(Vector2 movDir, float movDist, 
      float movTime, float rotTime, float targAngle){
            /*returns an int[3] array with all the new ltid's in it
            0: ltidMov
            1: ltidRot
            2: ltidCam */
        Vector2 movTarg = (movDir.normalized * movDist) + (Vector2)player.transform.position;

        LeanTween.cancel(ltidMov);
        LeanTween.cancel(ltidRot);
        LeanTween.cancel(ltidCam);
        //set the public ltids
        ltidMov = LeanTween.move(player.gameObject, movTarg, movTime)
                    .setEaseInOutSine().setEaseInOutSine().id;
        ltidCam = moveCamera(cam, (Vector3)movTarg, ltidCam, movTime); 
        ltidRot = LeanTween.rotate(player.gameObject,
                    new Vector3(player.transform.rotation.x, player.transform.rotation.y, targAngle), rotTime)
                    .setEaseInQuad().setEaseOutSine().id;
        return new int[] {ltidMov,ltidRot,ltidCam};
    }
    public int moveCamera(GameObject cam, Vector3 movTarg, int ltidCam, float movTime){
        /*hopefully we can do more fancy camera movement soon */
        ltidCam = LeanTween.move(cam, new Vector3(movTarg.x,movTarg.y, cam.transform.position.z), movTime)
                    .setEaseOutSine().id;
        return ltidCam;
    }
    
    public float FindAngle(Vector2 dir)
    {
        ///    quadrants (for easier math):
        ///    1 | 0
        /// -----|-----
        ///    2 | 3
        if (dir.x == 0)
        {
            if (dir.y >= 0) return 0;
            else return 180;
        }
        int quadrant;
        if (dir.x > 0)
            quadrant = dir.y > 0 ? 0 : 3;
        else
            quadrant = dir.y > 0 ? 1 : 2;
        float inQuadAngle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
        float simpleAngle = quadrant == 0 || quadrant == 3 ? inQuadAngle + 270 : inQuadAngle + 90;
        return simpleAngle;


    }


    public bool isPlayerMoving(){

        return LeanTween.isTweening(ltidMov);
    }

}
