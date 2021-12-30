using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GS
{
    public class Node : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private float size;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material blockedMaterial;

        public static event UnityAction<Structure> StartsBuildingStructure;

        public float Size => size;
        public Vector2Int Index { get; private set; }
        public List<Node> NeighborsTier1 { get; private set; } = new List<Node>();
        public List<Node> NeighborsTier2 { get; private set; } = new List<Node>();

        private bool _isAvailable;
        private StructurePreview _previewPrefab;
        private Renderer _rend;
        private Structure _structure;
        private EconomyManager _economyManager;
        private bool _isPermBlocked;

        private void Awake()
        {
            _rend = GetComponent<Renderer>();

            Structure.OnDie += CheckStructure;
        }

        private void OnDestroy()
        {
            Structure.OnDie -= CheckStructure;
        }

        public void Setup(EconomyManager economyManager, Vector2Int index, float x, float z, List<Node> neighborsTier1, List<Node> neighborsTier2,
            bool isActive, bool isAvailable, bool isPermBlocked)
        {
            _economyManager = economyManager;
            Index = index;
            NeighborsTier1 = neighborsTier1;
            NeighborsTier2 = neighborsTier2;
            transform.position = new Vector3(x, 0, z);
            SetActive(isActive);
            SetAvailable(isAvailable);
            _isPermBlocked = isPermBlocked;

            if (_isPermBlocked)
            {
                HighlightAsUnavailable();
            }
        }

        public void SetPreview(StructurePreview preview)
        {
            _previewPrefab = preview;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);

            if (_structure != null)
            {
                _structure.Toggle(!value);
            }
        }

        public void BuildStructure(Structure structurePrefab)
        {
            _structure = Instantiate(structurePrefab, transform.position, Quaternion.identity);
            _structure.Build();
            StartsBuildingStructure?.Invoke(structurePrefab);
            ToggleAvailability(structurePrefab.Size, false);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_previewPrefab == null)
            {
                return;
            }

            var isAvailable = !_isPermBlocked && SizeAndPlaceIsOkToPreview() &&
                              _economyManager.EvaluateBuildCostFor(_previewPrefab.Structure);

            _previewPrefab.OnSnap(this, isAvailable);
        }

        private void RemoveStructure()
        {
            ToggleAvailability(_structure.Size, true);
            _structure = null;
        }

        private void ToggleAvailability(StructureSize size, bool value)
        {
            _isAvailable = value;
            switch (size)
            {
                case StructureSize.Small:
                    NeighborsTier1.ForEach(item => item.SetAvailable(value));
                    break;
                case StructureSize.Medium:
                    NeighborsTier1.ForEach(item => item.SetAvailable(value));
                    NeighborsTier2.ForEach(item => item.SetAvailable(value));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetAvailable(bool value)
        {
            if (_isPermBlocked)
            {
                _isAvailable = false;
            }
            else
            {
                _isAvailable = value;
            }
            

            if (_isAvailable)
            {
                HighlightAsAvailable();
            }
            else
            {
                HighlightAsUnavailable();
            }
        }

        private bool SizeAndPlaceIsOkToPreview()
        {
            return _isAvailable && _previewPrefab.Size switch
            {
                StructureSize.Small => NeighborsTier1.FindAll(node => node._isAvailable).Count >= 8,
                StructureSize.Medium => NeighborsTier2.FindAll(node => node._isAvailable).Count >= 16,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void HighlightAsAvailable()
        {
            _rend.material = defaultMaterial;
        }

        private void HighlightAsUnavailable()
        {
            _rend.material = blockedMaterial;
        }

        private void CheckStructure(Structure structure)
        {
            if (structure == _structure)
            {
                RemoveStructure();
            }
        }
    }
}