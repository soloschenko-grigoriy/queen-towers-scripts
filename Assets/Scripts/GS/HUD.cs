using UnityEngine;

namespace GS
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private StructureMenu structureMenu;

        private void Awake()
        {
            Structure.OnSelect += ShowStructureMenu;
        }

        private void OnDestroy()
        {
            Structure.OnSelect -= ShowStructureMenu;
        }

        private void ShowStructureMenu(Structure structure, IAttacker attacker, Resource resource)
        {
            Level.PauseGame();
            structureMenu.Show(structure, attacker, resource);
        }
    }
}