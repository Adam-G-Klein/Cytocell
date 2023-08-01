using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNotes : MonoBehaviour
{
    /*

        list of notes
        and list of chords
        a chord is a list of ints, indices into the note list
        we have a currChord that we draw the notes from
        the index of the chord we draw from is decided from 
        counter associated with the swipe, which is provided in the function call
    */
    public List<AudioClip> notes;
    public List<int> chords;
    public int currChord;
    private AudioSource src;
    // amnt of notes in a chord
    public const int chordlen = 3;
    public int note;
    public float vol;
    public float chordPlayTime = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        currChord = 0;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            playDamageChord();
        }
        if(Input.GetKeyDown(KeyCode.O)){
            nextChord();
        }
    }

    public void playDamageChord(){
        StartCoroutine("damageChord", chordPlayTime);
    }

    private void playNoteOfCurrChord(int note)
    {
        int ind = chords[(currChord * chordlen) + note];
        src.PlayOneShot(
            notes[ind], vol);

        // uncomment if we want to try playing the whole chord again 
        // instead of arpeggiating
        /*
        int ind1 = chords[(currChord * chordlen) + (note * 2)];
        int ind2 = chords[(currChord * chordlen) + (note * 2) + 1];
        src.PlayOneShot(
            notes[ind1], vol/2);
        src.PlayOneShot(
            notes[ind2], vol/2);
            */

    }
    public void nextChord()
    {
        if(currChord >= (chords.Count / chordlen) - 1)
        {
            currChord = 0;
        }
        else
        {
            currChord += 1;
        }
        note = 0;
    }
    public void damageSound()
    {
        
        playNoteOfCurrChord(note);
        if(note < 2)
        {
            note += 1;
        }
        else
        {
            note = 0;
        }

    }
    private IEnumerator damageChord(float time){
        for(int i = 0; i < chordlen; i++){
            //notes must have even number of clips
            damageSound();
            yield return new WaitForSeconds(time/chordlen);
        }
        nextChord();
    }
    
}


