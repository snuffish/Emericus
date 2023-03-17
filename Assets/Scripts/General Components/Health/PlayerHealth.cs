using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : Health
{
    [System.Serializable]
    public struct VoiceSettings
    {
        public VoiceAction vAction;
        public VoiceEvent vEvent;
        //EmitterTest
        //public StudioEventEmitter vEmitter;
        public string keyName;
        public string paramName;
        public float paramValue;
        public bool ignoreSeek;
        public bool paramGlobal;
    }
    
    [NonReorderable] public VoiceSettings[] voiceSettings;
    private VLManager vM;
    
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private float transistionTime;
    void Start()
    {
        vM = GameObject.Find("VLManager").GetComponent<VLManager>();
    }
    
    protected override void Die() {
        Debug.Log("Dead");
        StartCoroutine(_sceneManager.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, transistionTime));
        
        GameObject.Find("MusicManager").GetComponent<NewAManager>().PlayGameOver();
        
        foreach (VoiceSettings v in voiceSettings)
        {
            switch (v.vAction)
            {
                case VoiceAction.PlayDialogue:
                    vM.PlayDialogue(v.vEvent, v.keyName);
                    vM.dialogueInstance.start();
                    break;
                case VoiceAction.SetParameter:
                    vM.SetParameterVL(v.vEvent, v.paramName, v.paramValue, v.ignoreSeek, v.paramGlobal);
                    break;
                default:Debug.Log("Error! No valid action.");
                    break;
            }
        }
    }
}
