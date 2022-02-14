using System;

public interface IImmuneAble : IUpdateable
{
    public Action OnStopImmune { get; set; }
    public Action<float> OnStartImmune { get; set; }
    public void Init(float timeImmune);
    public void Immunity(float _timeImmune);
}
