//  /*
//  Transform 1.1
//  23/8/2015
//  - Multiple edit - > Serialized Property
//  Alex Hajdu
//  */


using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    private Object[] tArray;
    private Transform[] transforms;
    private const float FIELD_WIDTH = 212.0f;
    private const bool WIDE_MODE = true;

    private const float POSITION_MAX = 100000.0f;

    private static GUIContent positionGUIContent = new GUIContent(LocalString("Position")
                                                                 , LocalString("The local position of this Game Object relative to the parent."));
    private static GUIContent rotationGUIContent = new GUIContent(LocalString("Rotation")
                                                                 , LocalString("The local rotation of this Game Object relative to the parent."));
    private static GUIContent scaleGUIContent = new GUIContent(LocalString("Scale")
                                                                 , LocalString("The local scaling of this Game Object relative to the parent."));

    private static string positionWarningText = LocalString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");

    private SerializedProperty positionProperty;
    private SerializedProperty rotationProperty;
    private SerializedProperty scaleProperty;

    private static string LocalString(string text)
    {
        return LocalizationDatabase.GetLocalizedString(text);
    }

    public void OnEnable()
    {
        this.positionProperty = this.serializedObject.FindProperty("m_LocalPosition");
        this.rotationProperty = this.serializedObject.FindProperty("m_LocalRotation");
        this.scaleProperty = this.serializedObject.FindProperty("m_LocalScale");
    }

    private void CreateArray()
    {
        transforms = new Transform[tArray.Length];

        var i = 0;
        foreach (var item in tArray)
        {
            transforms[i] = item as UnityEngine.Object as Transform;
            i++;
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.wideMode = TransformEditor.WIDE_MODE;
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - TransformEditor.FIELD_WIDTH; // align field to right of inspector

        this.serializedObject.Update();

        EditorGUILayout.PropertyField(this.positionProperty, positionGUIContent);
        this.RotationPropertyField(this.rotationProperty, rotationGUIContent);
        EditorGUILayout.PropertyField(this.scaleProperty, scaleGUIContent);

        if (!ValidatePosition(((Transform)this.target).position))
        {
            EditorGUILayout.HelpBox(positionWarningText, MessageType.Warning);
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Reset Position"))
        {
            var resetVector = positionProperty.vector3Value;
            resetVector.x = 0; resetVector.y = 0;resetVector.z = 0;
            positionProperty.vector3Value = resetVector;
        }
        
         if (GUILayout.Button("Reset - Keep Z"))
        {
            var resetVector = positionProperty.vector3Value;
            resetVector.x = 0; resetVector.y = 0;
            positionProperty.vector3Value = resetVector;
            rotationProperty.quaternionValue = Quaternion.identity;
            scaleProperty.vector3Value = Vector3.one;
        }

        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("Reset Transforms"))
        {
            positionProperty.vector3Value = Vector3.zero;
            rotationProperty.quaternionValue = Quaternion.identity;
            scaleProperty.vector3Value = Vector3.one;
        }

        EditorGUILayout.EndHorizontal();

        this.serializedObject.ApplyModifiedProperties();
    }

    private bool ValidatePosition(Vector3 position)
    {
        if (Mathf.Abs(position.x) > TransformEditor.POSITION_MAX) return false;
        if (Mathf.Abs(position.y) > TransformEditor.POSITION_MAX) return false;
        if (Mathf.Abs(position.z) > TransformEditor.POSITION_MAX) return false;
        return true;
    }

    private void RotationPropertyField(SerializedProperty rotationProperty, GUIContent content)
    {
        Transform transform = (Transform)this.targets[0];
        Quaternion localRotation = transform.localRotation;
        foreach (UnityEngine.Object t in (UnityEngine.Object[])this.targets)
        {
            if (!SameRotation(localRotation, ((Transform)t).localRotation))
            {
                EditorGUI.showMixedValue = true;
                break;
            }
        }

        EditorGUI.BeginChangeCheck();

        Vector3 eulerAngles = EditorGUILayout.Vector3Field(content, localRotation.eulerAngles);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(this.targets, "Rotation Changed");
            foreach (UnityEngine.Object obj in this.targets)
            {
                Transform t = (Transform)obj;
                t.localEulerAngles = eulerAngles;
            }
            rotationProperty.serializedObject.SetIsDifferentCacheDirty();
        }

        EditorGUI.showMixedValue = false;
    }

    private bool SameRotation(Quaternion rot1, Quaternion rot2)
    {
        if (rot1.x != rot2.x) return false;
        if (rot1.y != rot2.y) return false;
        if (rot1.z != rot2.z) return false;
        if (rot1.w != rot2.w) return false;
        return true;
    }
}
