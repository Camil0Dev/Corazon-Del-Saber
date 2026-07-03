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
        // Usamos SetTrigger para transiciones más controladas
        _controller.Animator.SetBool("IsDormant", true);
        _controller.Animator.SetBool("IsAwake", false);
        _controller.Animator.SetBool("IsIdle", false);
        _controller.Animator.SetBool("IsWalking", false);
        _controller.Animator.SetBool("IsAttacking", false);
        
        _controller.Movement.StopMoving();
        Debug.Log("💤 Estado: DORMANT");
    }

    public void Update()
    {
        if (_controller.Trigger != null && _controller.Trigger.IsPlayerDetected)
        {
            Debug.Log("🔊 ¡Jugador detectado! Despertando al boss...");
            _machine.ChangeState<AwakeState>();
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsDormant", false);
        Debug.Log("💤 Saliendo de DORMANT");
    }
}