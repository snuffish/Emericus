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
        AudioManager.Instance.PlayOneShot(pickUpSound,gameObject);
    }
    public virtual void DropEvent()
    {
        AudioManager.Instance.PlayOneShot(dropSound,gameObject);

    }

    public virtual void CollisionEvent()
    {
        AudioManager.Instance.PlayOneShot(collisionSound,gameObject);

    }

}
