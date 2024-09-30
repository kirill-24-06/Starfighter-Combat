using UnityEngine;

public class ForceFieldBehaviour : MonoBehaviour
{
    private EventManager _events;

    public void Initialise()
    {
        _events = EntryPoint.Instance.Events;

        _events.ForceField += OnActivated;
        _events.ForceFieldEnd += OnDeactivated;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _events.ForceField -= OnActivated;
        _events.ForceFieldEnd -= OnDeactivated;
    }

    private void OnActivated()
    {
        gameObject.SetActive(true);
        _events.Invunerable?.Invoke(true);
    }

    private void OnDeactivated()
    {
        gameObject.SetActive(false);
        _events.Invunerable?.Invoke(false);
    }
}