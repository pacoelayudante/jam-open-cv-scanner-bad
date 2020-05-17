using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDump : UnityEngine.UI.Text
{
    public bool saltoDeLineaAutomatico = true;

    public void Sumar(string entrada){
        if(saltoDeLineaAutomatico && text.Length>0) text+="\n";
        text += entrada;
    }
    public void Limpiar(){
        text = string.Empty;
    }
}
