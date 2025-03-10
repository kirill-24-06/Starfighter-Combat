using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;


namespace Refactoring
{
    public class SpawnController : IAwakeable, IDisposable
    {
        private SpawnerData _data;

        private Spawner _spawner;

        private CancellationTokenSource[] _stageTokens;
        private CancellationTokenSource _sceneExitToken;

        private int _currentStage = 0;

        private int _activeEnemies = 0;
        private int _activeEliteEnemies = 0;

        private bool _bonusIsActive = false;

        private bool _isGameActive;

        public SpawnController(Spawner spawner, int stagesAmount = 8)
        {
            _spawner = spawner;

            _stageTokens = new CancellationTokenSource[stagesAmount + 1];
            for (int i = 0; i < _stageTokens.Length; i++)
            {
                _stageTokens[i] = new CancellationTokenSource();
            }

            _sceneExitToken = new CancellationTokenSource();
        }

        public void Awake()
        {
            Utils.Events.Channel.Static.Channel<StartEvent>.OnEvent += OnStart;
            Utils.Events.Channel.Static.Channel<EnemyDestroyedEvent>.OnEvent += OnEnemyDestroyed;
            Utils.Events.Channel.Static.Channel<BonusTakenEvent>.OnEvent += OnBonusTaken;
            Utils.Events.Channel.Static.Channel<StopEvent>.OnEvent += OnStop;
            Utils.Events.Channel.Static.Channel<PlayerRespawnEvent>.OnEvent += RespawnPlayer;
        }

        private void OnStart(StartEvent @event) => _isGameActive = true;

        private void OnStop(StopEvent @event) => _isGameActive = false;

        public void OnNewStage(SpawnerData newData)
        {
            if (_currentStage != 0)
                CancelPreviousStage();

            _data = newData;

            UniTask
                .Delay(TimeSpan.FromSeconds(_data.SpawnDelay), cancellationToken: _sceneExitToken.Token)
                .ContinueWith(StartSpawnLoop)
                .Forget();
        }

        private void StartSpawnLoop()
        {
            if (_data.Enemies.Count > 0)
                SpawnLoop(SpawnEnemy, _data.SpawnTime, _stageTokens[_currentStage].Token).Forget();

            if (_data.EliteEnemies.Count > 0)
                SpawnLoop(SpawnEliteEnemy, _data.EliteSpawnTime, _stageTokens[_currentStage].Token).Forget();

            if (_data.Bonuses.Count > 0)
                SpawnLoop(SpawnBonus, _data.BonusSpawnTime, _stageTokens[_currentStage].Token).Forget();

            _currentStage++;
        }

        private async UniTaskVoid SpawnLoop(Action onSpawn, float spawnTime, CancellationToken cancellationToken)
        {
            onSpawn.Invoke();

            while (_isGameActive)
            {
                await UniTask
                    .Delay(TimeSpan.FromSeconds(spawnTime), cancellationToken: cancellationToken)
                    .ContinueWith(onSpawn);
            }
        }

        private void SpawnEnemy()
        {
            if (_activeEnemies >= _data.MaxEnemies)
                return;

            _spawner.SpawnUnit(_data.Enemies[Random.Range(0, _data.Enemies.Count)].Value);
            _activeEnemies++;
        }

        private void SpawnEliteEnemy()
        {
            if (_activeEnemies >= _data.MaxEnemies || _activeEliteEnemies >= _data.MaxHardEnemies)
                return;

            _spawner.SpawnUnit(_data.EliteEnemies[Random.Range(0, _data.EliteEnemies.Count)].Value);
            _activeEnemies++;
            _activeEliteEnemies++;
        }

        private void SpawnBonus()
        {
            if (_bonusIsActive)
                return;

            _spawner.SpawnUnit(_data.Bonuses[Random.Range(0, _data.Bonuses.Count)].Value);
            _bonusIsActive = true;
        }

        public void RespawnPlayer(PlayerRespawnEvent @event)
        {
            UniTask
                .Delay(TimeSpan.FromSeconds(2f), cancellationToken: _sceneExitToken.Token)
                .ContinueWith(() => _spawner.SpawnPlayer())
                .Forget();
        }

        private void OnEnemyDestroyed(EnemyDestroyedEvent @event)
        {
            if (@event.EnemyStrenght == EnemyStrenght.Hard)
            {
                _activeEnemies--;
                _activeEliteEnemies--;

                if (_activeEliteEnemies < 0)
                    _activeEliteEnemies = 0;
            }

            else if (@event.EnemyStrenght == EnemyStrenght.Basic)
                _activeEnemies--;

            if (_activeEnemies < 0)
                _activeEnemies = 0;
        }

        private void OnBonusTaken(BonusTakenEvent @event) => _bonusIsActive = false;

        public void BossArrival(BossWave bossWave, float delaySeconds)
        {
            CancelPreviousStage();

            UniTask
                .Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken: _sceneExitToken.Token)
                .ContinueWith(() =>
                {
                    _data = bossWave.SpawnerData;

                    foreach (var boss in bossWave.Bosses)
                        _spawner.SpawnUnit(boss.Value);

                    StartSpawnLoop();
                })
                .Forget();
        }

        private void CancelPreviousStage()
        {
            _stageTokens[_currentStage - 1].Cancel();
            _stageTokens[_currentStage - 1].Dispose();
        }

        public void Dispose()
        {
            for (int i = _currentStage; i < _stageTokens.Length; i++)
            {
                _stageTokens[i]?.Cancel();
                _stageTokens[i]?.Dispose();
            }

            _sceneExitToken?.Cancel();
            _sceneExitToken?.Dispose();

            Utils.Events.Channel.Static.Channel<StartEvent>.OnEvent -= OnStart;
            Utils.Events.Channel.Static.Channel<EnemyDestroyedEvent>.OnEvent -= OnEnemyDestroyed;
            Utils.Events.Channel.Static.Channel<BonusTakenEvent>.OnEvent -= OnBonusTaken;
            Utils.Events.Channel.Static.Channel<StopEvent>.OnEvent -= OnStop;
            Utils.Events.Channel.Static.Channel<PlayerRespawnEvent>.OnEvent -= RespawnPlayer;
        }
    }
}
