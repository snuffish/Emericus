using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class PhysicsSounds : MonoBehaviour
{
    [SerializeField] protected EventReference dropSound;
    [SerializeField] protected EventReference pickUpSound;
    [SerializeField] protected EventReference collisionSound;
    public virtual void PickUpEvent()
    {
        if (!pickUpSound.IsNull) AudioManager.Instance.PlayOneShot(pickUpSound, gameObject);
    }
    public virtual void DropEvent()
    {
        if (!pickUpSound.IsNull) AudioManager.Instance.PlayOneShot(dropSound, gameObject);
    }

    public virtual void CollisionEvent()
    {
        if (!pickUpSound.IsNull) AudioManager.Instance.PlayOneShot(collisionSound, gameObject);
    }

}
