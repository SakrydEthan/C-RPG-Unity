using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GeneralPurpose : OdinEditorWindow
{
    [MenuItem("Tools/General")]
    private static void OpenWindow()
    {
        GetWindow<GeneralPurpose>().Show();
    }

    [ButtonGroup("Open Scene")]
    private void StartScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity");
    }
    [ButtonGroup("Open Scene")]
    private void DialogTesting()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/Dialog.unity");
    }
    [ButtonGroup("Open Scene")]
    private void CombatTesting()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Testing/Combat.unity");
    }

    public CharacterDatabase characterDatabase;

    [ButtonGroup("Update Character Database")]
    private void UpdateCharacterDatabase()
    {
        string scene = EditorApplication.currentScene;
        UnityEngine.Debug.Log(scene);
        BaseCharacter[] sceneCharacters = FindObjectsByType<BaseCharacter>(UnityEngine.FindObjectsSortMode.None);
        List<CharacterDatabaseEntry> dbCharacters = new List<CharacterDatabaseEntry>();
        foreach (BaseCharacter character in sceneCharacters)
        {
            UnityEngine.Debug.Log($"adding character {character.id} to database");
            bool isPersistent = character.GetComponent<SpawnedCharacter>() != null;
            CharacterDatabaseEntry dbChar = new CharacterDatabaseEntry(character.id, character.charName, isPersistent);
            dbCharacters.Add(dbChar);
        }
        SceneEntry sceneEntry = new SceneEntry(scene, dbCharacters.ToArray());
        characterDatabase.AddEntry(sceneEntry);
    }
}
