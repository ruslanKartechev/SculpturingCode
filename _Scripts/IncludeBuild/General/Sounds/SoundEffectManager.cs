using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
namespace General.Sound
{

    public class SoundEffectManager : MonoBehaviour
    {
        [Header("Sound Data")]
        [SerializeField] private SoundData sounds;
        
        private List<SoundSource> soundSources = new List<SoundSource>();
        [SerializeField] private SoundSourceManager sourceManager;
        [SerializeField] private SoundLoopHandler soundLooper;
        [SerializeField] private bool playMusic = true;
        [Range(0f,1f)]
        [SerializeField] private float musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float effectsVolume = 1f;
        public float MusicVolume { get { return musicVolume; }}
        public float EffectsVolume { get { return effectsVolume; } }

        public void Init()
        {
            if (sounds == null)
                Debug.Log("</color=red>SoundData is not assigned </color>");
            sourceManager.Init();
            soundSources.Add(sourceManager.CreateSource(SourceByName.MusicSource));
            soundSources.Add(sourceManager.CreateSource(SourceByName.EffectsSource));
            if (soundLooper == null)
                soundLooper = GetComponent<SoundLoopHandler>();
            if (playMusic)
                PlayMusic();
        }
        public void StartSoundEffect(string soundName)
        {
            Sound sound = sounds.soundEffects.Find(x => x.name == soundName);
            AudioSource source = soundSources.Find(x => x.name == SourceByName.EffectsSource).source;
            soundLooper.StartLoop(source, sound, EffectsVolume);

        }
        public void StopSoundEffect(string soundName)
        {
            soundLooper.StopLoop(soundName);
        }
        public void PlaySingleTime(string soundName)
        {
            Sound sound = sounds.soundEffects.Find(x => x.name == soundName);
            AudioSource source = soundSources.Find(x => x.name == SourceByName.EffectsSource).source;
            source.pitch = sound.pitch;
            source.volume = sound.volume * EffectsVolume;
            source.PlayOneShot(sound.clip);
        }

        public void PlayMusic()
        {
            Sound sound = sounds.music[Random.Range(0, sounds.music.Count)];
            AudioSource source = soundSources.Find(x => x.name == SourceByName.MusicSource).source;
            source.clip = sound.clip;
            source.pitch = sound.pitch;
            source.volume = sound.volume * MusicVolume;
            source.PlayOneShot(source.clip);
        }

        public void OffMusic()
        {
            musicVolume = 0;
            InitSoundSources();
        }
        public void OffEffects()
        {
            effectsVolume = 0;
            InitSoundSources();
        }
        public void OnMusic()
        {
            musicVolume = 1;
            InitSoundSources();
        }
        public void OnEffects()
        {
            effectsVolume = 1;
            InitSoundSources();
        }
        private void InitSoundSources()
        {
            foreach(SoundSource source in soundSources)
            {
                if(source.name == SourceByName.EffectsSource)
                source.source.volume *= EffectsVolume;
                else if(source.name == SourceByName.MusicSource)
                    source.source.volume *= MusicVolume;
            }
        }

    }
}