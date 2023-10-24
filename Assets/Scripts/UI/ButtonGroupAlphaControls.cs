using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// A resuable interface that manipulates the game's custom gradient buttons
// along with the text and interactable elements in order to create
// a nested menu system in some places, and a simple code surface
// for ui manipulation in others.
public class ButtonGroupAlphaControls : MonoBehaviour
{
    // Should use this for all new menu items
    public List<GameObject> gameObjects;
    // Continuing to support separating items by images and buttons
    // because I don't want to recreate all of my current menus
    // by changing the serialization 
    public List<GameObject> buttons;
    private List<Clickable> clickables = new List<Clickable>();
    public List<GameObject> images;
    private List<Image> buttonImgs = new List<Image>();
    private List<Image> playerImgs = new List<Image>();
    private List<Material> imageMats = new List<Material>();
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private List<GameObject> all = new List<GameObject>();

    [SerializeField]
    private float maxButtonMatAlpha = 28;
    public float displayTime = 0.7f;
    public float initAlpha = 0;
    public float spriteEndAlpha = 0.4f;
    public float playerEndAlpha = 1f;
    public bool initActive = false;
    private TextGroupAlphaControls textGroup;
    [SerializeField]
    private bool textGroupBehavesTheSame = true;

    private string[] buttonShaders = new string[] { "Beset/UIButtons"};
    private string[] playerSkinShaders = new string[] {"Beset/CellAlphaUI", "Beset/JellyAlphaUI"};

    private List<int> currentLtids = new List<int>();
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        textGroup = GetComponent<TextGroupAlphaControls>();

        addAllItemsToLists(buttons);
        addAllItemsToLists(images);
        addAllItemsToLists(gameObjects);

        foreach (Image img in buttonImgs)
        {
            img.material.SetFloat("_publicAlpha", initAlpha);
        }
        foreach (Image img in playerImgs) {
            img.material.SetFloat("_publicAlpha", initAlpha);
        }
        SendMessageUpwards("initDone", null, SendMessageOptions.DontRequireReceiver);
        isActive = initActive;
    }

    private void addAllItemsToLists(List<GameObject> gos)
    {
        foreach (GameObject obj in gos)
        {
            Image img = obj.GetComponent<Image>();
            if(!img) img = obj.GetComponentInChildren<Image>();
            if(img) {
                if(!buttonImgs.Contains(img) && buttonShaders.Contains(img.material.shader.name)) {
                    buttonImgs.Add(img);
                }
                if(!playerImgs.Contains(img) && playerSkinShaders.Contains(img.material.shader.name)) {
                    playerImgs.Add(img);

                }
                else {
                    imageMats.Add(img.material);
                }

            } 
            Clickable clickable = obj.GetComponent<Clickable>();
            if(clickable) {
                clickable.clickable = initActive;
                if(!clickables.Contains(clickable)) clickables.Add(clickable);
            }
            SpriteRenderer renderer = obj.GetComponentInChildren<SpriteRenderer>();
            if (renderer && !sprites.Contains(renderer)) sprites.Add(renderer);
            all.Add(obj);
            obj.SetActive(initActive);
        }
    }

    public void displayAll()
    {
        isActive = true;
        foreach (GameObject obj in all)
        {
            obj.SetActive(true);
        }
        clearCurrentTweens();
        if(textGroup && textGroupBehavesTheSame) textGroup.displayAll();
        tweenCellAlphasTo(buttonImgs, maxButtonMatAlpha, displayTime);
        tweenCellAlphasTo(playerImgs, playerEndAlpha, displayTime);
        tweenSpriteAlphaTo(spriteEndAlpha, displayTime);
        tweenImageAlphaTo(spriteEndAlpha, displayTime);
        Invoke("setAllClickable", displayTime);
    }

    //this grossness because invoke can't take an argument 
    // and I didn't want to start another tween
    // TODO: a tween instead of a coroutine, or a custom object to pass
    private void setAllClickable()
    {
        foreach (Clickable c in clickables)
        {
            if(c) c.clickable = true;
        }
    }
    private void setAllUnClickable()
    {
        foreach (Clickable c in clickables)
        {
            if(c) c.clickable = false;
        }
    }
    public void hideAll()
    {
        isActive = false;
        clearCurrentTweens();
        tweenCellAlphasTo(buttonImgs, 0, displayTime);
        tweenCellAlphasTo(playerImgs, 0, displayTime);
        tweenSpriteAlphaTo(0, displayTime);
        tweenImageAlphaTo(0, displayTime);
        if(textGroup && textGroupBehavesTheSame) textGroup.hideAll();
        setAllUnClickable(); //don't need to be clickable as they're fading out
        Invoke("deactivateAll", displayTime);
    }

    private void clearCurrentTweens() {
        CancelInvoke("deactivateAll");
        CancelInvoke("setAllClickable");
        foreach (int ltId in currentLtids)
        {
            LeanTween.cancel(ltId);
        }
        currentLtids.Clear();
    }

    public void tweenCellAlphasTo(List<Image> imgs, float to, float time)
    {
        foreach (Image img in imgs)
        {
            currentLtids.Add(LeanTween.value(
            gameObject, img.material.GetFloat("_publicAlpha"), to, time)
            .setOnUpdate((float val) =>
            {
                img.material.SetFloat("_publicAlpha", val);
            }).id);
        }
    }

    public void tweenSpriteAlphaTo(float to, float time){
        foreach (SpriteRenderer renderer in sprites)
        {
            currentLtids.Add(LeanTween.value(
            gameObject, renderer.color.a, to, time)
            .setOnUpdate((float val) =>
            {
                renderer.color = new Color(renderer.color.r, renderer.color.b, renderer.color.g, val);
            }).id);

        }
    }

    public void tweenImageAlphaTo(float to, float time)
    {
        foreach (Material mat in imageMats)
        {

            currentLtids.Add(LeanTween.value(
            gameObject, mat.color.a, to, time)
            .setOnUpdate((float val) =>
            {
                mat.color = new Color(mat.color.r, mat.color.b, mat.color.g, val);
            }).id);
        }
    }
    private void deactivateAll()
    {

        foreach (GameObject obj in all)
        {
            obj.SetActive(false);
        }
    }

}
