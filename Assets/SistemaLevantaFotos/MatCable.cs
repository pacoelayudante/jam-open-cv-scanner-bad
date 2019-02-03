using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using static OpenCvSharp.Unity;

public class UnityEventMatCable : UnityEngine.Events.UnityEvent<MatCable>{}

public class MatCable : MonoBehaviour
{
    public static UnityEventMatCable AlActualizar = new UnityEventMatCable();

    public MatCable input;

public MatCable Input{
    get{return input;}
    set{input = value;}
}

    int ultimoFrameActualizado;

    private void Awake() {
        AlActualizar.AddListener(Actualizar);
    }
    public void Actualizar(MatCable matCable){
        if(matCable == input && matCable != this){
            if(ultimoFrameActualizado==Time.frameCount){
                Debug.LogWarning("Actualizacion(MatCable matCable) "+matCable+" > "+this);
                return;
            }
            ultimoFrameActualizado = Time.frameCount;
            Procesar(matCable);
            AlActualizar.Invoke(this);
        }
    }
    private void OnDestroy() {
        AlActualizar.RemoveListener(Actualizar);
    }
    
    protected Mat salida;
    public Mat Salida {
        get{
            return salida;
        }
    }
    protected Point[][] puntos;
    public Point[][] SalidaPts{
        get{
            return puntos;
        }
    }

    public virtual void Procesar(MatCable matCable) {
    }
}
