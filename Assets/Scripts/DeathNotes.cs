using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathNotes : MonoBehaviour
{
    public List<AudioClip> notes;
    public List<int> chords;
    private AudioSource src;
    private const int chordlen = 4;
    private SwipeNotes snotes;
    public int currNote;
    public float vol = 0.1f;

    //TEST VAL, this should come from swipe notes
    private int currChord;

    private int notesToPlay;
    private bool noteCoroutRunning = false;
    public float noteDelay = 0.3f;

    void Start()
    {
        src = GetComponent<AudioSource>();
        snotes = transform.parent.GetComponentInChildren<SwipeNotes>();
        currNote = 0;
        noteCoroutRunning = false;
        
    }

    /* this was just for testing, should queue on snotes.lastchord
     * 
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
        currNote = 0;
    }
    */

    private void incCurrNote()
    {
        currNote = currNote < chordlen - 1 ? currNote + 1 : 0;
    }


    public void queueNote()
    {
        notesToPlay += 1;
        if (!noteCoroutRunning)
        {
            StartCoroutine("noteCorout");
        }
    }

    public void playNote(int note)
    {
        // this note doesn't play until after enemydeath,
        //snotes will always have advanced
        print("note: " + note);
        print("ind into chords: " + ((snotes.lastChord * chordlen) + note));
        int ind = chords[(snotes.lastChord * chordlen) + note];
        src.PlayOneShot(
            notes[ind], vol);
        incCurrNote();
    }

    /* waiting on this bs until the normal shit works
    private IEnumerator noteCorout()
    {
        noteCoroutRunning = true;
        while(notesToPlay > 0)
        {
            if(currNote < chordlen - 1)
            {
                //if we're on note 0 or 1,
                //just increment
                currNote += 1;
            }else if(currNote == chordlen - 2 && notesToPlay > 2)
            {
                //watch out, if the diff in animation delay
                //is too much, notesToPlay may not accurately
                //reflect the amount of more times queuenote will
                //be called during this purge

                //this case means we need to loop though
                currNote = 0;
            }
            

            playNote(currNote);
            notesToPlay -= 1;
            yield return new WaitForSeconds(noteDelay);
        }
    }
    */

}
