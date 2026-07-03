using System;
using System.Collections.Generic;

public class BossStateMachine
{
    // Diccionario que guarda todos los estados por su Tipo (clase)
    private Dictionary<Type, IBossState> _states = new Dictionary<Type, IBossState>();
    
    // Estado actualmente activo
    private IBossState _currentState;

    // Método para registrar un estado (se llama desde el BossController al iniciar)
    public void AddState(IBossState state)
    {
        // Guarda el estado usando su propia clase como clave (ej: typeof(IdleState))
        _states[state.GetType()] = state;
    }

    // Cambia al estado deseado, usando <T> genérico para tener seguridad de tipos
    public void ChangeState<T>() where T : class, IBossState
    {
        // Si el estado actual no es nulo, llamamos a su Exit()
        _currentState?.Exit();

        // Buscamos el nuevo estado en el diccionario
        Type type = typeof(T);
        if (_states.TryGetValue(type, out IBossState newState))
        {
            _currentState = newState;
            _currentState.Enter(); // Llamamos al Enter del nuevo estado
        }
        else
        {
            UnityEngine.Debug.LogError($"El estado {type.Name} no ha sido registrado en la máquina.");
        }
    }

    // Getters para ejecutar los ciclos de vida desde el MonoBehaviour
    public void Update()
    {
        _currentState?.Update();
    }

    public void FixedUpdate()
    {
        _currentState?.FixedUpdate();
    }

    // Método para obtener el estado actual (útil para transiciones condicionales dentro de los mismos estados)
    public T GetCurrentState<T>() where T : class, IBossState
    {
        return _currentState as T;
    }
}