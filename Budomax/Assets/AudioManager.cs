using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip AmbientGroundLevel;
    public AudioClip AmbientSpaceLevel;
    public AudioClip AmbientCloudsLevel;
    public AudioClip AmbientBattle;
    bool SpaceIsPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        SpaceIsPlaying = false;
        AmbientGroundLevel = Resources.Load<AudioClip>("Sounds/AmbientGroundLevel");
        AmbientCloudsLevel = Resources.Load<AudioClip>("Sounds/AmbientCloudsLevel");
        AmbientSpaceLevel = Resources.Load<AudioClip>("Sounds/AmbientSpaceLevel");
        AmbientBattle = Resources.Load<AudioClip>("Sounds/AmbientBattle");  
    }

   public void playAudio(string filename, float volume)
    {
      string adress = "Sounds/" + filename;
      var clip = Resources.Load<AudioClip>(adress);
      int clipID = EazySoundManager.PlaySound(clip,volume);

    }

    // Update is called once per frame
    void Update()
    {
       swapBGMusic();
    }

    void swapBGMusic()
    {
        float CamPos = Camera.main.transform.position.y;
   
        if (CamPos > 50 && SpaceIsPlaying)
        {
            EazySoundManager.PlayMusic(AmbientSpaceLevel, 1.0f, true, true, 2, 3);
            SpaceIsPlaying = false;

        }
        else if (CamPos < 50 && !SpaceIsPlaying)
        {
            
            EazySoundManager.PlayMusic(AmbientGroundLevel, 1.0f, true, true, 2, 3);
            SpaceIsPlaying = true;
        }


    }
  
}
