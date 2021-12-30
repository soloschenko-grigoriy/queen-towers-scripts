using System;
using UnityEngine;

namespace GS
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;

        [SerializeField] private ResourceUI woodUI;
        [SerializeField] private ResourceUI foodUI;

        private void Awake()
        {
            Resource.OnYield += OnResourceYield;
            Node.StartsBuildingStructure += OnStructureBuilt;
        }

        private void Start()
        {
            UpdateStatsUI();
        }

        private void OnDestroy()
        {
            Resource.OnYield -= OnResourceYield;
            Node.StartsBuildingStructure -= OnStructureBuilt;
        }

        public bool EvaluateBuildCostFor(Structure structure) =>
            playerStats.Wood >= structure.CostWood && playerStats.Food >= structure.CostFood;

        public bool IsEnoughWoodToBuild(StructureConfig config) => playerStats.Wood >= config.CostWood;
        public bool IsEnoughFoodToBuild(StructureConfig config) => playerStats.Food >= config.CostFood;
        
        private void OnResourceYield(ResourceType type, int amount)
        {
            switch (type)
            {
                case ResourceType.Food:
                    playerStats.Food += amount;
                    break;
                case ResourceType.Wood:
                    playerStats.Wood += amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            UpdateStatsUI();
        }
        
        private void OnStructureBuilt(Structure structure)
        {
            playerStats.Food -= structure.CostFood;
            playerStats.Wood -= structure.CostWood;

            UpdateStatsUI();
        }

        private void UpdateStatsUI()
        {
            woodUI.SetValue(playerStats.Wood);
            foodUI.SetValue(playerStats.Food);
        }
    }
}