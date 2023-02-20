using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveHealer : Interactable
{
    [SerializeField] private List<Health> objectsToHeal;
    public override void Interact() {
        foreach (Health toHeal in objectsToHeal) {
            toHeal.Heal(20);
        }
    }
}