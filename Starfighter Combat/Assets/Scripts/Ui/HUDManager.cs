using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _maxScoreText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TimeBar _bonusTimer;

    public void Initialise()
    {
        EntryPoint.Instance.Events.ChangeScore += UpdateScore;
        EntryPoint.Instance.Events.BonusCollected += ActivateBonusTimer;
       
        _pauseButton.onClick.AddListener(Pause);

        _bonusTimer.Initialise(EntryPoint.Instance.Player.BonusTimer);
    }

    private void OnDestroy()
    {
        EntryPoint.Instance.Events.ChangeScore -= UpdateScore;
        EntryPoint.Instance.Events.BonusCollected -= ActivateBonusTimer;

        _pauseButton.onClick.RemoveAllListeners();
    }

    private void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }

    private void ActivateBonusTimer(BonusTag tag)
    {
        if (tag == BonusTag.Multilaser || tag == BonusTag.ForceField)
            _bonusTimer.gameObject.SetActive(true);
    }

    private void Pause()
    {
        EntryPoint.Instance.GameController.PauseGame(true);
    }
}
