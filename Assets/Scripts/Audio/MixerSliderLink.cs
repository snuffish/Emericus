using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using FMODUnity;
using FMOD.Studio;

public enum VCAName
{
    Master
}

[RequireComponent(typeof(Slider))]

public class MixerSliderLink : MonoBehaviour
{
    public VCAName vcaName;
    
    public Slider m_Slider;

    private VCA vca;

    private string vcaMaster = "vca:/Master";
    /*private string vcaSFX = "vca:/SFX";
    private string vcaMusic = "vca:/Music";*/

    void Awake()
    {
        switch (vcaName)
        {
            case VCAName.Master: vca = RuntimeManager.GetVCA(vcaMaster);
                break;
            /*case VCAName.Music: vca = RuntimeManager.GetVCA(vcaMusic);
                break;
            case VCAName.SFX: vca = RuntimeManager.GetVCA(vcaSFX);
                break;*/
            default: vca = RuntimeManager.GetVCA(vcaMaster);
                break;
        }
        
        float value;
        vca.getVolume(out value);
        
        m_Slider.value = (value);

        m_Slider.onValueChanged.AddListener(SliderValueChange);
    }
    void SliderValueChange(float value)
    {
        vca.setVolume(value);
    }
}
