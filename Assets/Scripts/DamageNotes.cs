using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNotes : MonoBehaviour
{
    /*

        okay so heres how this gonna work
        we're gonna have a list of notes
        and we're gonna have a list of chords
        a chord is a list of ints, indices into the note list
        we have a currChord that we draw the notes from
        the index of the chord we draw from is decided from 
        the direction of the swipe, which is provided in the function call
    */
    public List<AudioClip> notes;
    //this plays pairs of notes, every two notes is played
    //at the same time
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
            print("trying to go to next chord");
            nextChord();
        }
    }

    public void playDamageChord(){
        StartCoroutine("damageChord", chordPlayTime);
    }

    private void playNoteOfCurrChord(int note)
    {
        int ind = chords[(currChord * chordlen) + note];
        //int ind1 = chords[(currChord * chordlen) + (note * 2)];
        //int ind2 = chords[(currChord * chordlen) + (note * 2) + 1];
        src.PlayOneShot(
            notes[ind], vol);
        /*
        src.PlayOneShot(
            notes[ind1], vol/2);
        src.PlayOneShot(
            notes[ind2], vol/2);
            */

    }
    public void nextChord()
    {
        print(string.Format("currChord: {0} chord count: {1} chordlen: {2} ", currChord, chords.Count, chordlen));
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
        //int startingIndex = currChord == 0 ? 0 : currChord * chordlen - 1;
        //for(int i = startingIndex; i < startingIndex + chordlen; i++){
        for(int i = 0; i < chordlen; i++){
            //NOTES MUST HAVE EVEN NUMBER OF CLIPS
            damageSound();
            yield return new WaitForSeconds(time/chordlen);
        }
        nextChord();
    }
    
}


