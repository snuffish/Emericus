using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPuzzleStage : MonoBehaviour
{
  [SerializeField] private float puzzleStage;
  public void SendStage()
  {
    GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayPuzzle();
    puzzleStage++;
  }
}
