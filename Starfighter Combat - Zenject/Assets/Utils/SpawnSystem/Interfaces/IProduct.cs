using System;
using UnityEngine;


namespace Utils.SpawnSystem
{
    public interface IProduct<T> where T : MonoBehaviour
    {
        public T OnGet();
        public T OnRelease();
        public T WithRelease(Action<T> returnAction);
    }

}
