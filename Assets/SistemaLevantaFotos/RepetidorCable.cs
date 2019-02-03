using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepetidorCable : MatCable
{
    int ultimoProceso;
    public override void Procesar(MatCable matCable){
        salida = matCable.Salida;
        ultimoProceso = Time.frameCount;
    }

    private void LateUpdate() {
        if(salida!=null && ultimoProceso!=Time.frameCount)AlActualizar.Invoke(this);
    }

}
