public interface IFlyAble : IUpdateable
{
    public float Speed { get; }
    public float SpeedRotation { get; }
    public void SetSpeed(float newSpeed);
    public void SetSpeedRotation(float newSpeed);
}