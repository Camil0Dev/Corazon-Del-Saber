public interface IBossState
{
    void Enter();        // Se llama al entrar al estado
    void Update();       // Se llama en el Update del MonoBehaviour (lógica por frame)
    void FixedUpdate();  // Se llama en el FixedUpdate (física/movimiento)
    void Exit();         // Se llama al salir del estado
}