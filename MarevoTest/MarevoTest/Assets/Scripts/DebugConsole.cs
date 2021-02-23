using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole instance;

    private List<string> logArray = new List<string>();

    [SerializeField] private Text displayText;



    #region Singleton
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Singleton is already exist: " + instance.name);
        }
        instance = this;

        Log("Debug console game object: " + name);
        Debug.Log(name);
    }
    #endregion

    public void Log(string stringToAdd) {
        logArray.Add(stringToAdd);
        ShowText();
    }

    private void ShowText() {
        string logAstext = string.Join("\n", logArray.ToArray());
        displayText.text = logAstext.ToString();
    }
}
