using UnityEngine;

public interface IState
{
    public void OnEnter();
    public EState OnUpdate();
    public void OnExit();
    //CUIUIU
    public void OnTriggerEnter(Collider other);
    public void OnTriggerExit(Collider other);
}
