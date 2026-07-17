using UnityEngine;
using UnityEngine.InputSystem;

public class EntradaCualquierTecla : MonoBehaviour
{
    void Update()
    {
        bool entradaDetectada = false;

        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            entradaDetectada = true;
        }
        else if (Mouse.current != null && (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame))
        {
            entradaDetectada = true;
        }

        if (entradaDetectada)
        {
            ManejadorTransiciones.Instancia.CargarEscena("MenuPrincipal");
            this.enabled = false; // Nos apagamos para no repetir la orden
        }
    }
}