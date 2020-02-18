using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace MyBox
{
    /// <summary>
    /// Usage:
    /// <para>   #if UNITY_EDITOR</para>
    /// <para>   [MethodButton("MethodName1")]  // Must match a the method name!</para>
    /// <para>   [SerializeField] private bool showButtons; // this bool is mandatory!</para>
    /// <para>   #endif</para>
    /// </summary>
    public class ButtonAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        public static PropertyDrawer AssociatedDrawer
        {
            get { return new ButtonAttributeDrawer(); }
        }
#endif
        /// <summary>
        /// Array of different method names for which a button will be displayed.
        /// <para>"MethodOne", MethodTwo" ... "MethodN"</para>
        /// </summary>
        public string MethodName { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ButtonAttribute(string arg)
        {
            MethodName = arg;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonAttributeDrawer : PropertyDrawer
    {
        private int buttonCount;
        private readonly float buttonHeight = EditorGUIUtility.singleLineHeight * 1.5f;
        private ButtonAttribute attr;

        public override void OnGUI(Rect position, SerializedProperty editorFoldout, GUIContent label)
        {
            attr = (ButtonAttribute)base.attribute;

            buttonCount = 1;

            Rect buttonRect = new Rect(position.x, position.y, position.width, buttonHeight);
            if (GUI.Button(buttonRect, ObjectNames.NicifyVariableName(attr.MethodName)))
            {
                InvokeMethod(editorFoldout, attr.MethodName);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (buttonHeight);
        }

        private void InvokeMethod(SerializedProperty property, string name)
        {
            Object target = property.serializedObject.targetObject;
            target.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Invoke(target, null);
        }

        private void LogErrorMessage(SerializedProperty editorFoldout)
        {
            Debug.LogError("<color=red><b>Possible improper usage of method button attribute!</b></color>");
#if NET_4_6
        Debug.LogError($"Got field name: <b>{editorFoldout.name}</b>, Expected: <b>editorFoldout</b>");
        Debug.LogError($"Please see <b>{"Usage"}</b> at <b><i><color=blue>{"https://github.com/GlassToeStudio/UnityMethodButtonAttribute/blob/master/README.md"}</color></i></b>");
#else
            Debug.LogError(string.Format("Got field name: <b>{0}</b>, Expected: <b>editorFoldout</b>", editorFoldout.name));
            Debug.LogError("Please see <b>\"Usage\"</b> at <b><i><color=blue>\"https://github.com/GlassToeStudio/UnityMethodButtonAttribute/blob/master/README.md \"</color></i></b>");
#endif
        }
    }
#endif
}