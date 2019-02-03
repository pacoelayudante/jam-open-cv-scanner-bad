using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using static OpenCvSharp.Unity;

public class CapturadorWebcamMat : MatCable
{
		protected TextureConversionParams TextureParameters { get; private set; }
public RawImage meterWebCamAcaAlArrancar;
    public int indiceCamInicial;
    int indiceCam = -1;
    public int procesarCada = 0;
    public int IndiceCam
    {
        get
        {
            return indiceCam;
        }
        set
        {
            if (value > 0 && indiceCam != value && webCam)
            {
                webCam.Stop();
                webCam = null;
            }
            indiceCamInicial = indiceCam = value;
            if (enabled && indiceCam > -1 && indiceCam < WebCamTexture.devices.Length)
            {
                var mejorRes = MejorResolution;
                webCam = new WebCamTexture(Device.name,9999,9999);
                webCam.Play();
                if(meterWebCamAcaAlArrancar)meterWebCamAcaAlArrancar.texture = webCam;
                salida = TextureToMat(webCam,TextureParameters);
            }
        }
    }
    WebCamTexture webCam;
    public WebCamTexture WebCam
    {
        get
        {
            return webCam;
        }
    }
    WebCamDevice Device{
        get{
            if(indiceCam > -1 && indiceCam < WebCamTexture.devices.Length) return WebCamTexture.devices[indiceCam];
            else return new WebCamDevice();
        }
    }
    Resolution MejorResolution{
        get{
            if(Device.availableResolutions==null) return new Resolution();
            var resBase = Device.availableResolutions.Length > 0 ? Device.availableResolutions[0] : new Resolution();
            foreach(var res in Device.availableResolutions){
                if (res.width*res.height+res.refreshRate > resBase.width*resBase.height+resBase.refreshRate){
                    resBase = res;
                }
            }
            return resBase;
        }
    }

    public override void Procesar(MatCable matCable){
        salida = matCable.Salida;        
    }
    
    /// <summary>
    /// This method scans source device params (flip, rotation, front-camera status etc.) and
    /// prepares TextureConversionParameters that will compensate all that stuff for OpenCV
    /// </summary>
    private void ReadTextureConversionParameters()
    {
        TextureConversionParams parameters = new TextureConversionParams();

        // frontal camera - we must flip around Y axis to make it mirror-like
        parameters.FlipHorizontally = Device.isFrontFacing;
        
        // TODO:
        // actually, code below should work, however, on our devices tests every device except iPad
        // returned "false", iPad said "true" but the texture wasn't actually flipped

        // compensate vertical flip
        //parameters.FlipVertically = webCamTexture.videoVerticallyMirrored;
        
        // deal with rotation
        if (0 != WebCam.videoRotationAngle)
            parameters.RotationAngle = WebCam.videoRotationAngle; // cw -> ccw

        // apply
        TextureParameters = parameters;

        //UnityEngine.Debug.Log (string.Format("front = {0}, vertMirrored = {1}, angle = {2}", webCamDevice.isFrontFacing, webCamTexture.videoVerticallyMirrored, webCamTexture.videoRotationAngle));
    }

    private void OnEnable()
    {
        if (WebCam) WebCam.Play();
    }
    private void OnDisable()
    {
        if (WebCam) WebCam.Stop();
    }

    private void Update()
    {
        if (indiceCamInicial != IndiceCam)
        {
            IndiceCam = indiceCamInicial;
        }
        if (WebCam && WebCam.didUpdateThisFrame && (procesarCada==0 || Time.frameCount%procesarCada==0)) {
            salida = TextureToMat(WebCam,TextureParameters);
            AlActualizar.Invoke(this);
        }
    }
}
