using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundToggle : MonoBehaviour
{
    private TextMeshProUGUI text;
    private AudioListener listener;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        listener = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>();
        setTextForSoundState();
        listener.enabled = soundEnabled();
    }

    public void click(){
        setSoundEnabled(!soundEnabled());
        setTextForSoundState();
        listener.enabled = soundEnabled();
    }

    private void setTextForSoundState(){
        if(soundEnabled()){
            text.text = "Disable Sound";
        } else {
            text.text = "Enable Sound";
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
