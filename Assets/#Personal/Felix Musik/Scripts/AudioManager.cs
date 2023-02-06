using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
   public static AudioManager Instance;

   //Bankladdare
   private void Awake()
   {
      BankLoader();
      if (Instance != null && Instance != this)
      {
         Destroy(this);
      }
      else
      {
         Instance = this;
      }
      DontDestroyOnLoad(gameObject);
   }

   [Header("BankRef")] 
   [BankRef] public string masterBank;

   public bool loadSampleData;

   private void BankLoader()
   {
      Debug.Log("Has the bank loaded? " + RuntimeManager.HasBankLoaded(masterBank));
      RuntimeManager.LoadBank(masterBank, loadSampleData);
   }

   
   //Emitters för bakgrundsmusik
   [Header("BGM")] 
   public Emitters eventEmitters;
   
   [System.Serializable]
   
   public struct Emitters
   {
      public StudioEventEmitter bgmMusic;
   }

   [Header("Puzzle")] 
   [SerializeField] private EventReference puzzleStinger;

   [SerializeField] private string puzzleParamName;
   [SerializeField] private float puzzleParamValue;

   public void PlayPuzzle()
   {
      RuntimeManager.PlayOneShot(puzzleStinger);
   }
   
   //Player Funktioner Events.
   [Header("Player")]
   [SerializeField] private EventReference PlayerFootsteps;
   /*
   [EventRef]
   public string playerFootsteps;
   EventInstance footstepInstance;
   */
   
   
  /* //Styr stinger för när man dör
   [Header("GameOver")] 
   [SerializeField] private EventReference gameOverStinger;
   
   public void PlayGameOver()
   {
      RuntimeManager.PlayOneShot(gameOverStinger);
   }
   //Koppla till "Game Over" */

  /*
   //Styr Combatmusiken
   private bool inCombat = false;
   [Header("Combat")] 
   public float combatWaitTime = 3f;

   [SerializeField] private string combatParamName;
   [SerializeField] private float combatParamValue = 0f;

   public void Combat()
   {
      if (!inCombat)
      {
         Debug.Log("Fight MothaFocka!");
         RuntimeManager.StudioSystem.setParameterByName("Combat", 1);
      }
   }*/
   
   /*[Header("Sound")]
      
      [Header("Ambience")]
      */
   
 
   //Play, Stop, Parameter
   public void Play(StudioEventEmitter emitter)
   {
      if (emitter.IsActive == false)
      {
         emitter.Play();
         Debug.Log("Playing!");
      }
      else
      {
         Debug.Log("Already playing!");
      }
   }

   public void Stop(StudioEventEmitter emitter)
   {
      emitter.Stop();
      Debug.Log("Shut up!");
   }

   public void SetParameter(StudioEventEmitter emitter, string paramName, float paramValue, bool ignorSeek)
   {
      emitter.SetParameter(paramName, paramValue, ignorSeek);
      Debug.Log("Setting parameter.");
   }
}
