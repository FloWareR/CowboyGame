using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToPlay : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Scene1"; 
    [SerializeField] private string sceneToAdd = "Scene2";  

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            LoadMainScene();
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(sceneToLoad);
        SceneManager.sceneLoaded += OnMainSceneLoaded;
    }

    private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneToLoad)
        {
            SceneManager.LoadScene(sceneToAdd, LoadSceneMode.Additive);

            SceneManager.sceneLoaded -= OnMainSceneLoaded;
        }
    }
}