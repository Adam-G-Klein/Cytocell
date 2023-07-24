using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpNotes : MonoBehaviour
{
    //first three will be low notes
    //second three will be the high notes
    //NOTES MUST HAVE EVEN NUMBER OF CLIPS
    public List<AudioClip> notes;
    private AudioSource src;
    public float vol = 0.1f;
    public float time = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        
    }


    public void playLevelUpChords(){
        StartCoroutine("levelUpChordsCorout", time);
    }

    private IEnumerator levelUpChordsCorout(float time){
        for(int i = 0; i < notes.Count/2; i++){
            //NOTES MUST HAVE EVEN NUMBER OF CLIPS
            src.PlayOneShot(notes[i], vol/2);
            src.PlayOneShot(notes[i + (notes.Count / 2)], vol/2);
            yield return new WaitForSeconds(time/notes.Count);
        }
    }
}
