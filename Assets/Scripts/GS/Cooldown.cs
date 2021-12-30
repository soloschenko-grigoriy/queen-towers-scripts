using System;
using UnityEngine;

namespace GS
{
    public class Cooldown : MonoBehaviour
    {
        [SerializeField, Range(1, 99)] private int time = 10;
        [SerializeField] private CooldownUI ui;
        
        private float _elapsed;
        private bool _inProgress;
        private Action _onDone;
        private int _remaining;

        private void Update()
        {
            if (!_inProgress)
            {
                return;
            }

            ui.SetValue(_remaining);

            if (_remaining <= 0)
            {
                _inProgress = false;
                _onDone?.Invoke();
                return;
            }

            _elapsed += Time.deltaTime;
            if (_elapsed < 1f)
            {
                return;
            }

            _elapsed %= 1f;
            --_remaining;
        }

        public void StartCounting(Action onDone)
        {
            this._onDone = onDone;
            _elapsed = 0;
            _remaining = time;
            _inProgress = true;
        }
    }
}