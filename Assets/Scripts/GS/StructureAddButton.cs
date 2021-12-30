using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GS
{
    public class StructureAddButton : MonoBehaviour
    {
        [SerializeField] private Level level;
        [SerializeField] private UIAudioManager audioManager;
        [SerializeField] private StructureConfig config;
        [SerializeField] private EconomyManager economyManager;
        [SerializeField] private TextMeshProUGUI costFood;
        [SerializeField] private TextMeshProUGUI costWood;
        [SerializeField] private Color notEnoughResourcesColor;
        [SerializeField] private Image icon;
        
        private Button _btn;
        
        private void Awake()
        {
            _btn = GetComponentInChildren<Button>();

            costFood.text = config.CostFood.ToString();
            costWood.text = config.CostWood.ToString();
        }

        private void Update()
        {
            _btn.interactable = EvaluateFoodQty() && EvaluateWoodQty();

            UpdateIconColor();
        }

        private void UpdateIconColor()
        {
            var iconColor = icon.color;
            iconColor.a = _btn.interactable ? 1.0f : 0.3f;
            icon.color = iconColor;
        }

        public void OnClick()
        {
            audioManager.PlayBtnClickSfx();

            level.StartConstruction(config.Type);
        }
        
        private bool EvaluateWoodQty()
        {
            if (!economyManager.IsEnoughWoodToBuild(config))
            {
                costWood.color = notEnoughResourcesColor;
                return false;
            }
            
            costWood.color = Color.white;
            return true;
        }

        private bool EvaluateFoodQty()
        {
            if (!economyManager.IsEnoughFoodToBuild(config))
            {
                costFood.color = notEnoughResourcesColor;
                return false;
            }
            
            costFood.color = Color.white;
            return true;
        }
    }
}