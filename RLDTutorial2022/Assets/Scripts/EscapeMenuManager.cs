using UnityEngine;

public class EscapeMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject escapeMenuArea;

    private bool canOpenEscapeMenu;
    public bool IsOpen { get; private set; }

    private void Start()
    {
        canOpenEscapeMenu = true;
        Close();

        FindObjectOfType<TargetingSystem>()
            .RegisterOnTargetingStatusChanged(
                OnTargetingStatusChanged);
    }

    private void OnTargetingStatusChanged(bool targetingStatus)
    {
        canOpenEscapeMenu = !targetingStatus;
    }

    private void Update()
    {
        if (canOpenEscapeMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleEscapeMenu();
            }
        }
    }

    private void ToggleEscapeMenu()
    {
        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Close()
    {
        escapeMenuArea.SetActive(false);
        IsOpen = false;
    }

    private void Open()
    {
        escapeMenuArea.SetActive(true);
        IsOpen = true;
    }

}
