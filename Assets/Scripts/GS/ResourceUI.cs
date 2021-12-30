using TMPro;
using UnityEngine;

namespace GS
{
    public class ResourceUI : MonoBehaviour
    {
        private TextMeshProUGUI _valueComp;

        private void Awake()
        {
            _valueComp = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetValue(int value)
        {
            _valueComp.text = value.ToString();
        }
    }
}