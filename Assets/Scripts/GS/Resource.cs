using UnityEngine;
using UnityEngine.Events;

namespace GS
{
    [RequireComponent(typeof(Structure))]
    public class Resource : MonoBehaviour
    {
        [SerializeField] private ResourceConfig config;

        public static event UnityAction<ResourceType, int> OnYield;

        public ResourceType Type => config.Type;
        public int YieldsPerSecond => config.YieldsPerSecond;
        
        private float _elapsed;
        private Structure _structure;

        private void Awake()
        {
            _structure = GetComponent<Structure>();
        }

        private void Update()
        {
            if (!_structure.IsReady)
            {
                return;
            }

            _elapsed += Time.deltaTime;
            if (_elapsed < 1f)
            {
                return;
            }

            OnYield?.Invoke(config.Type, config.YieldsPerSecond);
            _elapsed = 0;
        }
    }
}