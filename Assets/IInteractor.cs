using UnityEngine;

public interface IInteractor
{
    bool CanInteract { get; }
    void StartEvent();
}