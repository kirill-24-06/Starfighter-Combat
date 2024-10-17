using Cysharp.Threading.Tasks;
using UnityEngine;

public class TransitionStage : BossStage
{
    private TransitionStageData _data;
    private BossDefenceDrone[] _defenceDrones;
    private Spawner _spawner;

    private PolygonCollider2D _bossCollider;
    private Animator[] _engines;
    private SpriteRenderer _texture;
    private Color _stealthColor = new Color(1, 1, 1, 0f);

    private int _activeDronesCount;
    private int _activeDrones;
    private bool _alreadyStarted = false;

    public TransitionStage(TransitionStageData data)
    {
        _data = data;
    }

    public override BossStage Initialise(Boss boss)
    {
        _boss = boss;
        _bossCollider = _boss.GetComponent<PolygonCollider2D>();
        _texture = _boss.transform.Find("Texture").GetComponent<SpriteRenderer>();
        _defenceDrones = new BossDefenceDrone[_data.DefenceDronesAmount];
        _activeDrones = _data.DefenceDronesAmount;

        _engines = _boss.GetComponentsInChildren<Animator>();

        _spawner = EntryPoint.Instance.Spawner;

        return this;
    }
    public override void Handle()
    {
        OnStageStart();
        Attack();
    }

    public override bool CheckCompletion()
    {
        if (_activeDrones > 0)
            return false;

        else
        {
            _bossCollider.enabled = true;
            _boss.SetInvunrability(false);

            ColorChanger(Color.white).Forget();

            foreach (var engine in _engines)
            {
                engine.gameObject.SetActive(true);
            }

            return true;
        }
    }

    private void OnStageStart()
    {
        if (_alreadyStarted) return;

        _alreadyStarted = true;

        _bossCollider.enabled = false;
        _boss.SetInvunrability(true);

        ColorChanger(_stealthColor).Forget();

        foreach (var engine in _engines)
        {
            engine.gameObject.SetActive(false);
        }

        for (int i = 0; i < _defenceDrones.Length; i++)
        {
            _defenceDrones[i] = _spawner.SpawnEnemy(_data.BossDefenceDrone).GetComponent<BossDefenceDrone>().InitialiseByBoss();
        }
    }

    private async UniTaskVoid ColorChanger(Color targetColor)
    {
        float count = 0;

        while (count <= _data.ColorChangeDuration && !_boss.destroyCancellationToken.IsCancellationRequested)
        {
            _texture.color = Color.Lerp(_texture.color, targetColor, count/_data.ColorChangeDuration);
            count += Time.deltaTime;

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    private void Attack()
    {
        foreach (var drone in _defenceDrones)
        {
            if (drone.gameObject.activeInHierarchy)
            {
                _activeDronesCount++;
                drone.Handle();
            }
        }

        _activeDrones = _activeDronesCount;
        _activeDronesCount = 0;
    }
}
