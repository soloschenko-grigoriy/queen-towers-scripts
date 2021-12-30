using UnityEngine;

namespace GS
{
    public class ExitBtn : MonoBehaviour
    {
        [SerializeField] private UIAudioManager audioManager;

        public void OnClick()
        {
            audioManager.PlayBtnClickSfx();
            Application.Quit();
        }
    }
}