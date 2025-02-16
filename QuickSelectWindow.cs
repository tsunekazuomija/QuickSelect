using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using System.IO;


public class QuickSelectWindow : EditorWindow
{
    private const string SaveFolder = "QuickSelect";
    private const string SaveFile = "SelectedGameObjects.json";
    private const string SavePath = SaveFolder + "/" + SaveFile;
    private string _currentScene;
    private List<GameObject> _selectedObjects = new();


    [MenuItem("Tools/Quick Select")]
    public static void ShowWindow()
    {
        GetWindow<QuickSelectWindow>("Quick Select");
    }

    private void OnEnable()
    {
        LoadObjects();
        EditorApplication.hierarchyChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= OnSceneChanged;
    }

    /// <summary>
    /// called when the scene changed
    /// </summary>
    private void OnSceneChanged()
    {
        LoadObjects();
        Repaint();
    }

    private void OnGUI()
    {
        GUILayout.Space(20);

        // select objects button
        if (GUILayout.Button("Register Selected Objects"))
        {
            RegisterSelectedObjects();
        }

        GUILayout.Space(20);

        // referencing objects after a scene change causes
        // MissingReferenceException.
        _selectedObjects.RemoveAll(obj => obj == null);


        // display selected objects
        foreach (var obj in _selectedObjects)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(obj.name, GUILayout.ExpandWidth(true)))
            {
                Selection.activeGameObject = obj;
            }

            if (GUILayout.Button("⋮", GUILayout.Width(30)))
            {
                GenericMenu menu = new();
                menu.AddItem(new GUIContent("Remove"), false, () =>
                {
                    _selectedObjects.Remove(obj);
                    SaveObjects();
                    Repaint();
                });
                menu.ShowAsContext();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
        }
    }

    private void RegisterSelectedObjects()
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (!_selectedObjects.Contains(obj))
            {
                _selectedObjects.Add(obj);
            }
        }
        SaveObjects();
    }

    #region Save/Load
    private void SaveObjects()
    {
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }

        var dataPaths = new List<string>();
        foreach (var obj in _selectedObjects)
        {
            if (obj != null)
            {
                dataPaths.Add(GetGameObjectPath(obj));
            }
        }


        _currentScene = SceneManager.GetActiveScene().name;

        var dataList = new List<SceneData>();
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            dataList = JsonUtility.FromJson<SceneDataArray>(json).scenes;
        }
        dataList.RemoveAll(d => d.sceneName == _currentScene);
        dataList.Add(new SceneData { sceneName = _currentScene, objects = dataPaths });
        File.WriteAllText(SavePath, JsonUtility.ToJson(new SceneDataArray{ scenes = dataList}, true));
    }

    private void LoadObjects()
    {
        _selectedObjects.Clear();
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            var dataList = JsonUtility.FromJson<SceneDataArray>(json).scenes;

            Scene currentScene = SceneManager.GetActiveScene();
            SceneData data = dataList.Find(d => d.sceneName == currentScene.name);
            if (data == null) return;

            foreach (var objPath in data.objects)
            {
                var go = GameObject.Find(objPath);
                if (go != null)
                {
                    _selectedObjects.Add(go);
                }
            }
        }
    }
    #endregion

    private string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "/" + path;
        }
        return path;
    }


    /// <summary>
    /// Since JsonUtility does support serialization of top-level
    /// arrays / lists, need to wrap the list in a class.
    /// </summary>
    [System.Serializable]
    private class SceneDataArray
    {
        public List<SceneData> scenes;
    }

    [System.Serializable]
    private class SceneData
    {
        public string sceneName;
        public List<string> objects;
    }
}
