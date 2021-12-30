using UnityEngine;

namespace GS
{
    [CreateAssetMenu(fileName = "Turret Config")]
    public class TurretConfig : ScriptableObject
    {
        [SerializeField, Range(0.1f, 10f)] private float radius = 5f;
        [SerializeField, Range(0.01f, 10f)] private float cooldown = 0.1f;
        [SerializeField, Range(0.01f, 10f)] private float attack = 0.1f;
        [SerializeField, Range(1f, 50f)] private float projectileVelocity = 10f;
        [SerializeField] private Projectile projectile;

        public float Radius => radius;
        public float Cooldown => cooldown;
        public float Attack => attack;
        public float ProjectileVelocity => projectileVelocity;
        public Projectile Projectile => projectile;
    }
}