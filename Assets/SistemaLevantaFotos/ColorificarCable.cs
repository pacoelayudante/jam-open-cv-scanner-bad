using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class ColorificarCable : MatCable
{
    public enum Proceso{
        Grayscale = -1,Hue = 0,Saturation = 1,Value = 2
    }
    public Proceso proceso = Proceso.Grayscale;
    
    public override void Procesar(MatCable matCable){
            // instead of regular Grayscale, we use BGR -> HSV and take Hue channel as
            // source
            if (proceso == Proceso.Hue || proceso == Proceso.Saturation || proceso == Proceso.Value)
            {
                var matHSV = matCable.Salida.CvtColor(ColorConversionCodes.RGB2HSV);
                Mat[] hsvChannels = matHSV.Split();
                salida = hsvChannels[(int)proceso];
            }
            // Alternative: just plain BGR -> Grayscale
            else
            {
                salida = matCable.Salida.CvtColor(ColorConversionCodes.BGR2GRAY,1);
            }
    }

}
