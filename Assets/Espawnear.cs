using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espawnear : MonoBehaviour
{
    float escala=1f;
    public float Escala{
        get{return escala;}
        set{
            escala = value;
            if(generado){
                generado.transform.localScale = Vector3.one*escala;
            }
        }
    }
    public DraggearImagen prefabGen;
    DraggearImagen generado;

    private void OnEnable() {
        if(generado)Destroy(generado);
        generado = Instantiate(prefabGen,prefabGen.transform.parent);
        generado.gameObject.SetActive(true);
    }
    private void OnDisable() {
        if(generado)Destroy(generado);
    }

}
