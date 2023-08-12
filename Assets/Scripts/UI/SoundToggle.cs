using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoundToggle : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
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
