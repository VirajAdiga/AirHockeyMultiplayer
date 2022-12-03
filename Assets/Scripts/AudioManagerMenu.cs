using UnityEngine;

public class AudioManagerMenu : MonoBehaviour
{
    public AudioSource menuSound;

    public AudioSource buttonClick;

    private void Start()
    {
        PlayMenuSound();
    }

    public void PlayButtonClick()
    {
        buttonClick.Play();
    }

    private void PlayMenuSound()
    {
        menuSound.loop = true;
        menuSound.Play();
    }
}
