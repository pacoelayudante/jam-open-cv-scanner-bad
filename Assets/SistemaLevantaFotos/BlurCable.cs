using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurCable : MatCable
{
    public int scale = 512;
    [Range(0, 1)]
    public float noiseReduction = .7f;
    [Range(3, 99)]
    public int medianKernel = 3;
    public int min = 3;
    public int max = 99;

[System.NonSerialized]
    public float sx=1f,sy=1f;

    public float MedianKernel
    {
        get
        {
            return Mathf.InverseLerp(min, max, medianKernel);
        }
        set
        {
            medianKernel = Mathf.FloorToInt(Mathf.Lerp(min, max, value));
            if (value < 3) value = 3;
            else if (value % 2 == 0) value++;
        }
    }
    public float NoiseReduction{
        get{
            return noiseReduction;
        }
        set{
            noiseReduction = value;
            if(input)Actualizar(input);
        }
    }

    override public void Procesar(MatCable matCable)
    {
        int ancho = matCable.Salida.Width;
        int alto = matCable.Salida.Height;
        // scale down if necessary
        salida = matCable.Salida;
        sx = 1f;
         sy = 1f;
        if (scale != 0)
        {
            if (ancho > scale)
                sx = (float)scale / ancho;
            if (alto > scale)
                sy = (float)scale / alto;

            salida = matCable.Salida.Resize(new OpenCvSharp.Size(System.Math.Min(ancho, scale), System.Math.Min(alto, scale)));
        }
        else salida = matCable.Salida.Clone();

        // reduce noise
        if (noiseReduction != 0)
        {
            int medianKernel = 11;

            // calculate kernel scale
            double kernelScale = noiseReduction;
            if (0 == scale)
                kernelScale *= System.Math.Max(ancho, alto) / 512.0;

            // apply scale
            medianKernel = (int)(medianKernel * kernelScale + 0.5);
            medianKernel = medianKernel - (medianKernel % 2) + 1;

            if (medianKernel > 1)
                salida = salida.MedianBlur(medianKernel);
        }
    }

    public void Procesar2(MatCable matCable)
    {
        if (!enabled)
        {
            salida = matCable.Salida;
            return;
        }
        if (medianKernel < 3) medianKernel = 3;
        else if (medianKernel % 2 == 0) medianKernel++;

        salida = matCable.Salida.Clone();
        salida = salida.MedianBlur(medianKernel);
    }

    private void OnValidate()
    {
        if (medianKernel < 3) medianKernel = 3;
        else if (medianKernel % 2 == 0) medianKernel++;
    }
}
