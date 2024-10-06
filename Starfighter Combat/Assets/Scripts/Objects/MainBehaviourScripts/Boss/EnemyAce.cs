public class EnemyAce : Boss
{
    private void Start()
    {
        EntryPoint.Instance.SpawnedObjects.RegisterObject(gameObject, ObjectTag.Enemy);
    }

    protected override void Disable()
    {
        Instantiate(_data.Explosion, transform.position, _data.Explosion.transform.rotation);

        EntryPoint.Instance.Events.BossDefeated?.Invoke();
        gameObject.SetActive(false);

        _audioPlayer.PlayOneShot(_data.ExplosionSound, _data.ExplosionSoundVolume);
    }
}