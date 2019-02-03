using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using static OpenCvSharp.Unity;

public class ShowPointsCable : MatCable
{
    public int ancho = 3;
    public Color32 color = Color.magenta;
    public MatCable original;

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

    Scalar colorScalar;
    public Scalar ColorScalar
    {
        get
        {
            if (colorScalar == null) colorScalar = new Scalar(color.b, color.g, color.r, color.a);
            else
            {
                colorScalar.Val0 = color.b;
                colorScalar.Val1 = color.g;
                colorScalar.Val2 = color.r;
                colorScalar.Val3 = color.a;
            }
            return colorScalar;
        }
    }
    Texture2D textura;

    private void OnValidate() {
        if(ancho < -1) ancho = -1;
    }

    private void OnEnable()
    {
        if (!input)
        {
            foreach (var cablesAca in GetComponents<MatCable>())
            {
                if (cablesAca != this)
                {
                    input = cablesAca;
                    break;
                }
            }
        }
    }

    override public void Procesar(MatCable matCable)
    {
        if (!matCable || matCable.SalidaPts == null || matCable.SalidaPts.Length == 0) return;
        if (original && original.Salida!=null) {
            if(original.Salida.Channels()==1) salida = original.Salida.CvtColor(ColorConversionCodes.GRAY2BGR);
            else salida = original.Salida.Clone();
        }
        else salida = matCable.Salida.CvtColor(ColorConversionCodes.GRAY2BGR);
        // Cv2.DrawContours(salida, matCable.SalidaPts, 0, ColorScalar, -1);

        foreach (var contour in matCable.SalidaPts)
        {
            if(contour.Length==0)continue;
            Moments m = Cv2.Moments(contour);
            int cx = (int)(m.M10 / m.M00);
            int cy = (int)(m.M01 / m.M00);

            Cv2.DrawContours(salida, new Point[][] {contour}, 0, ColorScalar, ancho);
        }

        if (RawImage && salida != null)
        {
            var converted = MatToTexture(salida, textura);
            if(textura != converted && textura) Destroy(textura);
            RawImage.texture = textura = converted;
            if (AspectRatioFitter) AspectRatioFitter.aspectRatio = RawImage.texture.width / (float)RawImage.texture.height;
        }
    }
}
