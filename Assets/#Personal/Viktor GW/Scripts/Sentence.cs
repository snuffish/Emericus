using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Audio;

namespace Script
{
    [System.Serializable]

    public class Sentence
    {
        public string name;
        [TextArea(3, 10)] public string text;
        public AudioClip voiceLine;
    }
}