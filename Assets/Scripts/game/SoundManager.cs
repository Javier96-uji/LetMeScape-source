using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Fuentes de audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSource;

    [Header("Efectos")]
    [SerializeField] private AudioClip BabyPickup;
    [SerializeField] private AudioClip BabyDelivery;
    [SerializeField] private AudioClip Windigowalk;
    [SerializeField] private AudioClip WindigoRun;
    [SerializeField] private AudioClip WindigoAttack;
    [SerializeField] private AudioClip Catwalk;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip Lose;

    private void Awake()
    {
        // solo existe un SoundManager en toda la escena
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }
    public void PlayLoop(AudioClip clip)
    {
        if (clip == null || loopSource == null) return;
        if (loopSource.clip == clip && loopSource.isPlaying) return; // evita reiniciar si ya suena
        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopLoop()
    {
        loopSource.Stop();
    }

    // Metodos
    public void PlayBabyPickup() => PlaySFX(BabyPickup);
    public void PlayBabyDelivery() => PlaySFX(BabyDelivery);
    public void PlayWindigoWalk() => PlayLoop(Windigowalk);
    public void PlayWindigoRun() => PlayLoop(WindigoRun);
    public void PlayWindigoAttack() => PlaySFX(WindigoAttack);
    public void PlayWalk() => PlayLoop(Catwalk);
    public void PlayWin() => PlaySFX(Win);
    public void PlayLose() => PlaySFX(Lose);
}
