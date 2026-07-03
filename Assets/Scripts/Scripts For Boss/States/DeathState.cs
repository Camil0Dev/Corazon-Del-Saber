using UnityEngine;

public class DeathState : IBossState
{
    private BossStateMachine _machine;
    private BossController _controller;

    public DeathState(BossStateMachine machine, BossController controller)
    {
        _machine = machine;
        _controller = controller;
    }

    public void Enter()
    {
        _controller.Animator.SetBool("IsDead", true);
        _controller.Movement.StopMoving();
        // Opcional: Desactivar colliders
        // _controller.GetComponent<Collider2D>().enabled = false;
        Debug.Log("💀 Boss derrotado");
    }

    public void Update() { }
    public void FixedUpdate() { }
    public void Exit() { }
}