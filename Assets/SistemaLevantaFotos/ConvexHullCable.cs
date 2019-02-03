using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class ConvexHullCable : MatCable
{
    public float areaEsperada = 0.2f;
    public float areaInsignificante = 25f;
    
    public override void Procesar(MatCable matCable){
        if (!matCable || matCable.SalidaPts == null || matCable.Salida == null) return;
        salida = matCable.Salida;
        // check contours and drop those we consider "noise", all others put into a single huge "key points" map
        // also, detect all almost-rectangular contours with big area and try to determine whether they're exact match
        List<Point> keyPoints = new List<Point>();
        List<Point[]> goodCandidates = new List<Point[]>();
        double referenceArea = matCable.Salida.Width * matCable.Salida.Height;
        foreach (Point[] contour in matCable.SalidaPts)
        {
            double length = Cv2.ArcLength(contour, true);

            // drop mini-contours
            if (length >= areaInsignificante)
            {
                Point[] approx = Cv2.ApproxPolyDP(contour, length * 0.01, true);
                keyPoints.AddRange(approx);

                if (approx.Length >= 4 && approx.Length <= 6)
                {
                    double area = Cv2.ContourArea(approx);
                    if (area / referenceArea >= areaEsperada)
                        goodCandidates.Add(approx);
                }
            }
        }

        var hull = Cv2.ConvexHull(keyPoints);
        Point[] hullContour = Cv2.ApproxPolyDP(hull, Cv2.ArcLength(hull, true) * 0.01, true);

        // if(puntos == null || puntos.Length == 0) puntos = new Point[1][];
        goodCandidates.Insert(0,hullContour);
        puntos = goodCandidates.ToArray();
    }

}
