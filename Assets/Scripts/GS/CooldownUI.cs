using TMPro;
using UnityEngine;

namespace GS
{
    public class CooldownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI valueComp;

        public void SetValue(int value)
        {
            valueComp.text = value.ToString();
        }
    }
}