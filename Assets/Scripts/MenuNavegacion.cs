using UnityEngine;

public class MenuNavegacion : MonoBehaviour
{
    [Header("Paneles y UI")]
    public GameObject panelOpciones;
    public GameObject panelControles;
    public GameObject tituloPrincipal;
    public GameObject contenedorBotones;

    // --- Navegación entre Escenas usando el Sistema Global ---

    // Función para tu botón de "Nuevo Juego" o "Continuar"
    public void IniciarJuego()
    {
        // Cambiamos el nombre al de tu nueva escena de prototipo
        ManejadorTransiciones.Instancia.CargarEscena("SantuarioDelAgua_Prototipo");
    }

    // Función por si necesitas volver al menú principal desde alguna otra pantalla
    public void IrAlMenu()
    {
        ManejadorTransiciones.Instancia.CargarEscena("MenuPrincipal");
    }

    // --- Control del Panel de Opciones ---
    public void AbrirOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(true);

        // Limpiamos la pantalla
        if (tituloPrincipal != null) tituloPrincipal.SetActive(false);
        if (contenedorBotones != null) contenedorBotones.SetActive(false);
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);

        // Restauramos el menú
        if (tituloPrincipal != null) tituloPrincipal.SetActive(true);
        if (contenedorBotones != null) contenedorBotones.SetActive(true);
    }

    // --- Control del Panel de Controles ---
    public void AbrirControles()
    {
        if (panelControles != null) panelControles.SetActive(true);

        // Limpiamos la pantalla
        if (tituloPrincipal != null) tituloPrincipal.SetActive(false);
        if (contenedorBotones != null) contenedorBotones.SetActive(false);
    }

    public void CerrarControles()
    {
        if (panelControles != null) panelControles.SetActive(false);

        // Restauramos el menú
        if (tituloPrincipal != null) tituloPrincipal.SetActive(true);
        if (contenedorBotones != null) contenedorBotones.SetActive(true);
    }

    // --- Control de Salida ---
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}