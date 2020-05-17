using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OpenCvSharp;
using static OpenCvSharp.Unity;

public class CamQuadBlobs : MonoBehaviour
{
    public UnityEngine.UI.RawImage meterWebCamAcaAlArrancar;
    public int indiceCamInicial;
    int indiceCam = -1;
    public int procesarCada = 0;

    TextureConversionParams textureParameters;
    protected TextureConversionParams TextureParameters
    {
        get
        {
            if (textureParameters == null)
            {
                textureParameters = new TextureConversionParams();
            }
            textureParameters.FlipHorizontally = Device.isFrontFacing;
            // deal with rotation
            if (0 != WebCam.videoRotationAngle)
                textureParameters.RotationAngle = WebCam.videoRotationAngle; // cw -> ccw
            return textureParameters;
        }
    }
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
                webCam = new WebCamTexture(Device.name, 9999, 9999);
                webCam.Play();
                if (meterWebCamAcaAlArrancar) {
                    meterWebCamAcaAlArrancar.texture = webCam;
                    var aspectRatioFitter = meterWebCamAcaAlArrancar.GetComponent<UnityEngine.UI.AspectRatioFitter>();
                    if(aspectRatioFitter)aspectRatioFitter.aspectRatio = webCam.width/webCam.height;
                }
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
    WebCamDevice Device
    {
        get
        {
            if (indiceCam > -1 && indiceCam < WebCamTexture.devices.Length) return WebCamTexture.devices[indiceCam];
            else return new WebCamDevice();
        }
    }
    Resolution MejorResolution
    {
        get
        {
            if (Device.availableResolutions == null) return new Resolution();
            var resBase = Device.availableResolutions.Length > 0 ? Device.availableResolutions[0] : new Resolution();
            foreach (var res in Device.availableResolutions)
            {
                if (res.width * res.height + res.refreshRate > resBase.width * resBase.height + resBase.refreshRate)
                {
                    resBase = res;
                }
            }
            return resBase;
        }
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
        if (WebCam && WebCam.didUpdateThisFrame && (procesarCada==0 || WebCam.updateCount%procesarCada==0)) {
            Procesar();
        }
    }

    Mat matPrincipal;

    public void Procesar()
    {
        matPrincipal = TextureToMat(WebCam,TextureParameters);
        
    }

}
