using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDeWebcam : MonoBehaviour
{
    [System.Serializable]
    public class UnityEventString : UnityEngine.Events.UnityEvent<string> { }

    public UnityEngine.UI.RawImage rawTextureDump;
    public int indiceCamDeseado = 0, resolucionDeseada = 0;
    public int IndiceCamDeseado{
        get{return indiceCamDeseado;}
        set{indiceCamDeseado=value;}
    }
    public int ResolucionDeseada{
        get{return resolucionDeseada;}
        set{resolucionDeseada=value;}
    }
    public UnityEventString AlMensaje = new UnityEventString(), AlMensajeError = new UnityEventString();
    int indiceCamActual = -1;
    int resolucionActual = 0;

    public int IndiceCam
    {
        get
        {
            return indiceCamActual;
        }
        // set
        // {
        //     if (value > 0 && indiceCamActual != value && webCam)
        //     {
        //         webCam.Stop();
        //         webCam = null;
        //     }
        //     indiceCamDeseado = indiceCamActual = value;
        //     if (enabled && indiceCamActual > -1 && indiceCamActual < WebCamTexture.devices.Length)
        //     {
        //         var mejorRes = MejorResolution;
        //         webCam = new WebCamTexture(Device.name, 9999, 9999);
        //         webCam.Play();
        //         if (meterWebCamAcaAlArrancar) {
        //             meterWebCamAcaAlArrancar.texture = webCam;
        //             var aspectRatioFitter = meterWebCamAcaAlArrancar.GetComponent<UnityEngine.UI.AspectRatioFitter>();
        //             if(aspectRatioFitter)aspectRatioFitter.aspectRatio = webCam.width/webCam.height;
        //         }
        //     }
        // }
    }

    public void Detener()
    {
        if (webCam != null)
        {
            if (webCam.isPlaying)
            {
                AlMensaje.Invoke("ControlDeWebcam : deteniendo camara");
                webCam.Stop();
            }
            webCam = null;
        }
    }

    public void Iniciar()
    {
        Iniciar(indiceCamDeseado, resolucionDeseada);
    }
    public void Iniciar(int indiceCam)
    {
        Iniciar(indiceCam, resolucionDeseada);
    }
    public void IniciarDeviceDeseadoConResolucion(int resolucion)
    {
        Iniciar(indiceCamDeseado, resolucion);
    }
    public void Iniciar(int indiceCam, int resolucion)
    {
        if (webCam != null)
        {
            if (webCam.isPlaying)
            {
                AlMensaje.Invoke("ControlDeWebcam : deteniendo camara anterior...");
                webCam.Stop();
            }
            webCam = null;
        }
        AlMensaje.Invoke(string.Format("ControlDeWebcam : chequeando valores, indiceCam = {0} resolucion = {1}...", indiceCam, resolucion));

        if (indiceCam < 0)
        {
            indiceCam = 0;
            AlMensajeError.Invoke("ControlDeWebcam : ( indiceCam < 0 ), seteando a 0...");
        }
        else if (indiceCam >= WebCamTexture.devices.Length)
        {
            indiceCam = WebCamTexture.devices.Length - 1;
            AlMensajeError.Invoke(string.Format("ControlDeWebcam : ( indiceCam >= {0} ), seteando a cantCams-1...", WebCamTexture.devices.Length));
        }
        indiceCamActual = indiceCam;
        var resDisponibles = Device.availableResolutions;
        var res = new Resolution();
        if (resDisponibles == null)
        {
            AlMensajeError.Invoke("ControlDeWebcam : resoluciones disponibles no informadas...");
        }
        else
        {
            if (resolucion < 0)
            {
                resolucion = 0;
                AlMensajeError.Invoke("ControlDeWebcam : ( resolucion < 0 ), seteando a 0...");
            }
            else if (resolucion >= Device.availableResolutions.Length)
            {
                resolucion = Device.availableResolutions.Length - 1;
                AlMensajeError.Invoke(string.Format("ControlDeWebcam : ( resolucion >= {0} ), seteando a cantResoluciones-1...", Device.availableResolutions.Length));
            }
            resolucionActual = resolucion;
            res = Device.availableResolutions[resolucion];
        }

        AlMensaje.Invoke("ControlDeWebcam : iniciando camara...");
        webCam = new WebCamTexture(Device.name, res.width, res.height, res.refreshRate);
        webCam.Play();
        AlMensaje.Invoke(string.Format("ControlDeWebcam : iniciada ({0},{1},{2})", webCam.width, webCam.height, webCam.requestedFPS));

        if (resDisponibles == null && Device.availableResolutions != null)
        {
            AlMensajeError.Invoke(string.Format("ControlDeWebcam : resoluciones encontradas tras iniciar = {0}", Device.availableResolutions.Length));
        }

        if (rawTextureDump)
        {
            AlMensaje.Invoke(string.Format("ControlDeWebcam : ubicando en Image y buscando aspectRatioFitter ({0:0.00})", webCam.width / (float)webCam.height));
            rawTextureDump.texture = webCam;
            var aspectRatioFitter = rawTextureDump.GetComponent<UnityEngine.UI.AspectRatioFitter>();
            if (aspectRatioFitter) aspectRatioFitter.aspectRatio = webCam.width / (float)webCam.height;
        }
    }

    WebCamTexture webCam;
    WebCamDevice Device
    {
        get
        {
            // if (indiceCamActual > -1 && indiceCamActual < WebCamTexture.devices.Length) return WebCamTexture.devices[indiceCamActual];
            // else return new WebCamDevice();
            return WebCamTexture.devices[Mathf.Clamp(indiceCamActual, 0, WebCamTexture.devices.Length - 1)];
        }
    }
}
