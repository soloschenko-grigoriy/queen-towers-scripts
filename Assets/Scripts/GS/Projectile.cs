using UnityEngine;

namespace GS
{
    public class Projectile : MonoBehaviour
    {
        private float _damage;
        private Structure _spawner;
        private Vector3 _spawnPoint;
        private Enemy _target;
        private float _velocity;

        private void Update()
        {
            var dest = _target.HitPoint.transform.position;
            var dir = dest - transform.position;
            var dist = (dest - transform.position).magnitude;
            transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * _velocity);
            transform.rotation = Quaternion.LookRotation(-dir, Vector3.up);

            if (dist < 1)
            {
                Hit();
            }
            else if (transform.position.y < 0)
            {
                Deactivate();
            }
        }

        public Projectile Setup(Structure spawner, Vector3 spawnPoint, float velocity)
        {
            this._spawner = spawner;
            this._velocity = velocity;
            this._spawnPoint = spawnPoint;

            transform.SetParent(spawner.transform, true);

            Deactivate();
            return this;
        }

        public void Fire(Enemy target, float damage)
        {
            this._target = target;
            this._damage = damage;

            transform.position = _spawnPoint;
            gameObject.SetActive(true);
        }

        private void Hit()
        {
            _target.TakeDamage(_damage, _spawner);
            Deactivate();
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);

            _target = null;
        }
    }
}