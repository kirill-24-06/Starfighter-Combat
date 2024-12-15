using UnityEngine;
using Zenject;

public class ForceFieldBehaviour : MonoBehaviour
{
    private EventManager _events;

    public bool ShieldActive { get; private set; }

    [Inject]
    public void Construct(EventManager events)
    {
        _events = events;
        _events.ForceField += Handle;
    }

    private void Start() => gameObject.SetActive(false);

    private void Handle(bool value)
    {
        if (value)
            OnActivated();

        else
            OnDeactivated();
    }

    private void OnActivated()
    {
        gameObject.SetActive(true);
        _events.Invunerable?.Invoke(true);
        ShieldActive = true;
    }

    private void OnDeactivated()
    {
        gameObject.SetActive(false);
        _events.Invunerable?.Invoke(false);

        ShieldActive = false;
    }
    
    private void OnDestroy()
    {
        _events.ForceField -= Handle;
    }
}