namespace Refactoring
{
    public interface IMissileBaseData : IProjectileBaseData
    {
        public float LaunchTime { get; set; }

        public float HomingTime { get; set; }
    }
}
