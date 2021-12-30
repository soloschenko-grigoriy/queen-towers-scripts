using UnityEngine;

namespace GS
{
    [CreateAssetMenu(fileName = "Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField, Range(10, 1000)] private int healthCapacity;
        [SerializeField, Range(10, 500)] private float attack;

        public int HealthCapacity => healthCapacity;
        public float Attack => attack;
    }
}