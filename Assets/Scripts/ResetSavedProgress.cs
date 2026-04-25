using UnityEngine;

public class ResetSavedProgress : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("All saved progress cleared.");
        }
    }
}