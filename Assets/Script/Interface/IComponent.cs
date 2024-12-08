using UnityEngine;

public interface IComponent
{
    public void Interact();
    string ComponentType { get; }
}
