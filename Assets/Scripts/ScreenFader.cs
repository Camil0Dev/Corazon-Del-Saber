using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    
    [Header("Referencias")]
    [SerializeField] private Image blackScreen;
    
    [Header("Ajustes")]
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float blackScreenDuration = 0.15f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        if (blackScreen != null) 
            blackScreen.color = new Color(0, 0, 0, 0); 
    }

    public void Blink()
    {
        if (blackScreen != null)
        {
            StopAllCoroutines();
            StartCoroutine(BlinkRoutine());
        }
    }

    private IEnumerator BlinkRoutine()
    {
        blackScreen.color = new Color(0, 0, 0, 1);
        
        yield return new WaitForSeconds(blackScreenDuration); 

        while (blackScreen.color.a > 0)
        {
            blackScreen.color = new Color(0, 0, 0, blackScreen.color.a - (Time.deltaTime * fadeSpeed));
            yield return null;
        }
    }
}