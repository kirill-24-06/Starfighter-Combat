using UnityEngine;

public enum AreaTag
{
    Upper,
    Left,
    Right,
    None
}

public class SpawnArea : MonoBehaviour
{
    private Bounds _spawnArea;

    public AreaTag Tag { get; private set; }

    public Quaternion Rotation { get; private set; }


    public void Initialise(string areaName)
    {
        _spawnArea = GetComponent<SpriteRenderer>().bounds;

        Tag = GlobalConstants.AreaTagsByName[areaName];
        Rotation = GlobalConstants.AreaRotationsByTag[Tag];
    }   

    public Vector3 GenerateSpawnPosition()
    {
        float randomX = Random.Range(_spawnArea.min.x, _spawnArea.max.x);
        float randomY = Random.Range(_spawnArea.min.y, _spawnArea.max.y);
        float Z = 0;

        return new Vector3(randomX, randomY, Z);

    }
}