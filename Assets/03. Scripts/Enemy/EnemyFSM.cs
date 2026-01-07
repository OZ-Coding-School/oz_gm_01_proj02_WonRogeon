using UnityEngine;

//상태 전환의 절차를 보장하는 게 유일한 책임
public class EnemyFSM
{
    private IEnemyState currentState;

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
