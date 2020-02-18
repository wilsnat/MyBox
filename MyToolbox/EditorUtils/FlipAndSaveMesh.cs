using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
public static class FlipAndSaveMesh
{

    //[MenuItem("CONTEXT/MeshFilter/Invert Mesh...")]
    //public static void SaveMeshInPlace(MenuCommand menuCommand)
    //{
    //    MeshFilter mf = menuCommand.context as MeshFilter;
    //    Mesh m = mf.sharedMesh;
    //    m.triangles = m.triangles.Reverse().ToArray();
    //    //SaveMesh(m, m.name, false, true);
    //}

    [MenuItem("CONTEXT/MeshFilter/Inverse and Save As New Instance...")]
    public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        m.triangles = m.triangles.Reverse().ToArray();
        m.normals = m.normals.ToList().Select(x => -1 * x).ToArray();
        SaveMesh(m, m.name + "_i", true, true);
        Debug.Log("Saved");
        m.triangles = m.triangles.Reverse().ToArray();
        m.normals = m.normals.ToList().Select(x => -1 * x).ToArray();
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }

}
#endif