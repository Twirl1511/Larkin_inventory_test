using System;

public interface IPlaceable
{
    public event Action OnPlaced;

    public void TryPlace();
}
