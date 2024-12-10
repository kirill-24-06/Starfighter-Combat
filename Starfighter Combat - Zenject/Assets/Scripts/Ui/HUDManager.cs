using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TimeBar _bonusTimer;
    [SerializeField] private GameObject _bossHealthBarText;
    [SerializeField] private Slider _bossHealthBarSlider;

    private IResetable _bossHealthBar;

    public void Initialise()
    {
        EntryPoint.Instance.Events.ChangeScore += UpdateScore;
        EntryPoint.Instance.Events.Pause += OnPause;

        _pauseButton.onClick.AddListener(Pause);

        _bonusTimer.Initialise(EntryPoint.Instance.Player.BonusTimer);

        _bossHealthBar = new BossHealthBar(_bossHealthBarText, _bossHealthBarSlider);

    }

    private void OnDestroy()
    {
        EntryPoint.Instance.Events.ChangeScore -= UpdateScore;
        EntryPoint.Instance.Events.Pause -= OnPause;
        _bossHealthBar.Reset();

        _pauseButton.onClick.RemoveAllListeners();
    }

    private void UpdateScore(int newScore) => _scoreText.text = newScore.ToString();

    public void ActivateBonusTimer() => _bonusTimer.gameObject.SetActive(true); 

    private void Pause() => EntryPoint.Instance.GameController.PauseGame(true);
   
    private void OnPause(bool value) => _pauseButton.gameObject.SetActive(!value);   
}