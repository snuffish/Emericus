using UnityEngine;
using FMODUnity;

public class BankLoader : MonoBehaviour
{
    [BankRef] 
    public string bankPath;
    public bool preLoad = true;

    private void Awake()
    {
       if (!RuntimeManager.HasBankLoaded(bankPath))
           {
               RuntimeManager.LoadBank(bankPath, preLoad);
           }
           else
           {
               RuntimeManager.UnloadBank(bankPath);
           } 
    }
    
}
