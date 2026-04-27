using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource sfxSourcePrefab;

    private void OnEnable()
    {
        GameEventsManager.Instance.audioEvents.OnPlaySound += OnPlaySound;

        GameEventsManager.Instance.audioEvents.OnPlaySoundFollowObject += OnPlaySoundFollowObject;
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.audioEvents.OnPlaySound -= OnPlaySound;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void OnPlaySound(SoundData sound, Vector3 position)
    {
        // Debug.Log("PLAYING SOUND");
        if (sound == null) return;

        AudioClip clip = sound.GetRandomClip();
        if (clip == null) return;

        AudioSource source = Instantiate(sfxSourcePrefab, position, Quaternion.identity);

        source.clip = clip;
        source.volume = sound.volume;
        source.pitch = Random.Range(sound.pitchMin, sound.pitchMax);

        source.Play();

        Destroy(source.gameObject, clip.length / source.pitch);
    }
    public void OnPlaySoundFollowObject(SoundData sound, GameObject gameObject)
    {
        if (sound == null) return;

        AudioClip clip = sound.GetRandomClip();
        if (clip == null) return;

        AudioSource source = Instantiate(
            sfxSourcePrefab,
            gameObject.transform.position,
            Quaternion.identity,
            gameObject.transform
        );

        source.clip = clip;
        source.volume = sound.volume;
        source.pitch = Random.Range(sound.pitchMin, sound.pitchMax);

        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.minDistance = 1f;
        source.maxDistance = 70f;

        source.Play();

        Destroy(source.gameObject, clip.length / source.pitch);
    }
}