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
        Debug.Log("🌅 Estado: AWAKE - Entrando");
        
        // 🔹 NOTIFICAR QUE EL BOSS HA DESPERTADO (¡AQUÍ SE MUESTRA LA BARRA!)
        _controller.TriggerBossAwaken();

        // Forzar parámetros del Animator
        _controller.Animator.SetBool("IsDormant", false);
        _controller.Animator.SetBool("IsAwake", true);
        _controller.Animator.SetBool("IsIdle", false);
        _controller.Animator.SetBool("IsWalking", false);
        _controller.Animator.SetBool("IsAttacking", false);
        
        _controller.Movement.StopMoving();
        _timer = 0;
        
        Debug.Log("🌅 Estado: AWAKE - Animator IsAwake = true");
        _controller.DebugAnimatorParameters();
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"⏱️ AwakeState: Timer = {_timer:F2}s / {_controller.Data.awakeDuration}s");
        }
        
        if (_timer >= _controller.Data.awakeDuration)
        {
            Debug.Log("✅ Awake completado. Cambiando a IdleState");
            _machine.ChangeState<IdleState>();
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _controller.Animator.SetBool("IsAwake", false);
        Debug.Log("🌅 Saliendo de AWAKE - IsAwake = false");
    }
}