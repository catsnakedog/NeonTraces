using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ScrollPlus : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ScrollRect ParentSR;

    public void OnBeginDrag(PointerEventData e)
    {
        ParentSR.OnBeginDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        ParentSR.OnDrag(e);
    }

    public void OnEndDrag(PointerEventData e)
    {
        ParentSR.OnEndDrag(e);
    }
}