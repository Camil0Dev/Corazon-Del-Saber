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
        Debug.Log("🔥🔥🔥 ATTACKSTATE: ENTRANDO 🔥🔥🔥");
        
        _controller.Animator.SetBool("IsDormant", false);
        _controller.Animator.SetBool("IsAwake", false);
        _controller.Animator.SetBool("IsIdle", false);
        _controller.Animator.SetBool("IsWalking", false);
        _controller.Animator.SetBool("IsAttacking", true);
        
        _controller.Movement.StopMoving();
        _timer = 0;
        
        // 🔹 FORZAR LA ANIMACIÓN DE ATAQUE
        _controller.ForceAnimation("Attack", 0f);
        
        // 🔹 EL DAÑO SE APLICA DESDE EL ANIMATION EVENT
        Debug.Log("⚔️ Estado: ATTACK - Esperando Animation Event para aplicar daño");
        _controller.DebugAnimatorParameters();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        // Debug.Log($"⏱️ AttackState: timer = {_timer:F2} / {_controller.Data.attackDuration}");

        if (_timer >= _controller.Data.attackDuration)
        {
            Debug.Log("⏰ Tiempo de ataque completado. Decidiendo siguiente estado...");

            if (_controller.Trigger == null)
            {
                Debug.LogError("❌ Trigger es NULL en AttackState");
                _machine.ChangeState<IdleState>();
                return;
            }

            if (_controller.Trigger.IsPlayerDetected)
            {
                if (_controller.IsPlayerInAttackRange())
                {
                    Debug.Log("⚔️ Jugador aún en rango. Atacando de nuevo.");
                    _machine.ChangeState<AttackState>();
                }
                else
                {
                    Debug.Log("🚶 Jugador fuera de rango. Persiguiendo.");
                    _machine.ChangeState<WalkState>();
                }
            }
            else
            {
                Debug.Log("🟡 Jugador perdido. Volviendo a Idle.");
                _machine.ChangeState<IdleState>();
            }
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsAttacking", false);
        Debug.Log("⚔️ Saliendo de ATTACK - IsAttacking = false");
    }
}