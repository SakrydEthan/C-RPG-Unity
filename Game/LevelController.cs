using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    public static LevelController instance;

    public int entryPoint = 0;


    public AsyncOperation operation;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadLevelByName(string scene)
    {
        SaveController.instance.ClearSaveDelegate();
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        //SaveController.instance.InstantiatePlayer();
    }

    public static void ReloadLevel(string scene)
    {
        Debug.Log("reloading: " + scene);
        //SceneManager.LoadScene("GameOver", LoadSceneMode.Single);

        instance.operation = instance.LoadSceneNameAsync(scene);
        instance.StartCoroutine(instance.LoadSceneAsync());
        //SceneManager.LoadScene(scene, LoadSceneMode.Single);
        //SaveController.instance.InstantiatePlayer();
    }

    public static string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void StartNewGame(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
        SaveController.instance.InstantiatePlayer();
    }

    /// <summary>
    /// Contains behavior for starting game from the menu
    /// </summary>
    public static void PlayGame()
    {
        if(SaveController.CheckForSave() == false)
        {
            //load the first level
            NewGameController.BeginCharacterCustomisation();
            return;

            //SceneManager.LoadScene(NewGameController.GetStartSceneName(), LoadSceneMode.Single);
            //SaveController.instance.InstantiatePlayer();
        }
        else
        {
            //load saved level
            SaveController.instance.LoadLevelFromSave();
            SaveController.instance.InstantiatePlayer();
        }
    }

    public static void CheckEntryPoint(int id, Transform transform)
    {
        if(id == instance.entryPoint)
        {
            PlayerInstanceController.instance.SetPlayerPositionLoad(transform.position, transform.rotation.eulerAngles);
        }
    }

    public static void SetEntryPoint(int id)
    {
        instance.entryPoint = id;
    }

    public AsyncOperation LoadSceneNameAsync(string scene)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        return loading;
    }

    public IEnumerator LoadSceneAsync()
    {
        while (!operation.isDone)
        {
            yield return null;
        }

        SaveController.instance.InstantiatePlayer();

        yield return null;
    }
}
