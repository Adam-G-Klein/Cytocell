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
    private string[] playerSkinShaders = new string[] {"Beset/CellAlphaUI"};

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
        SendMessageUpwards("initDone", null, SendMessageOptions.DontRequireReceiver);
    }

    private void addAllItemsToLists(List<GameObject> gos)
    {
        foreach (GameObject obj in gos)
        {
            print("checking go: " + obj.name);
            Image img = obj.GetComponent<Image>();
            if(!img) img = obj.GetComponentInChildren<Image>();
            if(img) {
                print("found image " + img.gameObject.name + " with shader " + img.material.shader.name);
                if(!buttonImgs.Contains(img) && buttonShaders.Contains(img.material.shader.name)) {
                    print("adding img with shader: " + img.material.shader + " to button list");
                    buttonImgs.Add(img);
                }
                print("player skin shaders: " + playerSkinShaders + " contains: " + img.material.shader.name + " ? " + playerSkinShaders.Contains(img.material.shader.name) + " player imgs contains: " + playerImgs.Contains(img));
                if(!playerImgs.Contains(img) && playerSkinShaders.Contains(img.material.shader.name)) {
                    print("adding img with shader: " + img.material.shader + " to skin list");
                    playerImgs.Add(img);
                    print("player images:");
                    foreach(Image i in playerImgs) {
                        print("img: " + i.gameObject.name + " shader: " + i.material.shader.name);
                    }

                }
                else {
                    print("adding img " + img.gameObject.name + " to list");
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
        foreach (GameObject obj in all)
        {
            print("btn group set active: " + obj.name);
            obj.SetActive(true);
        }
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
        tweenCellAlphasTo(buttonImgs, 0, displayTime);
        tweenCellAlphasTo(playerImgs, 0, displayTime);
        tweenSpriteAlphaTo(0, displayTime);
        tweenImageAlphaTo(0, displayTime);
        if(textGroup && textGroupBehavesTheSame) textGroup.hideAll();
        setAllUnClickable(); //don't need to be clickable as they're fading out
        Invoke("deactivateAll", displayTime);
    }

    public void tweenCellAlphasTo(List<Image> imgs, float to, float time)
    {
        foreach (Image img in imgs)
        {
            LeanTween.value(
            gameObject, img.material.GetFloat("_publicAlpha"), to, time)
            .setOnUpdate((float val) =>
            {
                img.material.SetFloat("_publicAlpha", val);
            });
        }
    }

    public void tweenSpriteAlphaTo(float to, float time){
        foreach (SpriteRenderer renderer in sprites)
        {
            LeanTween.value(
            gameObject, renderer.color.a, to, time)
            .setOnUpdate((float val) =>
            {
                renderer.color = new Color(renderer.color.r, renderer.color.b, renderer.color.g, val);
            });

        }
    }

    public void tweenImageAlphaTo(float to, float time)
    {
        foreach (Material mat in imageMats)
        {

            LeanTween.value(
            gameObject, mat.color.a, to, time)
            .setOnUpdate((float val) =>
            {
                mat.color = new Color(mat.color.r, mat.color.b, mat.color.g, val);
            });
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
