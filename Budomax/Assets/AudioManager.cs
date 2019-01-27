using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip BuildingOneWall;
      //RecieveDamage
//BuildingOneWall
//BuildingRoom
//Eating
//EnemyFlyingSpawn
//GenericClick
//UnitDeath
//EnemyDeath
//EnemyShot
//WarriorShot
//BuildingDamage
//RestockFeeder
//BuildRoom-Barracks
//BuildRoom-Kitchen
//BuildRoom-Scav
//UnitAppears
//UnitGoodbyeWarrior
//UnitGoodbyeCook
//UnitGoodbyeScav
// Start is called before the first frame update
    void Start()
    {

        //  int buildingonewallID = EazySoundManager.PlaySound(BuildingOneWall);
      //  playAudio("BuildOneWall");
    }

   public void playAudio(string filename, float volume)
    {
      Debug.Log("\"Sounds/" + filename + "\"");
      string adress = "Sounds/" + filename;
      var clip = Resources.Load<AudioClip>(adress);
      int clipID = EazySoundManager.PlaySound(clip,volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
