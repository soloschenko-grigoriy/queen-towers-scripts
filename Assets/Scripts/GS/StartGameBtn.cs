using UnityEngine;
using UnityEngine.SceneManagement;

namespace GS
{
    public class StartGameBtn : MonoBehaviour
    {
        [SerializeField] private UIAudioManager audioManager;

        public void OnClick()
        {
            audioManager.PlayBtnClickSfx();
            SceneManager.LoadScene("Loading", LoadSceneMode.Single);
        }
    }
}