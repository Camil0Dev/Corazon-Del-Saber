using UnityEngine;

public class AwakeState : IBossState
{
    private BossStateMachine _machine;
    private BossController _controller;
    private float _timer;

    public AwakeState(BossStateMachine machine, BossController controller)
    {
        _machine = machine;
        _controller = controller;
    }

    public void Enter()
    {
        _controller.TriggerBossAwaken();
        _controller.Animator.SetBool("IsAwake", true);
        _controller.Movement.StopMoving();
        _timer = 0;
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= _controller.Data.awakeDuration)
        {
            _machine.ChangeState<IdleState>();
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsAwake", false);
    }
}