using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Sound
{
    public class SoundSource
    {
        public AudioSource source;
        public string name;

    }
    public class SoundSourceManager : MonoBehaviour
    {

        [SerializeField] private GameObject sourceObject;

        public void Init(GameObject soundSource = null)
        {
            if (sourceObject == null && soundSource == null)
                sourceObject = gameObject;
            else if(sourceObject == null && soundSource != null)
                sourceObject = soundSource;
        }
        public SoundSource CreateSource(string name)
        {
            if (sourceObject == null)
                Debug.Log("source object null");
            SoundSource _source = new SoundSource();
            _source.name = name;
            _source.source = sourceObject.AddComponent<AudioSource>();
            return _source;
        }

    }
}