using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private PlayerInputHandler playerInputHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerInputHandler != null)
            playerInputHandler.MobileJumpPressed();
    }
}