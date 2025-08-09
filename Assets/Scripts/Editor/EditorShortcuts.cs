using UnityEditor;

namespace Assets.Scripts.Editor
{
    public static class EditorShortcuts
    {
        [MenuItem("GameObject/Util/Copy Path")]
        private static void CopyPath()
        {
            var go = Selection.activeGameObject;

            if (go == null)
            {
                return;
            }

            var path = go.name;

            while (go.transform.parent != null)
            {
                go = go.transform.parent.gameObject;
                path = string.Format("/{0}/{1}", go.name, path);
            }

            EditorGUIUtility.systemCopyBuffer = path;
        }

        [MenuItem("GameObject/Util/Copy Path", true)]
        private static bool CopyPathValidation()
        {
            // We can only copy the path in case 1 object is selected
            return Selection.gameObjects.Length == 1;
        }
    }
}
