using UnityEngine;

namespace GS
{
    [RequireComponent(typeof(AudioSource))]
    public class EnemyAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip attackAAudio;
        [SerializeField] private AudioClip attackBAudio;
        [SerializeField] private AudioClip deathAAudio;
        [SerializeField] private AudioClip deathBAudio;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlayDeathSound()
        {
            StopPlayingAll();
            _source.clip = Random.Range(0, 1) > 0.5 ? deathAAudio : deathBAudio;
            _source.Play();
        }

        public void PlayAttackSound()
        {
            StopPlayingAll();
            _source.clip = Random.Range(0, 1) > 0.5 ? attackAAudio : attackBAudio;
            _source.Play();
        }

        public void StopPlayingAll()
        {
            _source.Stop();
        }
    }
}