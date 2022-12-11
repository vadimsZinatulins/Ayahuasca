using System;
using UnityEditor;
using UnityEngine;

public static class UiLibraryMenus
{
#if UNITY_EDITOR
    private const int MenuPriority = -50;
    //If you move the PrefabManagerSO file somewhere else, also change the path in UiLibraryMenus! 
    private const string PrefabManagerPath = "Assets/MyUtility/PrefabManager/EditorPrefabManager.asset";

    private static EditorPrefabManagerSO LocatePrefabManager() => AssetDatabase.LoadAssetAtPath<EditorPrefabManagerSO>(PrefabManagerPath);

    private static void SafeInstantiate(Func<EditorPrefabManagerSO, GameObject> itemSelector)
    {
        var prefabManager = LocatePrefabManager();

        if (!prefabManager)
        {
            Debug.LogWarning($"PrefabManager not found at path {PrefabManagerPath}");
            return;
        }

        var item = itemSelector(prefabManager);
        var instance = PrefabUtility.InstantiatePrefab(item, Selection.activeTransform);

        Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
        Selection.activeObject = instance;
    }

    // Same method from the start of the blog post.
    [MenuItem("GameObject/Naidio/UI/Buttons/ButtonExtra")]
    private static void Create_ButtonExtra()
    {
        SafeInstantiate(prefabManager => prefabManager.UI.ButtonExtra);
    }
#endif

}
