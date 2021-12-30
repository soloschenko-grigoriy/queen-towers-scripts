using UnityEngine;

namespace GS
{
    [RequireComponent(typeof(AudioSource))]
    public class StructureAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip destroyedSfx;
        [SerializeField] private AudioClip turretAttack;
        [SerializeField] private AudioClip constructingPhase1;
        [SerializeField] private AudioClip constructingPhase2;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlayDestroyedSfx()
        {
            _source.Stop();
            _source.volume = 0.5f;
            _source.clip = destroyedSfx;
            _source.loop = false;
            _source.Play();
        }

        public void PlayTurretAttackSfx()
        {
            _source.Stop();
            _source.volume = 0.5f;
            _source.clip = turretAttack;
            _source.loop = false;
            _source.Play();
        }

        public void PlayConstructingSfx()
        {
            _source.Stop();
            _source.volume = 0.1f;
            _source.clip = Random.Range(0, 1) > 0.5 ? constructingPhase1 : constructingPhase2;
            _source.loop = true;
            _source.Play();
        }

        public void StopAll()
        {
            _source.Stop();
        }
    }
}