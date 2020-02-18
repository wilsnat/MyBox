using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Extends the Transform context menu to provide support for copying, pasting
/// and pushing position/rotation/scale data around.
/// 
/// Code by Zach Aikman - zachaikman@gmail.com
/// </summary>
#if UNITY_EDITOR
public class TransformContextMenu
{
    private class TransformClipboard
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public bool isPositionSet = false;
        public bool isRotationSet = false;
        public bool isScaleSet = false;
    }

    private static TransformClipboard clipboard = new TransformClipboard();

    #region Copy Methods
    [MenuItem("CONTEXT/Transform/Copy Transform", false, 150)]
    static void CopyTransform()
    {
        CopyPosition();
        CopyRotation();
        CopyScale();
    }

    [MenuItem("CONTEXT/Transform/Copy Position", false, 151)]
    static void CopyPosition()
    {
        clipboard.position = Selection.activeTransform.localPosition;
        clipboard.isPositionSet = true;
    }

    [MenuItem("CONTEXT/Transform/Copy Rotation", false, 152)]
    static void CopyRotation()
    {
        clipboard.rotation = Selection.activeTransform.localRotation;
        clipboard.isRotationSet = true;
    }

    [MenuItem("CONTEXT/Transform/Copy Scale", false, 153)]
    static void CopyScale()
    {
        clipboard.scale = Selection.activeTransform.localScale;
        clipboard.isScaleSet = true;
    }
    #endregion

    #region Paste Methods
    [MenuItem("CONTEXT/Transform/Paste Transform", false, 200)]
    static void PasteTransform()
    {
        PastePosition();
        PasteRotation();
        PasteScale();
    }

    [MenuItem("CONTEXT/Transform/Paste Position", false, 201)]
    static void PastePosition()
    {
        Undo.RegisterCompleteObjectUndo(Selection.activeTransform, "Paste Position");
        Selection.activeTransform.localPosition = clipboard.position;
    }

    [MenuItem("CONTEXT/Transform/Paste Rotation", false, 202)]
    static void PasteRotation()
    {
        Undo.RegisterCompleteObjectUndo(Selection.activeTransform, "Paste Rotation");
        Selection.activeTransform.localRotation = clipboard.rotation;
    }

    [MenuItem("CONTEXT/Transform/Paste Scale", false, 203)]
    static void PasteScale()
    {
        Undo.RegisterCompleteObjectUndo(Selection.activeTransform, "Paste Scale");
        Selection.activeTransform.localScale = clipboard.scale;
    }
    #endregion

    #region Validation
    [MenuItem("CONTEXT/Transform/Paste Transform", true)]
    static bool ValidatePasteTransform()
    {
        return ValidatePastePosition() && ValidatePasteRotation() && ValidatePasteScale();
    }

    [MenuItem("CONTEXT/Transform/Paste Position", true)]
    static bool ValidatePastePosition()
    {
        return clipboard.isPositionSet;
    }

    [MenuItem("CONTEXT/Transform/Paste Rotation", true)]
    static bool ValidatePasteRotation()
    {
        return clipboard.isRotationSet;
    }

    [MenuItem("CONTEXT/Transform/Paste Scale", true)]
    static bool ValidatePasteScale()
    {
        return clipboard.isScaleSet;
    }

    [MenuItem("CONTEXT/Transform/Push To Children", true)]
    static bool ValidatePushToChildren()
    {
        return Selection.activeTransform.childCount > 0;
    }

    [MenuItem("CONTEXT/Transform/Push To Parent", true)]
    static bool ValidatePushToParent()
    {
        return Selection.activeTransform.parent != null;
    }
    #endregion

    [MenuItem("CONTEXT/Transform/Uniform Scale", false, 250)]
    static void UniformScale()
    {
        GameObject go = Selection.activeTransform.gameObject;
        UniformScaleWindow usw = UniformScaleWindow.ShowWindow();
        usw.SelectedObject(go);
    }


    [MenuItem("CONTEXT/Transform/Create Empty Holder Object", false, 251)]
    static void CreateHolderObject()
    {
        GameObject go = Selection.activeTransform.gameObject;
        GameObject newGo = new GameObject(go.name + "_holder");
        Undo.RegisterCreatedObjectUndo(newGo, "Holder Object");
        newGo.transform.parent = go.transform.parent;
        newGo.transform.localPosition = go.transform.localPosition;
        newGo.transform.localRotation = go.transform.localRotation;
        newGo.transform.localScale = go.transform.localScale;
        Undo.SetTransformParent(go.transform, newGo.transform, "Holder Object");
        go.transform.parent = newGo.transform;
    }
}

class UniformScaleWindow : EditorWindow
{
    float scalingValue = 1;
    bool preview = true;
    GameObject selectedObject;
    Vector3 originalScale;

    public static UniformScaleWindow ShowWindow()
    {
        return (UniformScaleWindow)EditorWindow.GetWindow(typeof(UniformScaleWindow));
    }

    public void SelectedObject(GameObject go)
    {
        selectedObject = go;
        originalScale = selectedObject.transform.localScale;
        this.title = go.name + ": Uniform Scale";
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        scalingValue = EditorGUILayout.FloatField("Scaling Value", scalingValue);
        preview = EditorGUILayout.Toggle("Show scale", preview);
        if(EditorGUI.EndChangeCheck())
        {
            if (preview) selectedObject.transform.localScale = originalScale * scalingValue;
            else selectedObject.transform.localScale = originalScale;
        }
        if (GUILayout.Button("Confirm"))
        {
            this.Close();
        }
        else if(GUILayout.Button("Cancel"))
        {
            scalingValue = 1;
            this.Close();
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Closing Window, " + scalingValue);
        if (scalingValue != 1)
        {
            selectedObject.transform.localScale = originalScale;
            Undo.RecordObject(selectedObject.transform, "Uniform Scale : ");
            selectedObject.transform.localScale = originalScale * scalingValue;
        }
    }
}
#endif