using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomClip : MonoBehaviour
{
    public List<AudioClip> clipList;
    private AudioSource asource;

    // Start is called before the first frame update
    void Start()
    {
        asource = GetComponent<AudioSource>();
        StartCoroutine("playClips");
        
    }

    private IEnumerator playClips()
    {
        while (gameObject.activeSelf)
        {
            AudioClip chosenClip = clipList[Random.Range(0, clipList.Count - 1)];
            asource.clip = chosenClip;
            asource.Play();
            yield return new WaitForSeconds(chosenClip.length);
        }
    }
}
