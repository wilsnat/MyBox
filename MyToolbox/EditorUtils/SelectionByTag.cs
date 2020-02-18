using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 

class SelectAllOfTag : ScriptableWizard
{
    public string tagName = "BodyMesh";

    [MenuItem("Selection/Select All of Tag...")]
    static void SelectAllOfTagWizard()
    {
        ScriptableWizard.DisplayWizard<SelectAllOfTag>("Select All Of Tag...", "Make Selection");
    }

    void OnWizardCreate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tagName);
        Selection.objects = gameObjects;
    }
}

#endif