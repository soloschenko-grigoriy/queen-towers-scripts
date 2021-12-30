using System;
using UnityEditor;
using UnityEngine;

namespace GS
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class Turret : MonoBehaviour, IAttacker
    {
        [SerializeField] private TurretConfig configuration;
        [SerializeField] private StructureAudioManager audioManager;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Structure structure;
        
        private readonly Projectile[] _projectiles = new Projectile[10];
        private float _elapsed;
        private bool _recharging;
        private SphereCollider _sphereCollider;

        private Enemy _target;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = configuration.Radius;

            // init projectiles pool
            for (var i = 0; i < 10; i++)
            {
                _projectiles[i] = Instantiate(configuration.Projectile, transform.position, Quaternion.identity)
                    .Setup(structure, firePoint.position, configuration.ProjectileVelocity);
            }
        }

        private void Update()
        {
            if (!structure.IsReady)
            {
                return;
            }

            if (_target?.Health <= 0)
            {
                _target = null;
            }

            if (!_recharging && _target != null)
            {
                audioManager.PlayTurretAttackSfx();
                GetUnusedProjectile().Fire(_target, configuration.Attack);
                _recharging = true;
                return;
            }

            KeepRecharging();
        }

        private void OnDisable()
        {
            _target = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, configuration.Radius);

            if (_target != null)
            {
                Debug.DrawLine(transform.position, _target.transform.position, Color.blue);
            }
        }
#endif

        private void OnTriggerEnter(Collider other)
        {
            if (_target != null)
            {
                return;
            }

            if (other.CompareTag("Enemy"))
            {
                _target = other.GetComponent<Enemy>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_target == null)
            {
                return;
            }

            if (other.CompareTag("Enemy"))
            {
                _target = null;
            }
        }

        public float Attack => configuration.Attack;

        public void Toggle(bool value)
        {
            _sphereCollider.enabled = value;
        }

        private void KeepRecharging()
        {
            if (!_recharging)
            {
                return;
            }

            _elapsed += Time.deltaTime;
            if (_elapsed >= configuration.Cooldown)
            {
                _recharging = false;
                _elapsed = 0;
            }
        }

        private Projectile GetUnusedProjectile()
        {
            return Array.Find(_projectiles, projectile => !projectile.gameObject.activeSelf);
        }
    }
}