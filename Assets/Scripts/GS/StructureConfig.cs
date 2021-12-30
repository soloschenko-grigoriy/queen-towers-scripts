using UnityEngine;

namespace GS
{
    [CreateAssetMenu(fileName = "Structure Configuration")]
    public class StructureConfig : ScriptableObject
    {
        [SerializeField] private StructureType type;
        [SerializeField, Range(10, 1000)] private int healthCapacity = 1000;
        [SerializeField] private StructureSize size;
        [SerializeField] private float constructionTime = 10f;
        [SerializeField] private int costWood;
        [SerializeField] private int costFood;
        [SerializeField] private Sprite previewImg;

        public StructureType Type => type;
        public StructureSize Size => size;
        public int HealthCapacity => healthCapacity;
        public int CostWood => costWood;
        public int CostFood => costFood;
        public float ConstructionTime => constructionTime;
        public Sprite PreviewImg => previewImg;
    }
}