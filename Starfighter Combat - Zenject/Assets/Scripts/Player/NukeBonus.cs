public class NukeBonus : IBonus
{
    private int _nukesAmount;
    private EventManager _events;

    public NukeBonus(EventManager events, int nukesAmount)
    {
        _nukesAmount = nukesAmount;
        _events = events;
    }

    public void Handle() => _events.NukesAdded?.Invoke(_nukesAmount);

}

