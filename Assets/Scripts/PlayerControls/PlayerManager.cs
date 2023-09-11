using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool invulnerable = false;
    public float invulnerabilityTime = 3.0f;
    public float health; 
    public float maxHealth; 
    public float regenOnFlitKilled; 
    public bool disableSwipesOnKnock = false;
    public float disableSwipeTimeRatio = 0.5f;
    private GameManager gameManager;
    private PlayerMover mover;
    private TrailLeaver leaver;
    private TrailCollapser collapser;
    private PlayerSwiper swiper;
    private DamageNotes damageNotes;
    private DifficultyConstants constants;
    private bool damageNotesCoroutRunning = false;
    private SpriteRenderer rend;
    private ParticleSystem deathPs;
    private ParticleSystem idlePs;

    public bool dead;
    // Start is called before the first frame update

    private int ltidRegen;


    [SerializeField]
    private float knockBackRotMaxVal;
    [SerializeField]
    private float knockBackRotMinVal;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        constants = gameManager.GetComponent<DifficultyConstants>();
        maxHealth = constants.InitialHealth;
        regenOnFlitKilled = constants.RegenPerFlit;
        knockBackRotMaxVal = constants.enemyKnockbackRotDistMax;
        
        damageNotes = GameObject.FindGameObjectWithTag("AudioManager").GetComponentInChildren<DamageNotes>();
        mover = GetComponent<PlayerMover>();
        leaver = GetComponent<TrailLeaver>();
        collapser = GetComponent<TrailCollapser>();
        swiper = GetComponent<PlayerSwiper>();
        health = maxHealth;
        invulnerable = false;
        deathPs = transform.Find("PlayerDeathPS").GetComponentInChildren<ParticleSystem>();
        idlePs = transform.Find("PlayerIdlePS").GetComponentInChildren<ParticleSystem>();
        rend = GetComponentInChildren<SpriteRenderer>();
        dead = false;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P)) takeDamage(1);
    }

    void OnTriggerEnter2D(Collider2D other){
        collision(other);
    }
    void OnTriggerStay2D(Collider2D other){
        collision(other);

    }

    private void collision(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("3nemies")){
            FlitController flitController = other.gameObject.GetComponent<FlitController>();
            bool otherDead = flitController && flitController.toBePurged;
            if(!invulnerable && !otherDead && !dead){
                takeDamage(1);
                print("hp after: " + health);
            }
            if(!dead && !otherDead)
                enemyCollision(dead, other.gameObject);

        } 
        if(other.gameObject.layer == LayerMask.NameToLayer("Walls")){
            hitWall(other.gameObject);
        }
    }

    public void takeDamage(float amnt)
    {
        if (ltidRegen != 0 && LeanTween.isTweening(ltidRegen) && constants.stopRegenOnDamage)
        {
            LeanTween.cancel(ltidRegen);
        }
        health -= amnt;
        if(health <= 0 && !dead){
            mover.stopPlayer();
            swiper.swipeEnabled = false;
            collapser.killAllTrails();
            gameManager.playerKilled();
            rend.enabled = false;
            deathPs.transform.parent = null;
            deathPs.Play();
            if(idlePs) idlePs.gameObject.SetActive(false);
            dead = true;
        }
    }
    public void startRegen(int killCount)
    {
        float targetHealth = Mathf.Clamp(health + (regenOnFlitKilled * killCount), 0, maxHealth);
        ltidRegen = LeanTween.value(health, targetHealth, constants.regenTime).setOnUpdate(regenUpdate).id;
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
        float rot = getKnockBackRot(gameObject.transform.eulerAngles.z);
        mover.movePlayer(wallToPlayerDir, wallCont.knockBack, 
            wallCont.knockMovTime, wallCont.knockRotTime,rot);
        if(disableSwipesOnKnock){
            swiper.swipeEnabled = false;
            StopCoroutine("swipeReenableCorout");
            StartCoroutine("swipeReenableCorout",Mathf.Max(wallCont.knockMovTime, wallCont.knockRotTime) * disableSwipeTimeRatio);
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


    public void enemyCollision(bool dead, GameObject enemy){
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
        if(!dead)
            mover.movePlayer(enemyToPlayerDir, knockInfo[0], 
                knockInfo[1], knockInfo[2],rot);
        
        if(disableSwipesOnKnock){
            swiper.swipeEnabled = false;
            StopCoroutine("swipeReenableCorout");
            StartCoroutine("swipeReenableCorout",Mathf.Max(knockInfo[1], knockInfo[2]) * disableSwipeTimeRatio);
        }

        invulnerable = true;
        if(!damageNotesCoroutRunning)
            StartCoroutine("damageNotesCorout");
        //make sure there's no runing instance of the corout
        StopCoroutine("invulnerableCorout");
        StartCoroutine("invulnerableCorout");
    }
    private IEnumerator damageNotesCorout(){
        damageNotesCoroutRunning = true;
        damageNotes.playDamageChord();
        yield return new WaitForSeconds(damageNotes.chordPlayTime);
        damageNotesCoroutRunning = false;


    }

    private IEnumerator invulnerableCorout(){
        yield return new WaitForSeconds(invulnerabilityTime);
        invulnerable = false;
        print("invulnerability stopped");
        yield return null;
    }
    private IEnumerator swipeReenableCorout(float disableTime){
        yield return new WaitForSeconds(disableTime);
        swiper.swipeEnabled = true;
        yield return null;
    }
}
