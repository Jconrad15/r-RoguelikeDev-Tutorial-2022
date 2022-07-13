using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that describes entity types.
/// </summary>
public class EntityFactory : MonoBehaviour
{
    [SerializeField]
    private Color playerColor;

    public Entity PlayerPrefab { get; private set; }
    public Entity OrcPrefab { get; private set; }
    public Entity TrollPrefab { get; private set; }

    public void InitializeFactory()
    {
        PlayerPrefab = new Entity("@", playerColor, true, true);
        OrcPrefab = new Entity("o", new Color32(63,127,63,255));
        TrollPrefab = new Entity("t", new Color32(0, 127, 0, 255));

    }

    public static EntityFactory Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
