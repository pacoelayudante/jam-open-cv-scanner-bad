namespace OpenCvSharp.Demo
{
    using UnityEngine;
    using System.Collections;
    using OpenCvSharp;
    using UnityEngine.UI;

    public class ContoursByShapeWebcam : WebCamera
    {

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
        [Range(1, 254)]
        public int umbral = 127;
        public bool capturarSiguiente;
        Mat capturada;

        protected override void Awake()
        {
            base.Awake();
            base.forceFrontalCamera = true; // we work with frontal cams here, let's force it for macOS s MacBook doesn't state frontal cam correctly
        }

        protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
        {
            //Load texture
            Mat image = Unity.TextureToMat(input, TextureParameters);
            if (capturarSiguiente)
            {
                capturada = image;
				capturarSiguiente = false;
            }
            //Gray scale image
            // Mat grayMat = new Mat();
            // Cv2.CvtColor (image, grayMat, ColorConversionCodes.BGR2GRAY); 

            // Mat thresh = new Mat ();
            // Cv2.Threshold (grayMat, thresh, umbral, 255, ThresholdTypes.BinaryInv);

            // Render texture
            output = Unity.MatToTexture(image, output);
            return true;
        }

        void LateUpdate()
        {
            if (capturada != null)
            {
                //Gray scale image
                Mat grayMat = new Mat();
                Cv2.CvtColor(capturada, grayMat, ColorConversionCodes.BGR2GRAY);
                Mat thresh = new Mat();
                Cv2.Threshold(grayMat, thresh, umbral, 255, ThresholdTypes.BinaryInv);
                var textura = Unity.MatToTexture(thresh);
                // RawImage rawImage = gameObject.GetComponent<RawImage> ();
                if (RawImage) RawImage.texture = textura;
            }
        }

        // Use this for initialization
        void Procesar()
        {
            //Load texture
            Mat image = null;//Unity.TextureToMat (this.texture);

            //Gray scale image
            Mat grayMat = new Mat();
            Cv2.CvtColor(image, grayMat, ColorConversionCodes.BGR2GRAY);

            Mat thresh = new Mat();
            Cv2.Threshold(grayMat, thresh, 127, 255, ThresholdTypes.BinaryInv);


            // Extract Contours
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(thresh, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxNone, null);

            foreach (Point[] contour in contours)
            {
                double length = Cv2.ArcLength(contour, true);
                Point[] approx = Cv2.ApproxPolyDP(contour, length * 0.01, true);
                string shapeName = null;
                Scalar color = new Scalar();


                if (approx.Length == 3)
                {
                    shapeName = "Triangle";
                    color = new Scalar(0, 255, 0);
                }
                else if (approx.Length == 4)
                {
                    OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);
                    if (rect.Width / rect.Height <= 0.1)
                    {
                        shapeName = "Square";
                        color = new Scalar(0, 125, 255);
                    }
                    else
                    {
                        shapeName = "Rectangle";
                        color = new Scalar(0, 0, 255);
                    }
                }
                else if (approx.Length == 10)
                {
                    shapeName = "Star";
                    color = new Scalar(255, 255, 0);
                }
                else if (approx.Length >= 15)
                {
                    shapeName = "Circle";
                    color = new Scalar(0, 255, 255);
                }

                if (shapeName != null)
                {
                    Moments m = Cv2.Moments(contour);
                    int cx = (int)(m.M10 / m.M00);
                    int cy = (int)(m.M01 / m.M00);

                    // Cv2.DrawContours(image, new Point[][] {contour}, 0, color, -1);
                    Cv2.PutText(image, shapeName, new Point(cx - 50, cy), HersheyFonts.HersheySimplex, 1.0, new Scalar(0, 0, 0));
                }
            }


            // Render texture
            Texture2D texture = Unity.MatToTexture(image);
            RawImage rawImage = gameObject.GetComponent<RawImage>();
            rawImage.texture = texture;


        }

    }
}