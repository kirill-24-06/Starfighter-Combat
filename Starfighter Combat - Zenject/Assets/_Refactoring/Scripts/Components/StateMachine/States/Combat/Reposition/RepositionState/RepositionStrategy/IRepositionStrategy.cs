using UnityEngine;

namespace Refactoring
{
    public interface IRepositionStrategy
    {
        Vector3 GetRepositionDirection();
    }
}