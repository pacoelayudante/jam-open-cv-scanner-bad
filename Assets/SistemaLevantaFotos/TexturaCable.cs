using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using IntPtr = System.IntPtr;
using System.Runtime.InteropServices;
using static OpenCvSharp.Unity;

public class TexturaCable : MatCable
{
    public Texture2D textura;
    public bool modoSeguro = true;
    public bool autoApagar = true;

    public override void Procesar(MatCable matCable)
    {
        salida = matCable.Salida;
    }

    private void Update()
    {
        if (textura)
        {
            if (salida == null || modoSeguro)
            {
                salida = TextureToMat(textura);
            }
            else
            {
                try
                {
                    // textura.LoadRawTextureData(salida.CvPtr,salida.Cols*salida.Rows);
                    Color32[] pixels32 = textura.GetPixels32();
                    GCHandle gcHandle = GCHandle.Alloc(pixels32, GCHandleType.Pinned);

                    // NO mistake here - we negate flipVertically as it's necessary due to Unity and OpenCV storing images differently and Unity's texture is always
                    // vertically flipped for OpenCV. So, this trick allows to avoid user headache about the matter, leaving him thinking about
                    // his transforms only
                    IntPtr matPtr = utils_texture_to_mat(gcHandle.AddrOfPinnedObject(), salida.Width, salida.Height, true, false, 0);
                    salida = new Mat(matPtr);
                    gcHandle.Free();
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                    enabled = false;
                }
            }
            AlActualizar.Invoke(this);
            enabled = !autoApagar;
        }
    }
}
