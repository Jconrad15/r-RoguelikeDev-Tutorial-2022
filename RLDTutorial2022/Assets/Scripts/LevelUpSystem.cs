using System.Collections;
using UnityEngine;

/// <summary>
/// Singleton that handles leveling up selections.
/// </summary>
public class LevelUpSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    private Entity CurrentLeveledUpEntity;

    public static LevelUpSystem Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void StartLevelUpSelection(Entity leveledUpEntity)
    {
        StartCoroutine(SelectingLevelUpPerk(leveledUpEntity));
    }

    public IEnumerator SelectingLevelUpPerk(Entity leveledUpEntity)
    {
        CurrentLeveledUpEntity = leveledUpEntity;
        // Wait for end of frame, so that previous
        // clicks don't trigger location selection
        yield return new WaitForEndOfFrame();

        // Start selecting
        playerController.StartModal();
        FindObjectOfType<PerkSelectorUI>().Show();
    }

    public void OnPerkSelected(Perk selectedPerk)
    {
        Level l = CurrentLeveledUpEntity.TryGetLevelComponent();
        if (l == null)
        {
            Debug.LogError("No level component");
            return;
        }

        l.PerkSelected(selectedPerk);

        // Stop selecting
        playerController.StopModal();
        CurrentLeveledUpEntity = null;
    }


}
