using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static OpenCvSharp.Unity;

public class ShowMatCable : MatCable
{
    RawImage rawImage;
    RawImage RawImage
    {
        get
        {
            if (rawImage == null)
            {
                rawImage = GetComponent<RawImage>();
            }
            return rawImage;
        }
    }
    AspectRatioFitter aspectRatioFitter;
    AspectRatioFitter AspectRatioFitter
    {
        get
        {
            if (aspectRatioFitter == null)
            {
                aspectRatioFitter = GetComponent<AspectRatioFitter>();
            }
            return aspectRatioFitter;
        }
    }

    Texture2D textura;

    private void OnEnable() {
        if(!input) {
            foreach (var cablesAca in GetComponents<MatCable>())
            {
                if(cablesAca!=this){
                    input = cablesAca;
                    break;
                }
            }
        }
    }

    override public void Procesar(MatCable matCable){
        salida = matCable.Salida;
        puntos = matCable.SalidaPts;
        if(!enabled) return;
        if(RawImage && matCable.Salida!=null){
            var converted = MatToTexture(salida, textura);
            if(textura != converted && textura) Destroy(textura);
            RawImage.texture = textura = converted;
            if(AspectRatioFitter)AspectRatioFitter.aspectRatio = RawImage.texture.width/(float)RawImage.texture.height;
        }
    }
}
