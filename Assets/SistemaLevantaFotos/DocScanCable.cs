using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using static OpenCvSharp.Unity;

public class DocScanCable : MatCable
{
    [Range(0f,1f)]
    public float NoiseReduction = 0.7f;
    [Range(0f,1f)]
    public float EdgesTight = 0.9f;
    [Range(0f,1f)]
    public float ExpectedArea = 0.2f;
    public PaperScanner.ScannerSettings.ColorMode ColorMode = PaperScanner.ScannerSettings.ColorMode.Grayscale;

    private PaperScanner scanner = new PaperScanner();
     
     public override void Procesar(MatCable matCable){
         Size inputSize = new Size(matCable.Salida.Width, matCable.Salida.Height);
			// first of all, we set up scan parameters
			// 
			// scanner.Settings has more values than we use
			// (like Settings.Decolorization that defines
			// whether b&w filter should be applied), but
			// default values are quite fine and some of
			// them are by default in "smart" mode that
			// uses heuristic to find best choice. so,
			// we change only those that matter for us
			scanner.Settings.NoiseReduction = NoiseReduction;											// real-world images are quite noisy, this value proved to be reasonable
			scanner.Settings.EdgesTight = EdgesTight;												// higher value cuts off "noise" as well, this time smaller and weaker edges
			scanner.Settings.ExpectedArea = ExpectedArea;											// we expect document to be at least 20% of the total image area
			scanner.Settings.GrayMode = ColorMode;	// color -> grayscale conversion mode

			scanner.Input = matCable.Salida.Clone();
            
            // should we fail, there is second try - HSV might help to detect paper by color difference
			if (!scanner.Success)
				// this will drop current result and re-fetch it next time we query for 'Success' flag or actual data
				scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.HueGrayscale;

            salida = scanner.Output;
     }
    // public void Procesar(){
    //     if(Entrada == null){

	// 		Size inputSize = new Size(Entrada.Width, Entrada.Height);
            
	// 		// first of all, we set up scan parameters
	// 		// 
	// 		// scanner.Settings has more values than we use
	// 		// (like Settings.Decolorization that defines
	// 		// whether b&w filter should be applied), but
	// 		// default values are quite fine and some of
	// 		// them are by default in "smart" mode that
	// 		// uses heuristic to find best choice. so,
	// 		// we change only those that matter for us
	// 		scanner.Settings.NoiseReduction = 0.7;											// real-world images are quite noisy, this value proved to be reasonable
	// 		scanner.Settings.EdgesTight = 0.9;												// higher value cuts off "noise" as well, this time smaller and weaker edges
	// 		scanner.Settings.ExpectedArea = 0.2;											// we expect document to be at least 20% of the total image area
	// 		scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.Grayscale;	// color -> grayscale conversion mode

	// 		// process input with PaperScanner
	// 		Mat result = null;
	// 		scanner.Input = new Mat(Entrada);

	// 		// should we fail, there is second try - HSV might help to detect paper by color difference
	// 		if (!scanner.Success)
	// 			// this will drop current result and re-fetch it next time we query for 'Success' flag or actual data
	// 			scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.HueGrayscale;

    //         Salida = scanner.Output;
    //     }
    // }
}