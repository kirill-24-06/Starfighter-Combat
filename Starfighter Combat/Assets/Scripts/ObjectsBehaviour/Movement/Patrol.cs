using UnityEngine;

public class Patrol : AdvancedMove
{
    private Bounds[] _patrolAreas = null;
    private Vector3[] _movePoints = null;
    private int _point = 0;

    public Patrol(Transform client, IMovableData movableData) : base(client, movableData)
    {    
        _patrolAreas = EntryPoint.Instance.PatrolArea;
        _movePoints = new Vector3[_patrolAreas.Length];
        NewMovePoints();
    }

    public override void SetNewDirection()
    {
        switch (_mover)
        {
            case Mover:

                _direction = Vector2.up;
                break;

            case AdvancedMover:

                UpdatePoints();
                LookInTargetDirection(_direction);
                break;
        }
    }

    private void UpdatePoints()
    {
        _direction = _movePoints[_point];
        _point = (_point == 1) ? 0 : 1;
    }

    public void NewMovePoints()
    {
        for (int i = 0; i < _movePoints.Length; i++)
        {
            _movePoints[i] = GenerateMovePoint(_patrolAreas[i]);
        }

        _point = Random.Range(0, _movePoints.Length);
    }

    private Vector3 GenerateMovePoint(Bounds area)
    {
        float randomX = Random.Range(area.min.x, area.max.x);
        float randomY = Random.Range(area.min.y, area.max.y);
        float Z = 0;

        return new Vector3(randomX, randomY, Z);
    }
}
