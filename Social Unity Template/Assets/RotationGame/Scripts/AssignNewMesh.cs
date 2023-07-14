using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssignNewMesh : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        LoadMeshRecursively(this.gameObject.transform);
    }

    private void LoadMeshRecursively(Transform trans)
    {
        foreach (Transform child in trans)
        {
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;
                string[] matches = AssetDatabase.FindAssets(mesh.name.Substring(0, mesh.name.Length - 9), new string[] { "Assets/RotationGame/Meshes" });
                if(matches.Length == 1)
                {
                    Debug.Log(matches[0]);
                    Mesh neu = (Mesh) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(matches[0]), typeof(Mesh));
                    meshFilter.mesh = neu;
                }
            }
            LoadMeshRecursively(child);
        }
    }
}
