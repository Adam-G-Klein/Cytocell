using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool invulnerable = false;
    public float invulnerabilityTime = 3.0f;
    // Start is called before the first frame update
    //initial value will be set in editor 
    public float health; 
    public float maxHealth; 
    public bool disableSwipesOnKnock = false;
    public float disableSwipeTimeRatio = 0.5f;
    private GameManager gameManager;
    private PlayerMover mover;
    private TrailLeaver leaver;
    private TrailCollapser collapser;
    private PlayerSwiper swiper;
    private PlayerAnimation anim;
    // Start is called before the first frame update

    private int ltidRegen;


    [SerializeField]
    private float knockBackRotMaxVal;
    [SerializeField]
    private float knockBackRotMinVal;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mover = GetComponent<PlayerMover>();
        leaver = GetComponent<TrailLeaver>();
        collapser = GetComponent<TrailCollapser>();
        swiper = GetComponent<PlayerSwiper>();
        anim = GetComponent<PlayerAnimation>();
        health = maxHealth;
        invulnerable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {

            takeDamage(1);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other){
        collision(other);
    }
    void OnTriggerStay2D(Collider2D other){
        collision(other);

    }

    private void collision(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("3nemies")){
            print(string.Format("health before: {0} invuln? {1}", health, invulnerable));
            if(!invulnerable){
                takeDamage(1);
                
            }
            print(string.Format("health after: {0} invuln? {1}", health, invulnerable));
            enemyCollision(other.gameObject);

        } 
        if(other.gameObject.layer == LayerMask.NameToLayer("Walls")){
            hitWall(other.gameObject);
        }
    }

    public void takeDamage(float amnt)
    {
        if (ltidRegen != 0 && LeanTween.isTweening(ltidRegen))
        {
            LeanTween.cancel(ltidRegen);
        }
        health -= amnt;
        if(health == 0){
            gameManager.playerKilled();
        }
    }
    public void startRegen(float regenTime)
    {
        ltidRegen = LeanTween.value(health, maxHealth, regenTime).setOnUpdate(regenUpdate).id;
    }
    private void regenUpdate(float newRegen)
    {
        health = newRegen;

    }



    private void hitWall(GameObject wall){
        WallController wallCont = wall.GetComponent<WallController>();
        Vector3 playerPos = gameObject.transform.position;
        Vector3 wallPos = wall.transform.position;
        Vector2 wallToPlayerDir = 
            new Vector2(playerPos.x - wallPos.x, playerPos.y - wallPos.y);
        collapser.killAllTrails();
        leaver.currentTrailPlaced();
        //stop
        float rot = getKnockBackRot(gameObject.transform.eulerAngles.z);
        mover.movePlayer(wallToPlayerDir, wallCont.knockBack, 
            wallCont.knockMovTime, wallCont.knockRotTime,rot);
        if(disableSwipesOnKnock){
            swiper.swipeEnabled = false;
            StopCoroutine("swipeDisableCorout");
            StartCoroutine("swipeDisableCorout",Mathf.Max(wallCont.knockMovTime, wallCont.knockRotTime) * disableSwipeTimeRatio);
        }

    }
    private float getKnockBackRot(float currRot){
        float rand = Random.Range(knockBackRotMinVal, knockBackRotMaxVal);
        //left = 0, right = 1
        float leftOrRight = Random.Range(0,1);
        if(leftOrRight == 0)
            return currRot + rand;
        //basically: if(leftOrRight == 1)
        return currRot - rand;



    }


    public void enemyCollision(GameObject enemy){
        FlitController enemyCont = enemy.GetComponent<FlitController>();
        Vector3 playerPos = gameObject.transform.position;
        Vector3 enemyPos = enemy.transform.position;
        Vector2 enemyToPlayerDir = 
            new Vector2(playerPos.x - enemyPos.x, playerPos.y - enemyPos.y);
        collapser.killAllTrails();
        leaver.currentTrailPlaced();
        float rot = getKnockBackRot(gameObject.transform.eulerAngles.z);
        float[] knockInfo = enemyCont.getKnockBackInfo();
        /*get info on knockback
        0: knockBackDist
        1: knockMovTime
        2: knockRotTime*/
        mover.movePlayer(enemyToPlayerDir, knockInfo[0], 
            knockInfo[1], knockInfo[2],rot);
        
        if(disableSwipesOnKnock){
            swiper.swipeEnabled = false;
            StopCoroutine("swipeDisableCorout");
            StartCoroutine("swipeDisableCorout",Mathf.Max(knockInfo[1], knockInfo[2]) * disableSwipeTimeRatio);
        }

       invulnerable = true;
        //make sure there's no runing instance of the corout
        StopCoroutine("invulnerableCorout");
        StartCoroutine("invulnerableCorout");
    }

    private IEnumerator invulnerableCorout(){
        yield return new WaitForSeconds(invulnerabilityTime);
        invulnerable = false;
        print("invulnerability stopped");
        yield return null;
    }
    private IEnumerator swipeDisableCorout(float disableTime){
        yield return new WaitForSeconds(disableTime);
        swiper.swipeEnabled = true;
        yield return null;
    }
}
