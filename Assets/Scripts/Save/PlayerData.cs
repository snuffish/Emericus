using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects")]
public class PlayerData : ScriptableObject
{
    [Range(0, 100)] public int playerHealth;
    [Range(0, 1)] public float mouseSensitivity;
    [Range(0,1)] public float audioLevel;
    public bool subtitlesIsOn;
}
