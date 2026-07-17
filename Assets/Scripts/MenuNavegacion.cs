using UnityEngine;
using UnityEngine.UI; // 🔹 Necesario para controlar el botón

public class MenuNavegacion : MonoBehaviour
{
    [Header("Paneles y UI")]
    public GameObject panelOpciones;
    public GameObject panelControles;
    public GameObject tituloPrincipal;
    public GameObject contenedorBotones;
    
    [Header("Guardado")]
    [Tooltip("Arrastra aquí tu botón de Continuar")]
    public Button btnContinuar;

    private void Start()
    {
        if (btnContinuar != null)
        {
            if (PlayerPrefs.HasKey("SavedScene"))
            {
                btnContinuar.interactable = true;
            }
            else
            {
                btnContinuar.interactable = false;
            }
        }
    }


    public void BotonNuevoJuego()
    {
        PlayerPrefs.SetInt("LoadFromSave", 0);
        PlayerPrefs.Save();
        
        ManejadorTransiciones.Instancia.CargarEscena("PlanicieSaber");
    }

    public void BotonContinuar()
    {
        PlayerPrefs.SetInt("LoadFromSave", 1);
        PlayerPrefs.Save();

        string sceneToLoad = PlayerPrefs.GetString("SavedScene");
        ManejadorTransiciones.Instancia.CargarEscena(sceneToLoad);
    }

    public void IrAlMenu()
    {
        ManejadorTransiciones.Instancia.CargarEscena("MenuPrincipal");
    }

    public void AbrirOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(true);

        if (tituloPrincipal != null) tituloPrincipal.SetActive(false);
        if (contenedorBotones != null) contenedorBotones.SetActive(false);
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);

        if (tituloPrincipal != null) tituloPrincipal.SetActive(true);
        if (contenedorBotones != null) contenedorBotones.SetActive(true);
    }

    public void AbrirControles()
    {
        if (panelControles != null) panelControles.SetActive(true);

        if (tituloPrincipal != null) tituloPrincipal.SetActive(false);
        if (contenedorBotones != null) contenedorBotones.SetActive(false);
    }

    public void CerrarControles()
    {
        if (panelControles != null) panelControles.SetActive(false);

        if (tituloPrincipal != null) tituloPrincipal.SetActive(true);
        if (contenedorBotones != null) contenedorBotones.SetActive(true);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}