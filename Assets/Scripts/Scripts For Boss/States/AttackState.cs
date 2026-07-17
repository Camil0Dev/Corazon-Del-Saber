using UnityEngine;

public class AttackState : IBossState
{
    private BossStateMachine _machine;
    private BossController _controller;
    private float _timer;

    public AttackState(BossStateMachine machine, BossController controller)
    {
        _machine = machine;
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Animator.SetBool("IsAttacking", true);
        _controller.Movement.StopMoving();
        _timer = 0;
        
        _controller.ForceAnimation("Attack", 0f);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _controller.Data.attackDuration)
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
            else
            {
                _machine.ChangeState<IdleState>();
            }
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsAttacking", false);
    }
}