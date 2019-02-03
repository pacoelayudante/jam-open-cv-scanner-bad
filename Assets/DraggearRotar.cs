using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggearRotar : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    float anguloOriginal;

private void LateUpdate() {
    var escPad = transform.parent.localScale;
    transform.localScale = new Vector3( 1f/escPad.x,1f/escPad.y,1f/escPad.z);
}

private void Awake() {
     anguloOriginal = Vector2.SignedAngle( transform.right ,Vector2.right);   
}

    public void OnBeginDrag(PointerEventData pointer){
    }
    public void OnEndDrag(PointerEventData pointer){
        
    }
    public void OnDrag(PointerEventData pointer){
//  var anguloPrevio = Vector2.SignedAngle( transform.right ,Vector2.right);
 var anguloNuevo = Vector2.SignedAngle( pointer.position- (Vector2)transform.parent.position ,Vector2.right);
 transform.parent.rotation = Quaternion.Euler(0,0,anguloOriginal-anguloNuevo);
    }
}
