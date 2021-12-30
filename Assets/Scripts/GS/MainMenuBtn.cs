using UnityEngine;
using UnityEngine.SceneManagement;

namespace GS
{
    public class MainMenuBtn : MonoBehaviour
    {
        [SerializeField] private UIAudioManager audioManager;

        public void OnClick()
        {
            audioManager.PlayBtnClickSfx();
            Level.ResumeGame();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}