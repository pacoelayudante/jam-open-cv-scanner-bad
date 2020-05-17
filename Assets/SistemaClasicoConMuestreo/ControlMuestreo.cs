using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OpenCvSharp;


public class Muestra
{
    public class UnityEventMuestra:UnityEvent<Muestra>{}
    
    public static UnityEventMuestra AlTomarMuestra = new UnityEventMuestra();
    public static void ReiniciarCiclo(){
        MuestraPrincipal.Conteo=-1;
    }

    public static void EnviarMuestra(Mat mat){
        CargarDatosMuestraEnviar(mat);
    }
    static void CargarDatosMuestraEnviar(Mat mat=null, List<Mat> mats = null,List<Point> contorno = null,List<List<Point>> contornos = null){
        MuestraPrincipal.Clear();
        MuestraPrincipal.Conteo++;
        if (mat != null) MuestraPrincipal.mats.Add(mat);
        if(mats != null)MuestraPrincipal.mats.AddRange(mats);
        if (contorno != null) MuestraPrincipal.contornos.Add(contorno);
        if(contornos != null)MuestraPrincipal.contornos.AddRange(contornos);
        AlTomarMuestra.Invoke(MuestraPrincipal);
    }

    static Muestra muestraPrincipal;
    static Muestra MuestraPrincipal{
        get{
            if(muestraPrincipal==null) muestraPrincipal = new Muestra();
            return muestraPrincipal;
        }
    }

    int conteo = -1;
    public int Conteo{
        get{return conteo;}private set{conteo = value;}
    }

    List<Mat> mats = new List<Mat>();
    List<List<Point>> contornos = new List<List<Point>>();

    void Clear(){
        mats.Clear();
        contornos.Clear();
    }
}
