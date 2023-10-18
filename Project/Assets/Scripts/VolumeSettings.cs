using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer backgroundMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        SetBackgroundVolume();
    }

    public void SetBackgroundVolume()
    {
        float volume = volumeSlider.value;
        backgroundMixer.SetFloat("music", Mathf.Log10(volume)*20);
    }
}
