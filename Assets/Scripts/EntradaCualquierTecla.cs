using UnityEngine;
using UnityEngine.InputSystem; // <-- ¡NUEVO! Importamos el nuevo Input System

public class EntradaCualquierTecla : MonoBehaviour
{
    void Update()
    {
        bool entradaDetectada = false;

        // 1. Verificamos si hay un teclado conectado y si se presionó CUALQUIER tecla
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            entradaDetectada = true;
        }
        // 2. Verificamos si hay un ratón y si se hizo clic izquierdo o derecho
        else if (Mouse.current != null && (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame))
        {
            entradaDetectada = true;
        }

        // Si se detectó alguna de las dos acciones, pasamos de escena
        if (entradaDetectada)
        {
            ManejadorTransiciones.Instancia.CargarEscena("MenuPrincipal");
            this.enabled = false; // Nos apagamos para no repetir la orden
        }
    }
}