using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
        
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
            

        /*[Header("GameOver")] 
        [SerializeField] private EventReference gameOverStinger;*/
        

        
        private FMODUnity.StudioEventEmitter sEmitter;
        private float number = 1;
        [Header("BGM")]
        public Emitters eventEmitters;
            
            [System.Serializable]
            public struct Emitters
            {
                public StudioEventEmitter partA;
                public StudioEventEmitter partB;
                public StudioEventEmitter partC;
            }
        
            
        [Header("BankRef")]
        [FMODUnity.BankRef] public string masterBank;
        public bool loadSampleData;

       

        public void PlayBoss()
        {
            if (bossMusic.IsActive == false)
            {
                bossMusic.Play();
            }
            bossMusic.SetParameter(bossParamName, bossParamValue++);
            Debug.Log("ParamValue " + bossParamValue);
        }

       /* public void PlayGameOver()
        {
            RuntimeManager.PlayOneShot(gameOverStinger);
        }*/

        private void BankLoader()
        {
            Debug.Log("Has the bank loaded? " + FMODUnity.RuntimeManager.HasBankLoaded(masterBank));
            FMODUnity.RuntimeManager.LoadBank(masterBank, loadSampleData);
            Debug.Log("Has the bank loaded? " + FMODUnity.RuntimeManager.HasBankLoaded(masterBank));
        }

        public void Play(StudioEventEmitter emitter)
        {
            if (emitter.IsActive == false)
            {
                emitter.Play();
                Debug.Log("Playing tunes");
            }
            else
            {
                Debug.Log("Already playing");
            }
        }

        public void Stop(StudioEventEmitter emitter)
        {
            emitter.Stop();
            Debug.Log("Stop");
        }

        public void SetParameter(StudioEventEmitter emitter, string paramName, float paramValue, bool ignoreSeek)
        {
            emitter.SetParameter(paramName, paramValue, ignoreSeek);
            Debug.Log("Set parameter");
        }
}
