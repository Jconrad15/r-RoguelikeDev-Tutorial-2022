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

    public void LogMessage(string message)
    {
        AddLogItem(message);
    }

    private void AddLogItem(string logText)
    {
        GameObject newLogItem = Instantiate(
            logItemPrefab, logItemContainer.transform);

        logItems.Enqueue(newLogItem);

        InterfaceLogUI logUI =
            newLogItem.GetComponent<InterfaceLogUI>();

        logUI.Setup(logText);
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
