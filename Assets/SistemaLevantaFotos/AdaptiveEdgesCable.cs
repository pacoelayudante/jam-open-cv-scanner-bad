using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveEdgesCable : MatCable
{
    [Range(0f,1f)]
    public float edgesTight = 0.9f;

    public float EdgesTight{
        get{
            return edgesTight;
        }
        set{
            edgesTight = Mathf.Clamp01(value);
            if(input)Actualizar(input);
        }
    }

    public override void Procesar(MatCable matCable){
        
        // detect edges with our 'adaptive' algorithm that computes bounds automatically with
        // image's mean value
        salida = matCable.Salida.Clone().AdaptiveEdges(edgesTight);
    }

}
