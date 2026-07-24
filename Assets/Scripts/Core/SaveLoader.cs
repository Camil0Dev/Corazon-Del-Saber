using UnityEngine;

public class SaveLoader : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("LoadFromSave", 0) == 1)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            if (player != null)
            {
                float x = PlayerPrefs.GetFloat("PlayerX");
                float y = PlayerPrefs.GetFloat("PlayerY");
                
                player.transform.position = new Vector3(x, y, player.transform.position.z);
            }
            
            PlayerPrefs.SetInt("LoadFromSave", 0);
            PlayerPrefs.Save();
        }
    }
}