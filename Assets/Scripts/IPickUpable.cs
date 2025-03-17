using System;

public interface IPickUpable
{
    public event Action OnPickedUp;

    public void TryPickUp();
}
