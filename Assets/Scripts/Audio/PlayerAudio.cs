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
        if (!playerFootsteps.IsNull) AudioManager.Instance.PlayOneShot(playerFootsteps, soundLocation);
    }

    public void PlayHurt(GameObject soundLocation)
    {
        if (!playerHurt.IsNull) AudioManager.Instance.PlayOneShot(playerHurt, soundLocation);
    }

    public void PlayJump(GameObject soundLocation)
    {
        if (!playerJump.IsNull) AudioManager.Instance.PlayOneShot(playerJump, soundLocation);
    }

    public void PlayLand(GameObject soundLocation)
    {
        if (!playerLand.IsNull) AudioManager.Instance.PlayOneShot(playerLand, soundLocation);
    }

    public void PlayRespawn(GameObject soundLocation)
    {
        if (!playerRespawn.IsNull) AudioManager.Instance.PlayOneShot(playerRespawn, soundLocation);
    }

}