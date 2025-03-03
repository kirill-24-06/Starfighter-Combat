namespace Utils.SpawnSystem
{
    public interface IFactory<T>
    {
        public T Create();
    }

}
