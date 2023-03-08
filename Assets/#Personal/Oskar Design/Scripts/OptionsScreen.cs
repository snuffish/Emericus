using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenTog;

    public List<ResItem> resolutions = new List<ResItem>();
    public List<QualityItem> qualities = new List<QualityItem>();
    private int selectedResolution;
    private int selectedQuality;

    public TMP_Text resolutionLabel;
    public TMP_Text qualityLabel;


    public TMP_Text mastLabel/*, musicLabel, sfxLabel */;
    public Slider mastSlider/*, musicSlider, sfxSlider */;


    // Start is called before the first frame update
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                selectedResolution = i;

                UpdateResLabel();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;

            UpdateResLabel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void QualityLeft()
    {
        selectedQuality--;
        if (selectedQuality < 0)
        {
            selectedQuality = qualities.Count - 1;
        }

        UpdateQualityLabel();
    }

    public void QualityRight()
    {
        selectedQuality++;
        if (selectedQuality > qualities.Count - 1)
        {
            selectedQuality = 0;
        }

        UpdateQualityLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + "x" + resolutions[selectedResolution].vertical.ToString();
    }

    public void UpdateQualityLabel()
    {
        qualityLabel.text = qualities[selectedQuality].qualityText;
    }


    public void SetMasterVol()
    {
        mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();
    }





    public void ApplyGraphics()   //incorrect name, will apply other options too (ideally)
    {
        //Screen.fullScreen = fullscreenTog.isOn;

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }


}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
[System.Serializable]
public class QualityItem
{
    public string qualityText;
}

