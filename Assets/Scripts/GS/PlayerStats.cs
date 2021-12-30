using UnityEngine;
using UnityEngine.Serialization;

namespace GS
{
    [CreateAssetMenu(fileName = "Player Stats", menuName = "Player Stats", order = 0)]
    public class PlayerStats : ScriptableObject
    {
        public int Wood;
        public int Food;
    }
}