using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class DialogueManager : MonoBehaviour
    {
        public Text nameText;
        private Queue<string> Names;
        public Text dialogueText;
        private Queue<string> Sentences;
        private AudioSource AudioSource;
        private Queue<AudioClip> AudioClips;
        public List<GameObject> playerBlockers;
        //public bool isUsingVoiceLine = false;

        public Queue<Dialogue> dialogues;

        public Animator animator;
        public int letterDelay = 1;
        public bool dialogueOpen = false;
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private IEnumerator typeSentence;
        private void Awake()
        {
            dialogues = new Queue<Dialogue>();
        }

        private void Start()
        {
            Sentences = new Queue<string>();
            Names = new Queue<string>();
            AudioSource = GetComponent<AudioSource>();
            AudioClips = new Queue<AudioClip>();
            
            StartCoroutine(DisplayDialogue());
        }

        public void StartDialogue (Dialogue dialogue)
        {
            dialogueOpen = true;
            animator.SetBool(IsOpen, true);

            Sentences.Clear();
            Names.Clear();
            AudioClips.Clear();

            foreach (var sentence in dialogue.sentences)
            {
                Sentences.Enqueue(sentence.text);
                Names.Enqueue(sentence.name);
                AudioClips.Enqueue(sentence.voiceLine);
            }

            DisplayNextSentence();

        }

        private void DisplayNextSentence()
        {
            if (Sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            var sentence = Sentences.Dequeue();
            var name = Names.Dequeue();
            var voiceLine = AudioClips.Dequeue();
            if (typeSentence != null)
            {
                StopCoroutine(typeSentence);
            }
            typeSentence = TypeSentence(sentence, name, voiceLine);
            StartCoroutine(typeSentence);
           
        }

        private void Update()
        {
            
            if (dialogueOpen != true) return;

            if (AudioSource.clip == null) return;
            if (AudioSource.isPlaying != true)
            {
                DisplayNextSentence();
            }
        }

        private IEnumerator DisplayDialogue()
        {
            while (true)
            {
                if (dialogues.Count != 0)
                {
                    Dialogue dialogue = dialogues.Dequeue();
                    StartDialogue(dialogue);
                }

                yield return new WaitUntil(DialogueComplete);
            }
            /*Debug.Log(dialogues.Count);
            foreach (var dialogue in dialogues)
            {
                StartDialogue(dialogue);
                yield return new WaitUntil(DialogueComplete);
            }*/
            // ReSharper disable once IteratorNeverReturns
        }

        private bool DialogueComplete()
        {
            return dialogueOpen == false;
        }

        private IEnumerator TypeSentence (string sentence, string name, AudioClip voiceLine)
        {
            dialogueText.text = "";
            nameText.text = name;

            if (voiceLine != null)
            {
                AudioSource.clip = voiceLine;
                AudioSource.Play();
            }

            foreach (var letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(letterDelay*Time.deltaTime);
            }
        }

        private void EndDialogue()
        {
            foreach (var blocker in playerBlockers)
            {
                Destroy(blocker.gameObject);
            }
            
            dialogueOpen = false;
            animator.SetBool(IsOpen, false);
            AudioSource.Stop();
            /*if (dialogues.Count != 0)
            {
                Dialogue dialogue = dialogues.Dequeue();
                StartDialogue(dialogue);
            }*/
           
        }
    }
}