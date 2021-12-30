using UnityEngine;
using UnityEngine.EventSystems;

namespace GS
{
    [RequireComponent(typeof(Collider))]
    public class StructurePreview : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Structure structurePrefab;
        [SerializeField] private Transform model;
        [SerializeField] private Color unavailableColor = Color.red;
        
        public StructureSize Size => structurePrefab.Size;
        public Structure Structure => structurePrefab;

        private Node _current;
        private Material _meshMaterial;

        private void Awake()
        {
            _meshMaterial = model.GetComponent<MeshRenderer>().material;
        }

        private void OnDisable()
        {
            _current = null;
        }

        public void OnSnap(Node node, bool isAvailable)
        {
            _meshMaterial.color = isAvailable ? Color.white : unavailableColor;
            transform.position = node.transform.position;
            gameObject.SetActive(true);

            _current = isAvailable ? node : null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_current == null)
            {
                return;
            }

            _current.BuildStructure(structurePrefab);
            _current = null;
        }
    }
}