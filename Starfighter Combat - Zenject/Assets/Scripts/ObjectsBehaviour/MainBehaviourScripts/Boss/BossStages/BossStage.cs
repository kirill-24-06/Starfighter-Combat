public abstract class BossStage
{
    protected Boss _boss;
    
    public abstract void Handle();
    public abstract bool CheckCompletion();

    public virtual BossStage Initialise(Boss boss)
    {
        _boss = boss;
        return this;
    }
}