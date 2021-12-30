using UnityEngine;

namespace GS.Helpers
{
    public class Billboard : MonoBehaviour
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            transform.LookAt(transform.position + _cam.transform.forward);
        }
    }
}