using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWeight : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetWeightToOne()
    {
        animator.SetLayerWeight(1, 1);
    }
    public void SetWeightToZero()
    {
        animator.SetLayerWeight(1, 0.01f);
        animator.SetBool("Throw", false);
    }
}
