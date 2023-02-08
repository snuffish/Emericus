using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] EventReference playerFootsteps;
    [SerializeField] EventReference playerHurt;
    [SerializeField] EventReference playerJump;
    [SerializeField] EventReference playerLand;
    [SerializeField] EventReference playerRespawn;

    public void PlayFootstep(GameObject soundLocation)
    {
        AudioManager.Instance.PlayOneShot(playerFootsteps, soundLocation);
        Debug.Log("Footssete");
    }

    public void PlayHurt(GameObject soundLocation)
    {
        AudioManager.Instance.PlayOneShot(playerHurt, soundLocation);
    }

    public void PlayJump(GameObject soundLocation)
    {
        AudioManager.Instance.PlayOneShot(playerJump, soundLocation);
    }

    public void PlayLand(GameObject soundLocation)
    {
        AudioManager.Instance.PlayOneShot(playerLand, soundLocation);
    }

    public void PlayRespawn(GameObject soundLocation)
    {
        AudioManager.Instance.PlayOneShot(playerRespawn, soundLocation);
    }
    
}