using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlDeWebcamDropDown : MonoBehaviour
{
    public Dropdown dropdown;
    public Dropdown dropdownResolutions;

    private void Start()
    {
        var ops = new List<string>();
        foreach (var item in WebCamTexture.devices)
        {
            ops.Add(item.name);
        }
        dropdown.AddOptions(ops);
        if (dropdownResolutions) CargarResoluciones(0);
    }

    public void CargarResoluciones(int i)
    {
        var ops = new List<string>();
        if (WebCamTexture.devices[i].availableResolutions == null)
        {
            ops.Add("Sin Resoluciones");
            return;
        }
        else
        {
            foreach (var item in WebCamTexture.devices[i].availableResolutions)
            {
                ops.Add(item.width + "x" + item.height + "@" + item.refreshRate);
            }
        }
        dropdownResolutions.ClearOptions();
        dropdownResolutions.AddOptions(ops);
    }
}
