using UnityEngine;

namespace GS
{
    public class ResumeGameBtn : MonoBehaviour
    {
        [SerializeField] private UIAudioManager audioManager;
        [SerializeField] private Level level;
        [SerializeField] private LevelStateUI levelStateUI;

        public void OnClick()
        {
            audioManager.PlayBtnClickSfx();
            levelStateUI.HideModal();
            level.StartCooldown();
        }
    }
}