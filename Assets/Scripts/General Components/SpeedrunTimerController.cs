using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class SpeedrunTimerController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private bool addTime;

    [SerializeField] private TMP_Text recordText;


    void Start() {
        if (recordText != null) {
            recordText.SetText(convertFromFloatToTimeString(_playerData.speedrunTimer));
        }
    }
    void Update() {
        if(addTime)
            _playerData.speedrunTimer += Time.deltaTime;
        
    }

    public void SaveTime() {
        
        
        using (StreamWriter file = new StreamWriter("SPEEDRUN_RECORDS", true)) {
            file.WriteLine(convertFromFloatToTimeString(_playerData.speedrunTimer));
        }
        
        Debug.Log(convertFromFloatToTimeString(_playerData.speedrunTimer));
    }

    public void ResetTime() {
        _playerData.speedrunTimer = 0;
    }

    private string convertFromFloatToTimeString(float rawTime) {
        float tempTimer = rawTime;
        float minutes = 0, seconds = 0, milliseconds;
        while (tempTimer >= 60) {
            minutes++;
            tempTimer -= 60;
        }

        while (tempTimer >= 1) {
            seconds++;
            tempTimer--;
        }

        milliseconds = tempTimer;

        return $"{minutes}:{seconds}:{milliseconds*10000}";
    }
}
