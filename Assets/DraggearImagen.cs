using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggearImagen : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ContourClipCable clipeado;
    Sprite sprite;
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        if(clipeado && clipeado.sprite){
            sprite = clipeado.sprite;
        }
        img = GetComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
    }

    private void OnDestroy() {
        foreach (var item in GetComponentsInChildren<DraggearRotar>())
        {
            Destroy(item.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData pointer){
        
    }
    public void OnEndDrag(PointerEventData pointer){
        
    }
    public void OnDrag(PointerEventData pointer){
     var position = pointer.position;
     transform.position = position;
    }
}
