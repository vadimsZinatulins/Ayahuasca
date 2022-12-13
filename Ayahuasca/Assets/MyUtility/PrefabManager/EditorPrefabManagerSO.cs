
using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/PrefabManager")]
public class EditorPrefabManagerSO : ScriptableObject
{
#if UNITY_EDITOR
    public UIPrefabs UI;
    [Serializable]
    public class UIPrefabs
    {
        public GameObject ButtonExtra;
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(EditorPrefabManagerSO))]
public class EditorPrefabManagerSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.HelpBox("If you move this file somewhere else, also change the path in UiLibraryMenus! ", MessageType.Info);
    }
}
#endif