using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class ProjectionController : MonoBehaviour
{
    [ColorUsage(true,true)]
    public Color fireColor;
    [ColorUsage(true, true)]
    public Color waterColor;
    [ColorUsage(true, true)]
    public Color windColor;
    [ColorUsage(true, true)]
    public Color earthColor;
    [ColorUsage(true, true)]
    public VisualEffect graph;


    public bool hasFire;
    public bool hasWater;
    public bool hasWind;
    public bool hasEarth;

    Color mixColor;

    int colorParId;

    public List<Color> colorList = new List<Color>();
    void Start()
    {
        graph.playRate = 9;
        colorParId= Shader.PropertyToID("col");
        InputSystem.onDeviceChange += (device, change) =>
        {
            Debug.Log($"Device changed {device.name}");

            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;
            midiDevice.onWillControlChange += MidiDevice_onWillControlChange;
        };

    }

    private void MidiDevice_onWillControlChange(Minis.MidiValueControl arg1, float arg2)
    {
        //if (arg1.controlNumber == 64)
        Debug.Log($"Midi control control {arg1.controlNumber} value: {arg2}");


        //10 fire, 11earth, 12wind ,13 water  
        if (arg1.controlNumber == 10)
            hasFire = arg2 > 0;
        else if (arg1.controlNumber == 11)
            hasEarth = arg2 > 0;
        else if (arg1.controlNumber == 13)
            hasWind = arg2 > 0;
        else if (arg1.controlNumber == 12   )
            hasWater = arg2 > 0;
    }


    void Update()
    {
        colorList.Clear();


        if (hasFire)
            colorList.Add(fireColor);

        if (hasEarth)
            colorList.Add(earthColor);

        if (hasWind)
            colorList.Add(windColor);

        if (hasWater)
            colorList.Add(waterColor);

        mixColor = Color.black;

        foreach (var item in colorList)
            mixColor += item;


        mixColor /= colorList.Count;

        graph.SetVector4(colorParId, mixColor);

    }
}
