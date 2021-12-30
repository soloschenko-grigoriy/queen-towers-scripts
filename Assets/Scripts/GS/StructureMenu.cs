using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GS
{
    public class StructureMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI attackUIValue;
        [SerializeField] private TextMeshProUGUI healthUIValue;
        [SerializeField] private TextMeshProUGUI foodUIValue;
        [SerializeField] private TextMeshProUGUI woodUIValue;
        [SerializeField] private Transform foodUI;
        [SerializeField] private Transform woodUI;
        [SerializeField] private Transform attackUI;
        [SerializeField] private UIAudioManager audioManager;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Transform overlay;
        [SerializeField] private Image previewImg;
        
        private Structure _currentStructure;
        private bool _isShown;

        public void Show(Structure structure, IAttacker attacker, Resource resource)
        {
            if (_isShown)
            {
                return;
            }

            Level.PauseGame();
            
            UpdateTitle(structure);
            UpdateHealthText(structure);
            UpdateAttackText(attacker);
            UpdateResourceTextAndUI(resource, ResourceType.Food, foodUI, foodUIValue);
            UpdateResourceTextAndUI(resource, ResourceType.Wood, woodUI, woodUIValue);
            UpdatePreview(structure);
            
            _currentStructure = structure;
            
            gameObject.SetActive(true);
            overlay.gameObject.SetActive(true);
        }

        private void UpdatePreview(Structure structure)
        {
            previewImg.sprite = structure.PreviewImg;
        }

        private static void UpdateResourceTextAndUI(Resource resource, ResourceType type, Component container, TMP_Text text)
        {
            if (resource == null || resource.Type != type)
            {
                container.gameObject.SetActive(false);
                return;
            }

            text.text = $"+{resource.YieldsPerSecond}/s";
            container.gameObject.SetActive(true);
        }

        private void UpdateAttackText(IAttacker attacker)
        {
            if (attacker == null)
            {
                return;
            }

            attackUIValue.text = attacker.Attack.ToString();
            attackUI.gameObject.SetActive(true);
        }

        private void UpdateHealthText(Structure structure)
        {
            healthUIValue.text = $"{structure.Health}/{structure.HealthMax}";
        }

        private void UpdateTitle(Structure structure)
        {
            title.text = structure.Type switch
            {
                StructureType.Farm => "Farm",
                StructureType.LumberMill => "Lumber Mill",
                StructureType.Turret => "Archer Tower",
            };
        }

        public void Disassemble()
        {
            audioManager.PlayBtnClickSfx();

            if (_currentStructure == null)
            {
                return;
            }

            _currentStructure.Disassemble();
            Hide();
        }

        public void Hide()
        {
            audioManager.PlayBtnClickSfx();

            gameObject.SetActive(false);
            overlay.gameObject.SetActive(false);
            attackUI.gameObject.SetActive(false);
            _isShown = false;
            _currentStructure = null;

            Level.ResumeGame();
        }
    }
}