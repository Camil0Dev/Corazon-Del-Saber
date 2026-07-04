using UnityEngine;

public class WalkState : IBossState
{
    private BossStateMachine _machine;
    private BossController _controller;

    public WalkState(BossStateMachine machine, BossController controller)
    {
        _machine = machine;
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Animator.SetBool("IsWalking", true);
    }

    public void Update()
    {
        if (!_controller.Trigger.IsPlayerDetected)
        {
            _machine.ChangeState<IdleState>();
            return;
        }

        if (_controller.IsPlayerInAttackRange())
        {
            _machine.ChangeState<AttackState>();
        }
    }

    public void FixedUpdate()
    {
        // El movimiento físico DEBE ir en FixedUpdate
        if (_controller.Player != null && _controller.Trigger.IsPlayerDetected && !_controller.IsPlayerInAttackRange())
        {
            _controller.Movement.MoveTo(_controller.Player.position);
        }
    }

    public void Exit()
    {
        _controller.Animator.SetBool("IsWalking", false);
        _controller.Movement.StopMoving();
    }
}