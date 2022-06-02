using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameObject player;
    SaveObject currentSave;

    private static SceneLoader instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Scene Loader in the scene");
        }
        instance = this;

        currentSave = SaveHelper.currentSaveObject();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    public static SceneLoader GetInstance()
    {
        return instance;
    }

    public void LoadFromFile()
    {
        currentSave = SaveHelper.currentSaveObject();

        // Default to loading a new game if there is no save present
        if (currentSave == null) {
            SceneManager.LoadScene("TestLevel");
            return;
        }

        string sceneName = currentSave.currentScene;
        Debug.Log("Loading Saved Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Make sure we only run this stuff when loading the saved scene
        if (currentSave == null || scene.name != currentSave.currentScene)
            return;

        // Get the player already in the scene
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 savedPlayerPosition = currentSave.playerPosition;

        // Convert rotation from euler (vector3)
        Quaternion savedRotation = Quaternion.Euler(currentSave.playerRotationEuler);
        Quaternion savedCameraRotation = Quaternion.Euler(currentSave.cameraRotationEuler);

        Debug.Log("Saved location is " + savedPlayerPosition);
        Debug.Log("Saved rotation is " + savedRotation);
        Debug.Log("Saved cam rotation is " + savedCameraRotation);

        Camera playerCamera = player.gameObject.GetComponentInChildren<FirstPersonPlayer>().fpsCam;

        // Move player and rotate camera
        player.transform.SetPositionAndRotation(savedPlayerPosition, player.transform.rotation);
        playerCamera.transform.SetPositionAndRotation(playerCamera.transform.position, savedCameraRotation);

    }

}

