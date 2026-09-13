using UnityEngine;
using UnityEngine.EventSystems;

public class MobileBT : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Direction { Forward, Back, Left, Right }
    public Direction direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (direction)
        {
            case Direction.Forward: PlayerMovement.Instance.MobileForwardPress(); break;
            case Direction.Back: PlayerMovement.Instance.MobileBackPress(); break;
            case Direction.Left: PlayerMovement.Instance.MobileLeftPress(); break;
            case Direction.Right: PlayerMovement.Instance.MobileRightPress(); break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (direction)
        {
            case Direction.Forward: PlayerMovement.Instance.MobileForwardRelease(); break;
            case Direction.Back: PlayerMovement.Instance.MobileBackRelease(); break;
            case Direction.Left: PlayerMovement.Instance.MobileLeftRelease(); break;
            case Direction.Right: PlayerMovement.Instance.MobileRightRelease(); break;
        }
    }
}