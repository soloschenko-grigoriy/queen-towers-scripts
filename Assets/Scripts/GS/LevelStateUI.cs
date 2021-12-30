using UnityEngine;
using UnityEngine.Events;

namespace GS
{
    public class LevelStateUI : MonoBehaviour
    {
        [SerializeField] private RectTransform loseText;
        [SerializeField] private RectTransform winText;
        [SerializeField] private RectTransform victoryText;
        [SerializeField] private RectTransform finalWaveText;
        [SerializeField] private RectTransform overlay;
        public static event UnityAction<bool> LevelStateUIModalIsShownEvent;

        public void ShowModal(LevelState state, bool isFinalWave)
        {
            LevelStateUIModalIsShownEvent?.Invoke(true);

            RectTransform elm;
            
            if (isFinalWave)
            {
                elm = finalWaveText;
            }
            else
            {
                elm = state switch
                {
                    LevelState.Lost => loseText,
                    LevelState.Won => winText,
                    LevelState.Victory => victoryText,
                    _ => null
                };
            }

            if (elm == null)
            {
                return;
            }

            overlay.gameObject.SetActive(true);
            elm.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public void HideModal()
        {
            overlay.gameObject.SetActive(false);
            gameObject.SetActive(false);

            loseText.gameObject.SetActive(false);
            winText.gameObject.SetActive(false);
            victoryText.gameObject.SetActive(false);
            finalWaveText.gameObject.SetActive(false);

            LevelStateUIModalIsShownEvent?.Invoke(false);
        }
    }
}