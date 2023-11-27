using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;
    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }
        EventManager.AddListener(EventName.ChangeSoundStatus.ToString(), ChangeMuteAll);
    }

    public void PlaySound(SoundId soundId) {
        if (ProfileManager.Instance.playerData.playerResource.soundOn)
        {
            Sound sound = sounds.Find(s => s.soundID == soundId);
            if (sound == null)
                return;
            //if (sound.source.isPlaying) return;
            sound.source.Play();
        }
    }

    public void ChangeMuteAll(object isSoundOn) {
        foreach (Sound s in sounds)
        {
            s.source.mute = !(bool)isSoundOn;
        }
        if ((bool)isSoundOn)
            PlaySound(SoundId.BGSound);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySound(SoundId.UIClick);
        }
    }
}
[System.Serializable]
public class Sound {
    public SoundId soundID;
    public AudioSource source;
    public AudioClip clip;
    public bool loop;
}
