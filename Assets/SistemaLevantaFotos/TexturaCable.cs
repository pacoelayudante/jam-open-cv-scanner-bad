using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using static OpenCvSharp.Unity;

public class TexturaCable : MatCable
{
public Texture2D textura;

    public override void Procesar(MatCable matCable){
        salida = matCable.Salida;        
    }

    private void Update()
    {
        if (textura) {
            salida = TextureToMat(textura);
            AlActualizar.Invoke(this);
            enabled = false;
        }
    }
}
