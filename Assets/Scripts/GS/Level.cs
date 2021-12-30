using System;
using UnityEngine;

namespace GS
{
    [RequireComponent(typeof(Cooldown))]
    public class Level : MonoBehaviour
    {
        [SerializeField] private int numberOfWaves = 3;
        [SerializeField] private SpawnPoint[] spawnPoints;
        [SerializeField] private Flag[] flags;
        [SerializeField] private LevelStateUI ui;
        [SerializeField] private NodeGrid grid;
        [SerializeField] private AudioManager audioManager;

        private int _totalEnemies;
        private int _capturedFlags;
        private Cooldown _cooldown;
        private LevelState _state;
        private int _currentWave;
        
        private void Awake()
        {
            _currentWave = 0;
            _cooldown = GetComponent<Cooldown>();
            Flag.CapturedEvent += OnFlagCaptured;
            Enemy.DeathEvent += OnEnemyDie;
            LevelStateUI.LevelStateUIModalIsShownEvent += OnModalToggle;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                StopConstruction();
            }
        }

        private void Start()
        {
            StartCooldown();
        }

        private void OnDestroy()
        {
            Flag.CapturedEvent -= OnFlagCaptured;
            Enemy.DeathEvent -= OnEnemyDie;
            LevelStateUI.LevelStateUIModalIsShownEvent -= OnModalToggle;
        }

        private void StartRound()
        {
            StopConstruction();

            _state = LevelState.InProgress;
            _capturedFlags = 0;
            _totalEnemies = 0;

            // support only one flag for now
            var nearestFlag = flags[0];
            Array.ForEach(flags, flag => flag.Reset());
            Array.ForEach(spawnPoints, point =>
            {
                if (point.isActiveAndEnabled)
                {
                    _totalEnemies += point.GetAmountForThisRound(_currentWave);
                    point.StartSpawning(nearestFlag, _currentWave);
                }
            });

            audioManager.PlayBattleMusic();
        }

        private void OnFlagCaptured()
        {
            if (_state != LevelState.InProgress)
            {
                return;
            }

            if (++_capturedFlags == flags.Length)
            {
                RoundLost();
            }
        }

        private void OnEnemyDie()
        {
            if (_state != LevelState.InProgress)
            {
                return;
            }

            if (--_totalEnemies == 0)
            {
                RoundWon();
            }
        }
        
        private void RoundWon()
        {
            _state = ++_currentWave == numberOfWaves ? LevelState.Victory : LevelState.Won;
            
            Array.ForEach(spawnPoints, point => point.Reset());

            ui.ShowModal(_state, _currentWave == numberOfWaves - 1);
            audioManager.PlayVictoryMusic();
        }

        private void RoundLost()
        {
            _state = LevelState.Lost;
            Array.ForEach(spawnPoints, point => point.Reset());
            Debug.Log("Level lost");

            audioManager.PlayLostMusic();
            ui.ShowModal(_state, false);
        }

        public void StartCooldown()
        {
            _cooldown.StartCounting(StartRound);
            audioManager.PlayCooldownMusic();
        }

        public void StartConstruction(StructureType type)
        {
            grid.Activate();
            grid.SelectStructureToBuild(type);
        }

        private void StopConstruction()
        {
            grid.Deactivate();
            grid.SelectStructureToBuild(null);
        }

        private static void OnModalToggle(bool isModalShown)
        {
            if (isModalShown)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        public static void PauseGame()
        {
            Time.timeScale = 0;
        }

        public static void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}