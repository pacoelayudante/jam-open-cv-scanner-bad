using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class ThresholdCable : MatCable
{
    [Range(0,1)]
    public float umbral;
    public float max = 255f;
    public bool adaptive = false;
    public ThresholdTypes tipoThresh = ThresholdTypes.BinaryInv;
    public AdaptiveThresholdTypes adaptiveThresh;
    [Range(0.0001f,1f)]
    public float adapMaxValue = 0.5f;
    [Range(3,113)]
    public int adapBloques = 9;
    [Range(-2f,2f)]
    public float adapConstante = 0.0f;

    public float Umbral
    {
        get { return umbral; }
        set
        {
            umbral = value;
            if (input) Actualizar(input);
        }
    }

    public override void Procesar(MatCable matCable)
    {
        if (!matCable || matCable.Salida == null) return;
        salida = new Mat();

        if (adaptive)
        {
            if(adapMaxValue<=0f)adapMaxValue = 0.0001f;
            if(adapBloques < 3) adapBloques = 3;
            else if (adapBloques%2==0) adapBloques++;
            Cv2.AdaptiveThreshold(matCable.Salida,salida,adapMaxValue*max,adaptiveThresh,tipoThresh,adapBloques,adapConstante*max);
        }
        else
        {
            Cv2.Threshold(matCable.Salida, salida, umbral*max, max, tipoThresh);
        }
    }
}
