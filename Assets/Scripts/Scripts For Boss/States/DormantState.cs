using UnityEngine;

public class DormantState : IBossState
{
    private BossStateMachine _machine;
    private BossController _controller;

    public DormantState(BossStateMachine machine, BossController controller)
    {
        _machine = machine;
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Animator.SetBool("IsDormant", true);
        _controller.Movement.StopMoving();
    }

    public void Update()
    {
        if (_controller.Trigger != null && _controller.Trigger.IsPlayerDetected)
        {
            _machine.ChangeState<AwakeState>();
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsDormant", false);
    }
}