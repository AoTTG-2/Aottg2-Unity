using UnityEngine;
using UnityEditor;

/// <summary>
/// Simple input dialog for editor tools
/// </summary>
public class EditorInputDialog : EditorWindow
{
    private string _title;
    private string _message;
    private string _inputText;
    private System.Action<string> _onConfirm;
    private static EditorInputDialog _instance;

    public static string Show(string title, string message, string defaultValue = "")
    {
        string result = null;
        bool confirmed = false;

        var window = CreateInstance<EditorInputDialog>();
        window._title = title;
        window._message = message;
        window._inputText = defaultValue;
        window.titleContent = new GUIContent(title);
        window.position = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100);
        window.ShowModalUtility();

        return window._inputText;
    }

    void OnGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(_message);
        EditorGUILayout.Space(5);

        GUI.SetNextControlName("InputField");
        _inputText = EditorGUILayout.TextField(_inputText);

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("OK", GUILayout.Width(80)) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return))
        {
            Close();
            Event.current.Use();
        }

        if (GUILayout.Button("Cancel", GUILayout.Width(80)) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape))
        {
            _inputText = null;
            Close();
            Event.current.Use();
        }

        EditorGUILayout.EndHorizontal();

        // Auto-focus input field
        if (Event.current.type == EventType.Layout)
        {
            EditorGUI.FocusTextInControl("InputField");
        }
    }
}
