using UnityEngine;

namespace GS
{
    [RequireComponent(typeof(AudioSource))]
    public class UIAudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip btnCLick;
        [SerializeField] private AudioClip structurePlaced;
        [SerializeField, Range(0, 1)] private float defaultVolume;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            Node.StartsBuildingStructure += PlayStructurePlacedSfx;
        }

        private void OnDisable()
        {
            Node.StartsBuildingStructure -= PlayStructurePlacedSfx;
        }

        public void PlayBtnClickSfx()
        {
            _source.Stop();
            _source.volume = defaultVolume;
            _source.clip = btnCLick;
            _source.Play();
        }

        private void PlayStructurePlacedSfx(Structure structure)
        {
            _source.Stop();
            _source.volume = 1;
            _source.clip = structurePlaced;
            _source.Play();
        }
    }
}