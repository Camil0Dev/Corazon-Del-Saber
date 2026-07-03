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
        _controller.Animator.SetBool("IsDormant", false);
        _controller.Animator.SetBool("IsAwake", false);
        _controller.Animator.SetBool("IsIdle", true);
        _controller.Animator.SetBool("IsWalking", false);
        _controller.Animator.SetBool("IsAttacking", false);
        
        _controller.Movement.StopMoving();
        Debug.Log("🟡 Estado: IDLE - Animator IsIdle = true");
        _controller.DebugAnimatorParameters();
    }

    public void Update()
    {
        if (_controller.Trigger == null)
        {
            Debug.LogError("❌ Trigger es NULL en IdleState");
            return;
        }

        // 🔹 LOG CADA FRAME PARA VER EL ESTADO DEL TRIGGER
        // Debug.Log($"🔄 IdleState: IsPlayerDetected = {_controller.Trigger.IsPlayerDetected}");

        if (_controller.Trigger.IsPlayerDetected)
        {
            Debug.Log("✅ Jugador detectado en IdleState");

            bool inAttackRange = _controller.IsPlayerInAttackRange();
            Debug.Log($"⚔️ ¿Jugador en rango de ataque? {inAttackRange}");

            if (inAttackRange)
            {
                Debug.Log("⚔️ Jugador en RANGO DE ATAQUE. Cambiando a AttackState");
                _machine.ChangeState<AttackState>();
            }
            else
            {
                Debug.Log("🚶 Jugador FUERA de rango de ataque. Cambiando a WalkState");
                _machine.ChangeState<WalkState>();
            }
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsIdle", false);
        Debug.Log("🟡 Saliendo de IDLE");
    }
}