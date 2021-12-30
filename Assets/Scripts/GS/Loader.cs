using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GS
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private Slider progressSlider;
        [SerializeField] private Button startBtn;

        private AsyncOperation loader;
        
        private void Start()
        {
            progressSlider.value = 0;
            progressSlider.gameObject.SetActive(true);
            startBtn.gameObject.SetActive(false);
            startBtn.onClick.AddListener(StartGame);
            
            StartCoroutine(LoadScene());
        }

        private void StartGame()
        {
            if (loader == null)
            {
                return;
            }
            
            loader.allowSceneActivation = true;
        }

        private IEnumerator LoadScene()
        {
            loader = SceneManager.LoadSceneAsync("Level_1");
            loader.allowSceneActivation = false;
            
            while (loader.progress < 0.9f)
            {
                Debug.Log(loader.progress);
                progressSlider.value = loader.progress;
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("Done");
            progressSlider.gameObject.SetActive(false);
            startBtn.gameObject.SetActive(true);
        }
    }
}