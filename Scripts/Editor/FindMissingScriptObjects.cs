#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FindMissingScriptObjects : MonoBehaviour
{
    [HelpBox("Test")]    
    public List<Object> missingScriptObjs;
    [ReadOnly]
    public string path;

    [ContextMenu("SetPath")]
    [Button("ABC")]
    public void SetPath(AnimationCurve paramC, int paramA, string paramB = "aaa")
    {
        Debug.Log($"{paramA} {paramB}");
        path = EditorUtility.OpenFolderPanel("Select path to search", "", "");
    }

    //[ContextMenu("Crawling")]
    [Button]
    void Crawling()
    {
        if(!System.IO.Directory.Exists(path))
        {
            Debug.LogError($"path \"{path}\" is not exist");
            path = "";
            return;
        }
        missingScriptObjs = new List<Object>();
        CrawlingRecursive(path);
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