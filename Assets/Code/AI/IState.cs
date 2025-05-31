using UnityEngine;

public interface IState
{
    public void OnEnter();
    public EState OnUpdate();
    public void OnExit();

    public void OnTriggerEnter(Collider other);
    public void OnTriggerExit(Collider other);
}
