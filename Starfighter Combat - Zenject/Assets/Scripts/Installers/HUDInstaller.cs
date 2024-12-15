using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HUDInstaller: MonoInstaller
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TimeBar _bonusTimer;

    [Header("Boss Health Bar")]
    [SerializeField] private GameObject _bossText;
    [SerializeField] private Slider _healthBar;

    [Header("Player Health Bar")]
    [SerializeField] private GameObject _heartsLayout;
    [SerializeField] private List<Image> _hearts;
    [SerializeField] private GameObject _heartsGO;
    [SerializeField] private TextMeshProUGUI _heartsTextDynamic;
    [SerializeField] private GameObject _heartsTextGO;

    [Header("Player Bomb Bar")]
    [SerializeField] private GameObject _bombsLayout;
    [SerializeField] private List<Image> _bombs;
    [SerializeField] private GameObject _bombsGO;
    [SerializeField] private TextMeshProUGUI _bombsTextDynamic;
    [SerializeField] private GameObject _bombsTextGO;

    [Header("Root Canvas")]
    [SerializeField] private UiRoot _root;

    public override void InstallBindings()
    {
        var healtElements = new ImageBarElements()
        {
            ElementsLayout = _heartsLayout,
            Elements = _hearts,
            TextGO = _heartsGO,
            ElementsTextDynamic = _heartsTextDynamic,
            ElementsTextGO = _heartsTextGO,
        };

        var bombElements = new ImageBarElements()
        {
            ElementsLayout = _bombsLayout,
            Elements = _bombs,
            TextGO = _bombsGO,
            ElementsTextDynamic = _bombsTextDynamic,
            ElementsTextGO = _bombsTextGO,
        };

        var hudEl = new HUDElements()
        {
            ScoreText = _scoreText,
            PauseButton = _pauseButton,
            BonusTimer = _bonusTimer,

            BossHealthBar = new(_bossText, _healthBar),
            HealthBar = new HealthBar(healtElements),
            BombsBar = new BombsBar(bombElements),
        };

        Container.BindInterfacesAndSelfTo<HUDManager>().AsSingle().WithArguments(hudEl).NonLazy();
        Container.Bind<Transform>().FromInstance(_root.transform).WhenInjectedInto<EntryPoint>().NonLazy();
    }
}
