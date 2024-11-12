using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToPlay : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Scene1"; 

    private void Update()
    {
        if (Input.anyKeyDown) // This will detect any key or controller button press
        {
            LoadMainScene();
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}