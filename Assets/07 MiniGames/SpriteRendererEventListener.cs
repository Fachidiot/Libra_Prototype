using UnityEngine;
using UnityEngine.Events;

public class SpriteRendererEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent leftClickEvent;
    [SerializeField] private UnityEvent rightClickEvent;
    bool isOver = false;

    public UnityEvent GetLeftClickEvent()
    {
        return leftClickEvent;
    }

    public UnityEvent GetRightClickEvent()
    {
        return rightClickEvent;
    }
    
    void Update()
    {
        if (isOver && Input.GetMouseButtonDown(0))
        {
            //좌클릭 이벤트
            leftClickEvent.Invoke();
        }
        if (isOver && Input.GetMouseButtonDown(1))
        {
            //우클릭 이벤트
            rightClickEvent.Invoke();
        }
    }
    
    void OnMouseOver()
    {
        isOver = true;
    }
    
    void OnMouseExit()
    {
        isOver = false;
    }
}
