using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that receives log messages.
/// </summary>
public class InterfaceLogManager : MonoBehaviour
{
    [SerializeField]
    private GameObject logItemPrefab;
    [SerializeField]
    private GameObject logItemContainer;

    private readonly int maxLogItems = 5;
    public static readonly Color defaultColor = Color.white;
    private Queue<GameObject> logItems;

    public static InterfaceLogManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void Initialize()
    {
        logItems = new Queue<GameObject>();
        logItemContainer.SetActive(true);
    }

    public void LogMessage(string message, Color color = default)
    {
        if (color == default)
        {
            color = Color.white;
        }

        AddLogItem(message, color);
    }

    private void AddLogItem(string logText, Color color)
    {
        GameObject newLogItem = Instantiate(
            logItemPrefab, logItemContainer.transform);

        logItems.Enqueue(newLogItem);

        InterfaceLogUI logUI =
            newLogItem.GetComponent<InterfaceLogUI>();

        logUI.Setup(logText, color);
        CheckLogLength();
    }

    private void CheckLogLength()
    {
        if (logItems.Count > maxLogItems)
        {
            // Delete longest queued logItem
            GameObject logItem = logItems.Dequeue();
            Destroy(logItem);
        }
    }

}
