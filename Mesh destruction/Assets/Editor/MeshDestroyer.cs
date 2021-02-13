using UnityEngine;
using UnityEditor;

public class MeshDestroyer : EditorWindow 
{
    [MenuItem("Window/MeshDestroyer")]
    public static void ShowWiindow()
    {
        GetWindow<MeshDestroyer>("Mesh destroyer");
    }

    private void OnGUI()
    {
        // Window code
        GUILayout.Label("Object destruction");

        GUILayout.TextField("Cuts");

        if (GUILayout.Button("Apply"))
        {
            Debug.Log("dose work");
        }
    }
}
