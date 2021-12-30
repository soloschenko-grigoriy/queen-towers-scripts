using UnityEngine;
using UnityEngine.UI;

namespace GS
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;

        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.minValue = 0;
            _slider.maxValue = 1;
        }

        private void Update()
        {
            _slider.value = enemy.Health / enemy.HealthMax;
        }
    }
}