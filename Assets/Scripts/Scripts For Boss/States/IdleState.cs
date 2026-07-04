using UnityEngine;

public class IdleState : IBossState
{
    private BossStateMachine _machine;
    private BossController _controller;

    public IdleState(BossStateMachine machine, BossController controller)
    {
        _machine = machine;
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Animator.SetBool("IsIdle", true);
        _controller.Movement.StopMoving();
    }

    public void Update()
    {
        if (_controller.Trigger.IsPlayerDetected)
        {
            if (_controller.IsPlayerInAttackRange())
            {
                _machine.ChangeState<AttackState>();
            }
            else
            {
                _machine.ChangeState<WalkState>();
            }
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsIdle", false);
    }
}