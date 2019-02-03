using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using static OpenCvSharp.Unity;

public class ContourClipCable : MatCable
{
    public Sprite sprite;
    public ParticleSystem partSys;
    public MatCable original;

    override public void Procesar(MatCable matCable)
    {
        if (!matCable || matCable.Salida == null || matCable.SalidaPts == null) return;
        puntos = matCable.SalidaPts;
        if(original && original.Salida!=null) salida = original.Salida;
        else salida = matCable.Salida.Clone();

        if (puntos.Length > 0)
        {
            var split = new List<Mat>(salida.Split());
            if (split.Count == 3) split.Add(new Mat(salida.Height, salida.Width, MatType.CV_8UC1, new Scalar(0)));

            Cv2.DrawContours(split[split.Count - 1], puntos, 0, new Scalar(255), -1);
            Cv2.Merge(split.ToArray(), salida);

            var boundBox = Cv2.BoundingRect(puntos[0]);
            salida = new Mat(salida, boundBox);
        }
    }

    public void Fiesta()
    {
        var texturaSprite = MatToTexture(salida);
        sprite = Sprite.Create(texturaSprite, new UnityEngine.Rect(0, 0, texturaSprite.width, texturaSprite.height), UnityEngine.Vector2.one / 2f, 75, 1, SpriteMeshType.FullRect, new Vector4(0, 0, 0, 0), false);
        if (partSys)
        {
            var textAnim = partSys.textureSheetAnimation;
            textAnim.SetSprite(0, sprite);
            partSys.Play();
        }
    }
}
