namespace Utils.SpawnSystem
{
    public interface ISettings<T>
    {
        public T Create();
        public void OnGet(T obj);
        public void OnRelease(T obj);

    }
}
