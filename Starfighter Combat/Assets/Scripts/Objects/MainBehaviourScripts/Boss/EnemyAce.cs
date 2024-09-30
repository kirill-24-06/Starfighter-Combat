public class EnemyAce : Boss
{
    private void Start()
    {
        EntryPoint.Instance.SpawnedObjects.RegisterObject(gameObject, ObjectTag.Enemy);
    }
    protected override void Disable()
    {
        EntryPoint.Instance.Events.BossDefeated?.Invoke();
        gameObject.SetActive(false);
    }
}