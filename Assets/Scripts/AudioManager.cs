using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] hit;

    public AudioClip[] goal;

    public AudioSource gamePlayAudio, goalAudio, gameResultAudio;

    public AudioClip gameWon, gameLost;

    private AudioManager()
    {

    }

    static AudioManager instance;

    public static AudioManager audioManager
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void PlayHitSound()
    {
        int temp = Random.Range(1,100);
        gamePlayAudio.clip = hit[temp % hit.Length];
        gamePlayAudio.Play();
    }

    public void PlayGoalSound()
    {
        int temp = Random.Range(1, 100);
        goalAudio.clip = goal[temp % goal.Length];
        goalAudio.Play();
    }

    public void PlayGameWin()
    {
        gameResultAudio.clip = gameWon;
        gameResultAudio.Play();
    }

    public void PlayGameLost()
    {
        gameResultAudio.clip = gameLost;
        gameResultAudio.Play();
    }
}
