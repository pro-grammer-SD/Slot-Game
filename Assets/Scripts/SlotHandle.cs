using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlotHandle : MonoBehaviour
{
    [Header("Handle Images (two separate GameObjects)")]
    [Tooltip("Image showing the handle at rest. Size it in the Editor.")]
    public GameObject idleImageObject;
    [Tooltip("Image showing the handle pulled down. Size it in the Editor.")]
    public GameObject pulledImageObject;

    [Header("Animation")]
    public float pullDuration = 0.2f;
    public float returnDuration = 0.4f;

    [Header("References")]
    public Button handleButton;

    private SlotGameManager gameManager;

    public void Initialize(SlotGameManager manager)
    {
        gameManager = manager;
        handleButton.onClick.AddListener(OnHandlePulled);
        SetInteractable(false);
        ShowIdle();
    }

    /// <summary>
    /// Called when the player clicks/taps the handle.
    /// </summary>
    private void OnHandlePulled()
    {
        SetInteractable(false);

        ShowPulled();
        DOVirtual.DelayedCall(pullDuration, () =>
        {
            DOVirtual.DelayedCall(returnDuration, () =>
            {
                ShowIdle();
            });
            gameManager.OnHandlePulled();
        });
    }

    private void ShowIdle()
    {
        idleImageObject.SetActive(true);
        pulledImageObject.SetActive(false);
    }

    private void ShowPulled()
    {
        idleImageObject.SetActive(false);
        pulledImageObject.SetActive(true);
    }

    public void SetInteractable(bool state)
    {
        handleButton.interactable = state;
    }
}