using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Runtime component copy
public class ComponentAdd : MonoBehaviour
{
    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
            Debug.Log(destination + " " + field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] targetTag = GameObject.FindGameObjectsWithTag("Body");

        GameObject subtractee = GameObject.Find("Subtractee");


        MonoBehaviour[] teeComponents = subtractee.GetComponents<MonoBehaviour>();

        Debug.Log(teeComponents[0].GetType());
        Debug.Log(teeComponents[1].GetType());

        foreach (GameObject go in targetTag)
        {
            if(go.GetComponent<MeshRenderer>() != null || go.GetComponent<SkinnedMeshRenderer>() != null)
            {
                
                //Debug.Log(go.name);
                //Component c = go.AddComponent(teeComponents[0].GetType());
                Component goComp1 = CopyComponent<MonoBehaviour>(teeComponents[0], go);
                Component goComp2 = CopyComponent<MonoBehaviour>(teeComponents[1], go);
                //System.Reflection.FieldInfo[] fields = go.GetComponent(teeComponents[0].GetType()).GetType().GetFields();
                //Debug.Log(fields);
                //foreach (System.Reflection.FieldInfo field in fields)
                //{
                //    field.SetValue(go.GetComponent(teeComponents[0].GetType()), field.GetValue(teeComponents[0]));

                //    Debug.Log("This is this: " + field);
                //}

                //go.AddComponent(teeComponents[1].GetType());
                //fields = teeComponents[1].GetType().GetFields();
                //foreach (System.Reflection.FieldInfo field in fields)
                //{
                //    field.SetValue(go.GetComponent(teeComponents[1].GetType()), field.GetValue(teeComponents[1]));
                //}
                go.tag = "BodyMesh";
            }
        }
    }
}
