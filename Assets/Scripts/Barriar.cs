using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barriar : MonoBehaviour
{
    public AudioClip hitAudio;


    public void playAudio()
    {
        AudioSource.PlayClipAtPoint(hitAudio, transform.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
