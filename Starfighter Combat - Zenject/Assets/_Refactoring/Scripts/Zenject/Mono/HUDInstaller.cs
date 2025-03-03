using Refactoring.Ui.DialogWindows;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Refactoring
{
    public class HUDInstaller : MonoInstaller
    {
        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TimeBar _bonusTimer;

        [Header("Boss Health Bar")]
        [SerializeField] private GameObject _bossText;
        [SerializeField] private Slider _bossHealthBar;

        [Header("Player Health Bar")]
        [SerializeField] private GameObject _heartsLayout;
        [SerializeField] private List<Image> _hearts;
        [SerializeField] private GameObject _heartsImageGO;
        [SerializeField] private TextMeshProUGUI _heartsTextDynamic;
        [SerializeField] private GameObject _heartsTextGO;

        [Header("Player Bomb Bar")]
        [SerializeField] private GameObject _bombsLayout;
        [SerializeField] private List<Image> _bombs;
        [SerializeField] private GameObject _bombsImageGO;
        [SerializeField] private TextMeshProUGUI _bombsTextDynamic;
        [SerializeField] private GameObject _bombsTextGO;

        [Header("Root Canvas")]
        [SerializeField] private UiRoot _root;

        public override void InstallBindings()
        {
            var healthBar = CreateHealthBar();
            var bombsBar = CreateBombsBar();
            var bossHealthBar = CreateBossHealthBar();

            var hud = new HUDManager(_scoreText, _pauseButton, _bonusTimer, bossHealthBar, healthBar, bombsBar);

            Container
                .BindInterfacesAndSelfTo<HUDManager>()
                .FromInstance(hud)
                .AsCached()
                .NonLazy();

            var dialogManager = new DialogManager(_root.transform);
            Container.Bind<DialogManager>().FromInstance(dialogManager).AsSingle().NonLazy();
        }

        private HealthBar CreateHealthBar()
        {
            return new HealthBar(_heartsLayout, _hearts, _heartsImageGO, _heartsTextDynamic, _heartsTextGO);
        }

        private BossHealthBar CreateBossHealthBar()
        {
            return new BossHealthBar(_bossText,_bossHealthBar);
        }

        private BombsBar CreateBombsBar()
        {
            return new BombsBar(_bombsLayout,_bombs,_bombsImageGO, _bombsTextDynamic, _bombsTextGO);
        }
    }

}
