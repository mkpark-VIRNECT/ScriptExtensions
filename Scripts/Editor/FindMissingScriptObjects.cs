#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FindMissingScriptObjects : MonoBehaviour
{
    [HelpBox("1. Select target path with SetPath button\n2. Click crawling button")]    
    public List<Object> missingScriptObjs;
    //[ReadOnly]
    public string path;

    [Button]
    public void SetPath()
    {
        path = EditorUtility.OpenFolderPanel("Select path to search", "", "");
        var current = System.IO.Directory.GetCurrentDirectory();
        current = current.Replace("\\", "/") + "/";

        if (System.IO.Directory.Exists(path))
        {
            path = path.Replace(current, "");
            //if (path[0].Equals('/'))
            //    path.Remove(0);
            Debug.Log($"\"{path}\" path is selected");
        }
        else
        {
            Debug.LogError($"path \"{path}\" is not exist");
            path = "Empty";
        }
    }

    [Button]
    void Crawling()
    {
        EditorUtility.DisplayProgressBar("Crawling...","crawling now please wait", .5f);
        isProcessing = true;
        if(path.Equals("Empty"))
        {
            Debug.LogError("Select target path first");
            return;
        }
        missingScriptObjs = new List<Object>();
        CrawlingRecursive(path);
        Debug.Log($"Crawling finished {missingScriptObjs.Count} mssing script object found ");
        EditorUtility.ClearProgressBar();
        isProcessing = false;
    }
    bool isProcessing = false;
    private void OnGUI ()
    {
        if(isProcessing)
            EditorUtility.DisplayProgressBar("Crawling...", "crawling now please wait", Random.Range(0,1f));
    }

    void CrawlingRecursive(string path)
    {
        var folders = AssetDatabase.GetSubFolders(path);
        for (int i = 0; i < folders.Length; i++)
        {
            CrawlingRecursive(folders[i]);
        }
        var guids = AssetDatabase.FindAssets("", new string[] { path });
        var assets = new List<GameObject>();
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            if (asset != null)
                assets.Add(asset);
        }
        for (int i = 0; i < assets.Count; i++)
        {
            var allComponents = assets[i].GetComponentsInChildren<Component>();
            if (allComponents.Where(x => x == null).Count() > 0)
                missingScriptObjs.Add(assets[i]);
        }

    }
}
#endif