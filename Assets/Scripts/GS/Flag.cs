using UnityEngine;
using UnityEngine.Events;

namespace GS
{
    public class Flag : MonoBehaviour
    {
        public static event UnityAction CapturedEvent;

        public bool IsCaptured { get; private set; }

        public void Reset()
        {
            IsCaptured = false;
            gameObject.SetActive(true);
        }

        public void Capture()
        {
            IsCaptured = true;
            gameObject.SetActive(false);
            CapturedEvent?.Invoke();
        }
    }
}