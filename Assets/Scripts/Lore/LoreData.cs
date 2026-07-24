using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLoreData", menuName = "Sistema de Lore/Datos de Lore")]
public class LoreData : ScriptableObject
{
    public enum LoreType { Eco, Tablilla, Espejo, Terminal }
    
    [Header("Tipo de Objeto")]
    public LoreType tipoDeLore;

    [Header("Textos Disponibles")]
    [TextArea(3, 10)] // Hace que la caja de texto en el inspector sea más grande
    public List<string> textos;
}