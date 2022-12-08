using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class CustomGizmo : MonoBehaviour
{
    static CustomGizmo _instance;
    public static CustomGizmo Instance
    {
        get
        {
            if(_instance == null )
            {
                _instance = new GameObject("CustomGizmo").AddComponent<CustomGizmo>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    List<Action> gizmoDrawActions_OneTime = new List<Action>();
    Dictionary<int, Action> gizmoDrawActions_Consist = new Dictionary<int, Action>();

    public static void RegistOneTimeGizmo (Action a)
    {
        Instance.gizmoDrawActions_OneTime.Add(a);
    }

    public static void RegistConsistGizmo (Action a)
    {
        Instance.gizmoDrawActions_Consist[a.GetHashCode()] = a;
    }

    public static void DisregistConsistGizmo(Action a)
    {
        Instance.gizmoDrawActions_Consist.Remove(a.GetHashCode());
    }
    private void OnDrawGizmos ()
    {
        for(int i = 0; i < gizmoDrawActions_OneTime.Count; i++)
        {
            Gizmos.color = Color.white;
            gizmoDrawActions_OneTime[i]?.Invoke();
        }
        gizmoDrawActions_OneTime.Clear();

        foreach(var action in gizmoDrawActions_Consist)
        {
            Gizmos.color = Color.white;
            action.Value?.Invoke();
        }
    }
}
