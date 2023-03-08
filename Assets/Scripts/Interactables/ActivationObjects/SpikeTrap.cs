using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Activators
{
    public bool isInUse = false;
    [SerializeField] private Rigidbody spikeRB;
    [SerializeField] private float spikeSpeed;
    [SerializeField] private float spikeResetTime;
    [SerializeField] private float spikeShootLenght;
    [SerializeField] private BoxCollider collider;
    private float spikeStartHeight;

    void Start() {
        spikeRB.isKinematic = true;
        spikeStartHeight = spikeRB.transform.position.y;
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
        
        //  Code For Opening the spiketrap
        while (spikeRB.transform.localPosition.y < spikeStartHeight + spikeShootLenght) {
            spikeRB.MovePosition(spikeRB.transform.position + spikeRB.transform.up * spikeSpeed * Time.deltaTime);
            yield return null;
        }
        
        //  Delay before closing
        yield return new WaitForSeconds(spikeResetTime);
        StartCoroutine(ResetSpikes());
    }

    private IEnumerator ResetSpikes() {
        
        //  Code For Closing the spikestrap
        while (spikeRB.transform.localPosition.y > spikeStartHeight) {
            spikeRB.MovePosition(spikeRB.transform.position - spikeRB.transform.up * spikeSpeed * Time.deltaTime);
            yield return null;
        }
        
        spikeRB.MovePosition(new Vector3(transform.position.x, spikeStartHeight, transform.position.z));
        //  Delay beore being able to activate trap again
        yield return new WaitForSeconds(spikeResetTime);
        isInUse = false;
    }
}
