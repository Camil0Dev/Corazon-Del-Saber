using UnityEngine;
using System.Collections.Generic;

public class LoreManager : MonoBehaviour
{
    public static LoreManager Instance { get; private set; }

    [Header("Bases de Datos de Lore (Arrastra aquí tus ScriptableObjects)")]
    [SerializeField] private List<LoreData> basesDeDatos;

    // Diccionario para llevar el control de los textos que aún NO se han mostrado
    private Dictionary<LoreData.LoreType, List<string>> textosDisponibles = new Dictionary<LoreData.LoreType, List<string>>();

    private void Awake()
    {
        // Configuración básica de Singleton para poder llamarlo desde cualquier script
        if (Instance == null)
        {
            Instance = this;
            InicializarTextos();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InicializarTextos()
    {
        foreach (var data in basesDeDatos)
        {
            if (!textosDisponibles.ContainsKey(data.tipoDeLore))
            {
                // Hacemos una COPIA de la lista. 
                // Es vital para no borrar los textos del ScriptableObject original permanentemente.
                textosDisponibles[data.tipoDeLore] = new List<string>(data.textos);
            }
        }
    }

    // Método para que los objetos pidan un texto
    public string ObtenerTextoAleatorioUnico(LoreData.LoreType tipo)
    {
        if (textosDisponibles.ContainsKey(tipo) && textosDisponibles[tipo].Count > 0)
        {
            // Elegimos un índice al azar
            int randomIndex = Random.Range(0, textosDisponibles[tipo].Count);
            string textoElegido = textosDisponibles[tipo][randomIndex];
            
            // Lo removemos de la lista de disponibles para que no se repita en otro objeto
            textosDisponibles[tipo].RemoveAt(randomIndex);
            
            return textoElegido;
        }

        // Si ya se agotaron todos los textos de este tipo, mostramos un mensaje por defecto
        return "... (La inscripción es ilegible) ..."; 
    }
}