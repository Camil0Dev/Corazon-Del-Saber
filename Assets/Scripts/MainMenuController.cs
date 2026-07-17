using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Configuración de Botones")]
    [Tooltip("Arrastra aquí tu botón de Continuar")]
    [SerializeField] private Button btnContinuar;

    private void Start()
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

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("LoadFromSave", 0);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("PlanicieSaber"); 
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("LoadFromSave", 1);
        PlayerPrefs.Save();

        string sceneToLoad = PlayerPrefs.GetString("SavedScene");
        SceneManager.LoadScene(sceneToLoad);
    }
}