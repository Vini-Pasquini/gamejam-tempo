using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState[] _states;

    private EState _currentState = EState.Idle;

    private void Start()
    {
        this._states = new IState[]
        {
            new Idle(this.transform),
            new Chase(this.transform),
            new Attack(),
        };
    }

    private void Update()
    {
        EState nextState = this._states[(int)this._currentState].OnUpdate();

        if (this._currentState != nextState)
        {
            this._states[(int)this._currentState].OnExit();
            this._currentState = nextState;
            this._states[(int)this._currentState].OnEnter();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this._states[(int)this._currentState].OnTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this._states[(int)this._currentState].OnTriggerExit(other);
        }
    }
}
