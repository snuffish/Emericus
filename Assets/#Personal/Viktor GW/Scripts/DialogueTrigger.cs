using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private DialogueManager dialogueManager;
        
        public Dialogue dialogue;
        [HideInInspector]
        public bool dialogueButtonPressed = false;

        private bool DialogueAdded = false;
        private readonly List<GameObject> _children = new List<GameObject>();

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                _children.Add(child.gameObject);
            }

            foreach (var child in _children)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void TriggerDialogue()
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
            //dialogueManager.StartDialogue(dialogue);
            dialogueManager.dialogueOpen = true;
            dialogueManager.playerBlockers = _children;
            WakeChildObjects();
            DetachChildObjectsFromParent();
            Destroy(gameObject);
        }

        private void WakeChildObjects()
        {
            foreach (var child in _children)
            {
                child.gameObject.SetActive(true);
            }
        }
        
        private void DetachChildObjectsFromParent()
        {
            foreach (var child in _children)
            {
                child.gameObject.transform.parent = null;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (DialogueAdded == false)
            {
                dialogueManager.dialogues.Enqueue(dialogue);
                DialogueAdded = true;
            }
            if (dialogueManager.dialogueOpen) return;
            TriggerDialogue();
        }
    }
}