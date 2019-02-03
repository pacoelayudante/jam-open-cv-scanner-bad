using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class UnwrapQuadCable : MatCable
{
    public BlurCable cableDeBlur;
    public MatCable original;

    override public void Procesar(MatCable matCable){
        if (!matCable || matCable.SalidaPts == null || matCable.Salida == null) return;
        salida = matCable.Salida;
        puntos = matCable.SalidaPts;
        if(matCable.SalidaPts.Length ==0)return;
        // scale contour back to input image coordinate space - if necessary
        var sx=1f;
        var sy=1f;
        if(cableDeBlur){
            sx = cableDeBlur.sx;
            sy = cableDeBlur.sy;
        }
        Point[] paperContour = new Point[matCable.SalidaPts[0].Length];
        System.Array.Copy( matCable.SalidaPts[0] , paperContour,paperContour.Length) ;
        if (sx != 1 || sy != 1)
        {
            for (int i = 0; i < paperContour.Length; ++i)
            {
                Point2f pt = paperContour[i];
                paperContour[i] = new Point2f(pt.X / sx, pt.Y / sy);
            }
        }

        // un-wrap
        salida = original?original.Salida:matCable.Salida;
        if (paperContour.Length == 4)
        {
            salida = salida.UnwrapShape(System.Array.ConvertAll(paperContour, p => new Point2f(p.X, p.Y)));
        }

    }
}
