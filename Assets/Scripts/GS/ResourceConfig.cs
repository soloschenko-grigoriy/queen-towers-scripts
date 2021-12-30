using UnityEngine;

namespace GS
{
    [CreateAssetMenu(fileName = "Resource Config", menuName = "Resource Config", order = 0)]
    public class ResourceConfig : ScriptableObject
    {
        [SerializeField] private ResourceType type;
        [SerializeField] private int yieldsPerSecond;

        public ResourceType Type => type;
        public int YieldsPerSecond => yieldsPerSecond;
    }
}