using System.Collections;
using UnityEngine;

namespace GS
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip cooldownMusic;
        [SerializeField] private AudioClip battleMusic;
        [SerializeField] private AudioClip battleMusicStarts;
        [SerializeField] private AudioClip victoryMusic;
        [SerializeField] private AudioClip lostMusic;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlayCooldownMusic()
        {
            _source.Stop();
            _source.clip = cooldownMusic;
            _source.Play();
        }

        public void PlayBattleMusic()
        {
            _source.Stop();
            StartCoroutine(DoPlayBattleMusic());
        }

        private IEnumerator DoPlayBattleMusic()
        {
            _source.clip = battleMusicStarts;
            _source.Play();

            yield return new WaitForSeconds(3f);

            _source.clip = battleMusic;
            _source.Play();
        }

        public void PlayVictoryMusic()
        {
            _source.Stop();
            _source.clip = victoryMusic;
            _source.Play();
        }

        public void PlayLostMusic()
        {
            _source.Stop();
            _source.clip = lostMusic;
            _source.Play();
        }
    }
}