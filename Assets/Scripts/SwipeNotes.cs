using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeNotes : MonoBehaviour
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
    public List<AudioClip> sfx;
    // so that it's serializable, we assume every chord has 3 notes for now
    public List<int> chords;
    public int currChord;
    private AudioSource src;
    // amnt of notes in a chord
    private const int chordlen = 3;
    public int note;
    public float vol = 1;
    public float initvol = 0.1f;
    public float volscale = 0.1f;
    public float initvolscale= 0.5f;

    public float lowScaleTurnaround = 0.001f;
    public int lastChord { get
        {
            return currChord == 0 ? (chords.Count / chordlen) - 1 : currChord - 1;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        currChord = 0;
        vol = initvol;
        
    }

    private void playNoteOfCurrChord(int note)
    {
        int ind = chords[(currChord * chordlen) + note];
        src.PlayOneShot(
            notes[ind], vol);

    }
    private void playRandSfx()
    {
        src.PlayOneShot(sfx[Random.Range(0, sfx.Count - 1)], 0.2f);
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
        vol = initvol;
        volscale = initvolscale;
    }
    public void swipeSound()
    {
        
        playNoteOfCurrChord(note);
        playRandSfx();
        if(note < 2)
        {
            note += 1;
        }
        else
        {
            note = 0;
            if(vol <= lowScaleTurnaround && volscale < 1)
            {
                volscale += 1;
            }else if(vol > initvol && volscale > 1){
                volscale -= 1;
            }
            vol *= volscale;
        }

    }
    /*
    public void swipeSound(Vector2 movDir)
    {
        float angle = Vector2.SignedAngle(Vector2.right, movDir);
        if(angle < 0)
        {
            angle = 360 + angle;
        }
        print("swipesound got angle: " + angle.ToString());
        int note = 0;
        if(angle < 120)
        {
            note = 0;
        }else if(angle < 240)
        {
            note = 1;
        }
        else
        {
            note = 2;
        }
        print("swipesound playing note: " + note.ToString());
        playNoteOfCurrChord(note);


    }
    */

}
