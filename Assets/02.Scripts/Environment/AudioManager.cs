using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance=>_instance;
    private AudioSource _audioSource;


    private void Awake()
    {
        if (_instance==null) 
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
        
        
    public void PlayEffectClip(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
    }
}
