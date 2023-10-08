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
    private List<Material> imageMats = new List<Material>();
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private List<GameObject> all = new List<GameObject>();

    [SerializeField]
    private float maxButtonMatAlpha = 28;
    public float displayTime = 0.7f;
    public float initAlpha = 0;
    public float spriteEndAlpha = 0.4f;
    public bool initActive = false;
    private TextGroupAlphaControls textGroup;

    private string[] buttonShaders = new string[] { "Beset/UIButtons" };

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
            Image img = obj.GetComponent<Image>();
            if(!img) img = obj.GetComponentInChildren<Image>();
            if(img) {
                print("found image " + img.gameObject.name + " with shader " + img.material.shader.name);
                if(!buttonImgs.Contains(img) && buttonShaders.Contains(img.material.shader.name)) {
                    print("adding img with shader: " + img.material.shader + " to list");
                    buttonImgs.Add(img);
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
            obj.SetActive(true);
        }
        if(textGroup) textGroup.displayAll();
        tweenButtonsAlphaTo(maxButtonMatAlpha, displayTime);
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
        tweenButtonsAlphaTo(0, displayTime);
        tweenSpriteAlphaTo(0, displayTime);
        tweenImageAlphaTo(0, displayTime);
        if(textGroup) textGroup.hideAll();
        setAllUnClickable(); //don't need to be clickable as they're fading out
        Invoke("deactivateAll", displayTime);
    }

    public void tweenButtonsAlphaTo(float to, float time)
    {
        foreach (Image img in buttonImgs)
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
