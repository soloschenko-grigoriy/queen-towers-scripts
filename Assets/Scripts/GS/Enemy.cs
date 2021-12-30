using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace GS
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyConfig configuration;
        [SerializeField] private EnemyAudioManager audioManager;
        [SerializeField] private Transform hitPoint;
        [SerializeField] private ParticleSystem bloodFx;

        public static event UnityAction DeathEvent;

        public float Health { get; private set; }
        public float HealthMax => configuration.HealthCapacity;
        public Transform HitPoint => hitPoint;

        private static readonly int AnimatorRun = Animator.StringToHash("Run");
        private static readonly int AnimatorDamage = Animator.StringToHash("DamageA");
        private static readonly int AnimatorDeathA = Animator.StringToHash("DeathA");
        private static readonly int AnimatorDeathB = Animator.StringToHash("DeathB");
        private static readonly int AnimatorAttackA = Animator.StringToHash("AttackA");
        private static readonly int AnimatorAttackB = Animator.StringToHash("AttackB");

        private NavMeshAgent _agent;
        private Animator _animator;
        private Structure _nearestThreat;
        private EnemyState _state;
        private Flag _target;

        private void Awake()
        {
            Health = configuration.HealthCapacity;
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _agent.isStopped = true;
        }

        private void Update()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            if (_agent.isStopped)
            {
                return;
            }

            if (_agent.pathPending)
            {
                return;
            }

            if (_agent.remainingDistance >= _agent.stoppingDistance)
            {
                return;
            }

            if (_agent.hasPath && _agent.velocity.sqrMagnitude != 0f)
            {
                return;
            }

            _agent.isStopped = true;

            if (_state == EnemyState.MovingToFlag)
            {
                StartCoroutine(CaptureTheFlagIfNotYet());
            }
            else if (_state == EnemyState.MovingToStructure)
            {
                StartCoroutine(Attack());
            }
        }

        public void Spawn(Vector3 spawnPoint, Flag target)
        {
            Health = configuration.HealthCapacity;

            transform.position = spawnPoint;
            gameObject.SetActive(true);

            this._target = target;
            StartMovingTowardsFlag();
        }

        public void Cleanup()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            audioManager.StopPlayingAll();
            _agent.isStopped = true;
            _target = null;
            _nearestThreat = null;
            gameObject.SetActive(false);
        }

        private IEnumerator Attack()
        {
            if (!_nearestThreat.isActiveAndEnabled)
            {
                StartMovingTowardsFlag();
                yield break;
            }

            LookTowardsTarget(_nearestThreat.transform.position);
            RunAttackAnimation();
            yield return new WaitForSeconds(1f);
            StopAttackAnimation();
            audioManager.PlayAttackSound();

            _nearestThreat.TakeDamage(configuration.Attack);

            yield return new WaitForSeconds(1f);
            StartCoroutine(Attack());
        }

        public void TakeDamage(float value, Structure from)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            Health -= value;
            bloodFx.Play();

            if (Health <= 0)
            {
                Health = 0;
                StartCoroutine(Die());
                _state = EnemyState.Dying;
                return;
            }

            // if attacking or going to attack, ignore
            if (_state == EnemyState.MovingToStructure || _state == EnemyState.Attacking)
            {
                return;
            }

            _nearestThreat = from;
            StartMovingTowardsThreat();
        }

        private void StartMovingTowardsFlag()
        {
            _agent.SetDestination(_target.transform.position);
            _agent.isStopped = false;
            _state = EnemyState.MovingToFlag;
            _animator.SetBool(AnimatorRun, true);
        }

        private void StartMovingTowardsThreat()
        {
            _state = EnemyState.MovingToStructure;
            _agent.SetDestination(_nearestThreat.transform.position);
            _agent.isStopped = false;
            _animator.SetBool(AnimatorRun, true);
        }

        private IEnumerator Die()
        {
            if (_state == EnemyState.Dying)
            {
                yield break;
            }

            StopAttackAnimation();
            _animator.SetBool(AnimatorRun, false);

            _animator.SetBool(Random.Range(0, 1) > 0.5 ? AnimatorDeathA : AnimatorDeathB, true);
            audioManager.PlayDeathSound();
            yield return new WaitForSeconds(2f);

            DeathEvent?.Invoke();
            Cleanup();
        }

        private IEnumerator CaptureTheFlagIfNotYet()
        {
            if (_target.IsCaptured)
            {
                yield break;
            }

            LookTowardsTarget(_target.transform.position);
            RunAttackAnimation();
            yield return new WaitForSeconds(1f);
            StopAttackAnimation();
            _target.Capture();
        }

        private void LookTowardsTarget(Vector3 target)
        {
            var dir = target - transform.position;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        private void RunAttackAnimation()
        {
            _animator.SetBool(AnimatorRun, false);
            _animator.SetBool(Random.Range(0, 1) > 0.5 ? AnimatorAttackA : AnimatorAttackB, true);
        }

        private void StopAttackAnimation()
        {
            _animator.SetBool(AnimatorAttackA, false);
            _animator.SetBool(AnimatorAttackB, false);
        }
    }
}