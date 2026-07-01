using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Weapon weapon;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (weapon != null)
            weapon.FireButtonDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (weapon != null)
            weapon.FireButtonUp();
    }
}