using System;
using System.Collections.Generic;
using UnityEngine;

namespace GS
{
    public class NodeGrid : MonoBehaviour
    {
        [SerializeField] private Node nodePrefab;
        [SerializeField] private StructurePreview turretPreviewPrefab;
        [SerializeField] private StructurePreview farmPreviewPrefab;
        [SerializeField] private StructurePreview lumberMillPreviewPrefab;
        [SerializeField] private EconomyManager economyManager;
        
        private Node[] _nodes = Array.Empty<Node>();
        private StructurePreview _farmPreview;
        private StructurePreview _lumberMillPreview;
        private StructurePreview _turretPreview;
        private bool _isActive;
        private List<Vector2Int> _permBlocked = new List<Vector2Int>();

        private void Awake()
        {
            // env
            _permBlocked.Add(new Vector2Int(3, 13));
            _permBlocked.Add(new Vector2Int(3, 14));
            _permBlocked.Add(new Vector2Int(4, 13));
            _permBlocked.Add(new Vector2Int(4, 14));
            _permBlocked.Add(new Vector2Int(4, 15));
            _permBlocked.Add(new Vector2Int(5, 13));
            _permBlocked.Add(new Vector2Int(5, 14));
            _permBlocked.Add(new Vector2Int(5, 15));
            
            _permBlocked.Add(new Vector2Int(9, 17));
            _permBlocked.Add(new Vector2Int(9, 16));
            _permBlocked.Add(new Vector2Int(10, 17));
            _permBlocked.Add(new Vector2Int(10, 16));
            
            _permBlocked.Add(new Vector2Int(16, 16));
            _permBlocked.Add(new Vector2Int(18, 16));
            _permBlocked.Add(new Vector2Int(18, 17));
            _permBlocked.Add(new Vector2Int(17, 16));
            _permBlocked.Add(new Vector2Int(17, 17));
            
            _permBlocked.Add(new Vector2Int(15, 2));
            _permBlocked.Add(new Vector2Int(15, 3));
            _permBlocked.Add(new Vector2Int(15, 4));
            _permBlocked.Add(new Vector2Int(16, 1));
            _permBlocked.Add(new Vector2Int(16, 2));
            _permBlocked.Add(new Vector2Int(16, 3));
            _permBlocked.Add(new Vector2Int(16, 4));
            _permBlocked.Add(new Vector2Int(17, 3));
            _permBlocked.Add(new Vector2Int(17, 4));
            
            _permBlocked.Add(new Vector2Int(3, 1));
            
            // flag
            _permBlocked.Add(new Vector2Int(4, 9));
            _permBlocked.Add(new Vector2Int(5, 9));
            _permBlocked.Add(new Vector2Int(6, 9));
            _permBlocked.Add(new Vector2Int(4, 10));
            _permBlocked.Add(new Vector2Int(5, 10));
            _permBlocked.Add(new Vector2Int(6, 10));
            _permBlocked.Add(new Vector2Int(4, 11));
            _permBlocked.Add(new Vector2Int(5, 11));
            _permBlocked.Add(new Vector2Int(6, 11));
            
            // road
            _permBlocked.Add(new Vector2Int(6, 0));
            _permBlocked.Add(new Vector2Int(7, 0));
            _permBlocked.Add(new Vector2Int(6, 1));
            _permBlocked.Add(new Vector2Int(7, 1));
            _permBlocked.Add(new Vector2Int(6, 2));
            _permBlocked.Add(new Vector2Int(7, 2));
            _permBlocked.Add(new Vector2Int(6, 3));
            _permBlocked.Add(new Vector2Int(7, 3));
            _permBlocked.Add(new Vector2Int(6, 4));
            _permBlocked.Add(new Vector2Int(7, 4));
            _permBlocked.Add(new Vector2Int(6, 5));
            _permBlocked.Add(new Vector2Int(7, 5));
            _permBlocked.Add(new Vector2Int(6, 6));
            _permBlocked.Add(new Vector2Int(7, 6));
            _permBlocked.Add(new Vector2Int(6, 7));
            _permBlocked.Add(new Vector2Int(7, 7));
            _permBlocked.Add(new Vector2Int(6, 8));
            _permBlocked.Add(new Vector2Int(7, 8));
            
            _permBlocked.Add(new Vector2Int(7, 9));
            _permBlocked.Add(new Vector2Int(7, 10));
            _permBlocked.Add(new Vector2Int(8, 8));
            _permBlocked.Add(new Vector2Int(8, 9));
            _permBlocked.Add(new Vector2Int(8, 10));
            _permBlocked.Add(new Vector2Int(9, 8));
            _permBlocked.Add(new Vector2Int(9, 9));
            _permBlocked.Add(new Vector2Int(9, 10));
            _permBlocked.Add(new Vector2Int(10, 8));
            _permBlocked.Add(new Vector2Int(10, 9));
            _permBlocked.Add(new Vector2Int(10, 10));
            _permBlocked.Add(new Vector2Int(11, 8));
            _permBlocked.Add(new Vector2Int(11, 9));
            _permBlocked.Add(new Vector2Int(11, 10));
            _permBlocked.Add(new Vector2Int(12, 8));
            _permBlocked.Add(new Vector2Int(12, 9));
            _permBlocked.Add(new Vector2Int(12, 10));
            _permBlocked.Add(new Vector2Int(13, 8));
            _permBlocked.Add(new Vector2Int(13, 9));
            _permBlocked.Add(new Vector2Int(13, 10));
            _permBlocked.Add(new Vector2Int(14, 8));
            _permBlocked.Add(new Vector2Int(14, 9));
            _permBlocked.Add(new Vector2Int(14, 10));
            _permBlocked.Add(new Vector2Int(15, 8));
            _permBlocked.Add(new Vector2Int(15, 9));
            _permBlocked.Add(new Vector2Int(15, 10));
            _permBlocked.Add(new Vector2Int(16, 8));
            _permBlocked.Add(new Vector2Int(16, 9));
            _permBlocked.Add(new Vector2Int(16, 10));
            _permBlocked.Add(new Vector2Int(17, 8));
            _permBlocked.Add(new Vector2Int(17, 9));
            _permBlocked.Add(new Vector2Int(17, 10));
            _permBlocked.Add(new Vector2Int(18, 8));
            _permBlocked.Add(new Vector2Int(18, 9));
            _permBlocked.Add(new Vector2Int(18, 10));


            var position = transform.position;
            var localScale = transform.localScale;
            var length = (int) localScale.z;
            var width = (int) localScale.x;

            _turretPreview = Instantiate(turretPreviewPrefab);
            _farmPreview = Instantiate(farmPreviewPrefab);
            _lumberMillPreview = Instantiate(lumberMillPreviewPrefab);

            DeactivatePreviews();

            _nodes = new Node[width * length];
            _isActive = false;

            for (int y = 0, i = 0; y < length; y++)
            {
                for (var x = 0; x < width; x++, i++)
                {
                    var node = Instantiate(nodePrefab, transform, true);

                    var posX = position.x + x * node.Size;
                    var posZ = position.z + y * node.Size + node.Size;

                    var (neighborsTier1, neighborsTier2) = FindNeighborsFor(node, x, y);
                    var index = new Vector2Int(x, y);
                    node.Setup(economyManager,index, posX, posZ, neighborsTier1, neighborsTier2, _isActive, true, _permBlocked.Contains(index));
                    _nodes[i] = node;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position + transform.localScale * nodePrefab.Size / 2,
                transform.localScale * nodePrefab.Size);
        }

        private (List<Node>, List<Node>) FindNeighborsFor(Node node, int x, int y)
        {
            var (placesTier1, placesTier2) = PreparePlaces(x, y);
            var neighborsTier1 = new List<Node>();
            var neighborsTier2 = new List<Node>();
            foreach (var item in _nodes)
            {
                if (item == null)
                {
                    continue;
                }

                if (Array.Exists(placesTier1, place => place == item.Index))
                {
                    neighborsTier1.Add(item);
                    item.NeighborsTier1.Add(node);
                }
                else if (Array.Exists(placesTier2, place => place == item.Index))
                {
                    neighborsTier2.Add(item);
                    item.NeighborsTier2.Add(node);
                }
            }

            return (neighborsTier1, neighborsTier2);
        }

        private static (Vector2Int[], Vector2Int[]) PreparePlaces(int x, int y)
        {
            var placesTier1 = new Vector2Int[4];
            var placesTier2 = new Vector2Int[8];

            var tier1Count = 0;
            var tier2Count = 0;
            for (var k = -2; k <= 0; k++)
            {
                var maxN = k == 0 ? -1 : 2;
                for (var n = -2; n <= maxN; n++)
                {
                    var place = new Vector2Int(x + n, y + k);
                    if (IsTier1(n, k))
                    {
                        placesTier1[tier1Count++] = place;
                    }
                    else
                    {
                        placesTier2[tier2Count++] = place;
                    }
                }
            }

            return (placesTier1, placesTier2);
        }

        private static bool IsTier1(int x, int y)
            => Mathf.Abs(y) == 1 && Mathf.Abs(x) == 1 ||
               y == 0 && Mathf.Abs(x) == 1 ||
               Mathf.Abs(y) == 1 && x == 0;


        public void Activate()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
            ActivateDeactivateNodes();
        }

        public void Deactivate()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;

            ActivateDeactivateNodes();
            DeactivatePreviews();
        }

        private void ActivateDeactivateNodes()
        {
            Array.ForEach(_nodes, node => node.SetActive(_isActive));
        }

        private void DeactivatePreviews()
        {
            _turretPreview.gameObject.SetActive(false);
            _farmPreview.gameObject.SetActive(false);
            _lumberMillPreview.gameObject.SetActive(false);
        }

        public void SelectStructureToBuild(StructureType? type)
        {
            DeactivatePreviews();

            if (type == null)
            {
                return;
            }

            var previewPrefab = type switch
            {
                StructureType.Turret => _turretPreview,
                StructureType.Farm => _farmPreview,
                StructureType.LumberMill => _lumberMillPreview,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            previewPrefab.gameObject.SetActive(true);

            Array.ForEach(_nodes, node => node.SetPreview(previewPrefab));
        }
    }
}