using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteCallback : MonoBehaviour
{
    public Test2 test2;


    private void Awake()
    {
        test2=GetComponent<Test2>();

    }
    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            Debug.Log($"Device changed {device.name}");

            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) => {
                // Note that you can't use note.velocity because the state
                // hasn't been updated yet (as this is "will" event). The note
                // object is only useful to specify the target note (note
                // number, channel number, device name, etc.) Use the velocity
                // argument as an input note velocity.
                test2.ToggleNote(true,note.noteNumber);

                Debug.Log(string.Format(
                    "Note On #{0} ({1}) vel:{2:0.00} ch:{3} dev:'{4}'",
                    note.noteNumber,
                    note.shortDisplayName,
                    velocity,
                    (note.device as Minis.MidiDevice)?.channel,
                    note.device.description.product
                ));
            };
            midiDevice.onWillControlChange += MidiDevice_onWillControlChange;
            midiDevice.onWillNoteOff += (note) => {
                test2.ToggleNote(false, note.noteNumber);
                //Debug.Log(string.Format(
                //    "Note Off #{0} ({1}) ch:{2} dev:'s
                //    note.shortDisplayName,
                //    (note.device as Minis.MidiDevice)?.channel,
                //    note.device.description.product
                //));
            };
        };
    }

    private void MidiDevice_onWillControlChange(Minis.MidiValueControl arg1, float arg2)
    {
        //if (arg1.controlNumber == 64)
        Debug.Log($"Midi control changed {arg1.controlNumber} value: {arg2}");
            test2.TogglePedal(arg2 > 0);
    }
}
