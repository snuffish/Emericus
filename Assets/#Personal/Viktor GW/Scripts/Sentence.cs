using UnityEngine;

namespace _Personal.Viktor_GW.Scripts
{
    [System.Serializable]

    public class Sentence
    {
        public string name;
        [TextArea(3, 10)] public string text;
        public AudioClip voiceLine;
    }
}