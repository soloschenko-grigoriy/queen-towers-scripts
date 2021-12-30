using UnityEngine;
using UnityEngine.UI;

namespace GS
{
    public class StructureHealthBar : MonoBehaviour
    {
        [SerializeField] private Structure structure;

        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.minValue = 0;
            _slider.maxValue = 1;
        }

        private void Update()
        {
            _slider.value = structure.Health / structure.HealthMax;
        }
    }
}