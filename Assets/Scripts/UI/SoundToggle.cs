using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SoundToggle : MonoBehaviour
{
    [SerializeField]
    private Sprite enabledImage;
    [SerializeField]
    private Sprite disabledImage;
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject soundImage;
    private Image img;

    // Start is called before the first frame update
    void Awake()
    {
        
        img = soundImage.GetComponent<Image>();
        setTextForSoundState();
        AudioListener.volume = soundEnabled() ? 1 : 0;
    }

    public void click(){
        setSoundEnabled(!soundEnabled());
        setTextForSoundState();
        AudioListener.volume = soundEnabled() ? 1 : 0;
    }

    private void setTextForSoundState(){
        if(soundEnabled()){
            img.sprite = enabledImage;
        } else {
            img.sprite = disabledImage;
        }
    }

    public bool soundEnabled()
    {
        return PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
    }

    public void setSoundEnabled(bool enabled)
    {
        PlayerPrefs.SetInt("SoundEnabled", enabled ? 1 : 0);
    }
}
