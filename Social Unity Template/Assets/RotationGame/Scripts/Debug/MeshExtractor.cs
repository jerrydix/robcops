using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class MeshExtractor : MonoBehaviour
{
    private string path = "Assets/RotationGame/Meshes/";

    private void Awake()
    {
        SaveMeshRecursively(this.gameObject.transform);
    }

    private void SaveMeshRecursively(Transform trans)
    {
        foreach (Transform child in trans)
        {
            Debug.Log(child.gameObject.name);
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;
                string currentPath = path + mesh.name.Substring(0, mesh.name.Length - 9) + ".mesh";
                AssetDatabase.CreateAsset(mesh, currentPath);
            }
            SaveMeshRecursively(child);
        }
    }
}
