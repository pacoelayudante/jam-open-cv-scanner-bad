using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class ContourCable : MatCable
{
    public ContourApproximationModes approxMode = ContourApproximationModes.ApproxNone;
    public RetrievalModes retrievalMode = RetrievalModes.List;

    public int seleccionarUnico = -1;
    public bool masGrande = false;

    public int cantConts;

    public override void Procesar(MatCable matCable)
    {
        if (!matCable || matCable.Salida == null) return;
        // salida = matCable.Salida;
        salida = matCable.Salida.Clone();
        HierarchyIndex[] hierarchy;
        Cv2.FindContours(salida, out puntos, out hierarchy, retrievalMode, approxMode, null);
        cantConts = puntos.Length;
        if (puntos.Length > 0)
        {
            if (masGrande)
            {
                var mayor = puntos[0];
                var areaMayor = 0d;
                foreach(var grupo in puntos){
                    var area =Cv2.ContourArea(grupo);
                    if(area > areaMayor){
                        areaMayor = area;
                        mayor = grupo;
                    }
                }
                puntos = new Point[][] { mayor };
            }
            else if (seleccionarUnico >= 0)
            {
                puntos = new Point[][] { puntos[seleccionarUnico % hierarchy.Length] };
            }
        }
    }
}
