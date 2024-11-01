using System.Collections.Generic;
using UnityEngine;

public class CollisionMap
{
    //collisions with player weapon
    private Dictionary<Collider2D, IInteractableEnemy> _interactables;

    //collisions with nuke overlap circle
    private Dictionary<Collider2D, INukeInteractable> _nukeInteractables;

    private List<Transform> _playerMissileTartgets;

    public Dictionary<Collider2D, IInteractableEnemy> Interactables => _interactables;

    public Dictionary<Collider2D, INukeInteractable> NukeInteractables => _nukeInteractables;

    public List<Transform> PlayerMissileTargets => _playerMissileTartgets;

    public CollisionMap()
    {
        _interactables = new Dictionary<Collider2D, IInteractableEnemy>();
        _nukeInteractables = new Dictionary<Collider2D, INukeInteractable>();
        _playerMissileTartgets = new List<Transform>();
    }

    public void Register(Collider2D collider, IInteractableEnemy interactable)
    {
        _interactables.Add(collider, interactable);
    }

    public void RegisterNukeInteractable(Collider2D collider, INukeInteractable interactable)
    {
        _nukeInteractables.Add(collider, interactable);
    }
    public void RegisterMissileTarget(Transform enemyTransform) => _playerMissileTartgets.Add(enemyTransform);
}