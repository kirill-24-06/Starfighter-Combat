using UnityEngine;

public class MissileMover : IMover
{
    private readonly Transform _client;

    public MissileMover(Transform client)
    {
        _client = client;
    }

    public void Move(Vector3 direñtion, float speed)
    {
        Vector3 newDirection = (direñtion - _client.transform.position).normalized;

        _client.transform.position += speed * Time.deltaTime * newDirection;

        LookInTargetDirection(direñtion);
    }

    private void LookInTargetDirection(Vector3 target)
    {
        float rotation = Mathf.Atan2(target.y - _client.transform.position.y, target.x - _client.transform.position.x) * Mathf.Rad2Deg - 90;

        _client.transform.rotation = Quaternion.Euler(_client.transform.rotation.eulerAngles.x, _client.transform.rotation.eulerAngles.y, rotation);
    }
}
