using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpeedrunTimerController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private bool addTime;

    // Update is called once per frame
    void Update() {
        if(addTime)
            _playerData.speedrunTimer += Time.deltaTime;
    }

    public void SaveTime() {
        using (StreamWriter file = new StreamWriter("SPEEDRUN_RECORDS", true)) {
            file.WriteLine(_playerData.speedrunTimer.ToString());
        }
    }
}
