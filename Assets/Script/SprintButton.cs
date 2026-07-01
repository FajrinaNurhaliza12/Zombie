using UnityEngine;
using UnityEngine.EventSystems;

public class SprintButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerInputHandler playerInputHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerInputHandler != null)
            playerInputHandler.MobileSprintDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (playerInputHandler != null)
            playerInputHandler.MobileSprintUp();
    }

    private void OnDisable()
    {
        if (playerInputHandler != null)
            playerInputHandler.MobileSprintUp();
    }
}