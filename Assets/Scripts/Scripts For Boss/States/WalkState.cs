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
        _controller.Animator.SetBool("IsDormant", false);
        _controller.Animator.SetBool("IsAwake", false);
        _controller.Animator.SetBool("IsIdle", false);
        _controller.Animator.SetBool("IsWalking", true);
        _controller.Animator.SetBool("IsAttacking", false);
        
        Debug.Log("🔵 Estado: WALK - Animator IsWalking = true");
        _controller.DebugAnimatorParameters();
    }

    public void Update()
    {
        if (_controller.Trigger == null)
        {
            Debug.LogError("❌ Trigger es NULL en WalkState");
            return;
        }

        if (!_controller.Trigger.IsPlayerDetected)
        {
            Debug.Log("🔴 Jugador perdido en WalkState. Volviendo a Idle");
            _machine.ChangeState<IdleState>();
            return;
        }

        if (_controller.Player != null)
        {
            _controller.Movement.MoveTo(_controller.Player.position);
        }

        bool inAttackRange = _controller.IsPlayerInAttackRange();
        Debug.Log($"⚔️ WalkState: ¿Jugador en rango de ataque? {inAttackRange}");

        if (inAttackRange)
        {
            Debug.Log("⚔️ Jugador en rango de ataque. Cambiando a AttackState");
            _machine.ChangeState<AttackState>();
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsWalking", false);
        _controller.Movement.StopMoving();
        Debug.Log("🔵 Saliendo de WALK");
    }
}