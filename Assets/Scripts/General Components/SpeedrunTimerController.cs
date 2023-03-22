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
        float timeLeft = rawTime;
        float minutes = 0, seconds = 0, milliseconds;
        while (timeLeft >= 60) {
            minutes++;
            timeLeft -= 60;
        }

        

        return $"{minutes}.{timeLeft:0.###}";
    }
}
