using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class SpikeTrap : Activators
{
    public bool isInUse = false;
    [SerializeField] private Rigidbody spikeRB;
    [SerializeField] private float spikeSpeed;
    [SerializeField] private float spikeResetTime;
    [SerializeField] private float spikeShootLenght;
    [SerializeField] private BoxCollider collider;
    private float spikeStartHeight;
    [SerializeField] private bool ejectAndDestroy = false;
    [SerializeField] EventReference SpikeTrapEventRef;

    void Start() {
        spikeRB.isKinematic = true;
        spikeStartHeight = transform.localPosition.y;
        
        // Get the FMOD event instance
        RuntimeManager.CreateInstance(SpikeTrapEventRef);
    }
    public override void ChangeState(bool toState)
    {
        base.ChangeState(toState);
        if (isActive) //animator.SetBool("IsOpen", true);
        {
            if (!isInUse) {
                StartCoroutine(EjectSpikes());
                isInUse = true;
            }
        }
        
    }
    IEnumerator EjectSpikes() {

        collider.enabled = true;
        // Play the FMOD event on hover
        
        //  Code For Opening the spiketrap
        while (transform.localPosition.y < spikeStartHeight + spikeShootLenght) {
            spikeRB.MovePosition(transform.position + transform.up * spikeSpeed * Time.deltaTime);
            yield return null;
        }
        RuntimeManager.PlayOneShot(SpikeTrapEventRef);
        
        if (!ejectAndDestroy) {
            //  Delay before closing
            yield return new WaitForSeconds(spikeResetTime);
            StartCoroutine(ResetSpikes());
        }
        else
            Destroy(gameObject, 0.1f);

    }

    private IEnumerator ResetSpikes() {

        
        //  Code For Closing the spikestrap
        while (transform.localPosition.y > spikeStartHeight) {
            spikeRB.MovePosition(transform.position - transform.up * spikeSpeed * Time.deltaTime);
            yield return null;
        }
        
        //  Delay beore being able to activate trap again
        yield return new WaitForSeconds(spikeResetTime);
        isInUse = false;
    }
}
