using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

public class Test2 : MonoBehaviour
{
    public VisualEffect visualEffect;

    VFXEventAttribute eventAttribute;

    int eventIndex;

    List<int> particlesToSpawn = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        eventAttribute = visualEffect.CreateVFXEventAttribute();
    }

    int notesMatrix1 = 0, notesMatrix2 = 0, notesMatrix3 = 0;
    bool pedalState;
    int noteIndexId;
    int noteIdEventPar;
    int pressEventId1;
    int pedalStateId;

    private void Awake()
    {
        eventIndex = 0;
        noteIndexId = Shader.PropertyToID("NoteIndex");
        pedalStateId = Shader.PropertyToID("Pedal");
        noteIdEventPar = Shader.PropertyToID("NoteId");
        pressEventId1 = Shader.PropertyToID("Note1");
    }



    public void ToggleNote(bool toActive, int noteIndex)
    {
        noteIndex -= 21;

        lock (visualEffect)
        {

            if (noteIndex > 61)
            {
                SetMatrix(toActive, noteIndex, ref notesMatrix3);
                visualEffect.SetInt("NotesMatrix3", notesMatrix3);
            }
            else if (noteIndex > 31)
            {
                SetMatrix(toActive, noteIndex, ref notesMatrix2);
                visualEffect.SetInt("NotesMatrix2", notesMatrix2);
            }
            else
            {
                SetMatrix(toActive, noteIndex, ref notesMatrix1);
                visualEffect.SetInt("NotesMatrix1", notesMatrix1);
            }

            if (toActive)
            {
                Debug.LogWarning($"Toggle Note {noteIndex} eventIndex {eventIndex}");

                //lock (visualEffect)
                //{

                //visualEffect.SetInt(noteIndexId, noteIndex);

                particlesToSpawn.Add(noteIndex);
                //eventAttribute.SetInt(noteIdEventPar, noteIndex);

                //visualEffect.SendEvent(pressEventId1, eventAttribute);


                //await Task.Delay(10);
                //}
            }
        }
    }

    public void TogglePedal(bool on)
    {
        //Debug.Log($"Setting on sate: {on}");
        if (on == pedalState) return;
        //Debug.Log($" After val -  Setting on sate: {on}");

        pedalState = on;
        visualEffect.SetBool(pedalStateId, on);
    }


    private void SetMatrix(bool noteActive, int index, ref int matrix)
    {
        if (!noteActive) matrix &= ~(1 << index); else matrix |= 1 << index;
    }


    // Update is called once per frame
    void Update()
    {


        for (int number = 0; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()))
                ToggleNote(true, 21+number*4);
        }
        for (int number = 0; number <= 9; number++)
        {
            if (Input.GetKeyUp(number.ToString()))
                ToggleNote(false, 21+number*4);
        }





        if (particlesToSpawn.Count <= 0) return;

        Debug.Log("Spawning particle");


        eventAttribute.SetInt(noteIdEventPar, particlesToSpawn[0]);
        visualEffect.SendEvent(pressEventId1, eventAttribute);

        particlesToSpawn.RemoveAt(0);
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    visualEffect.SetInt("Index", index);
        //    visualEffect.SendEvent("Note");
        //    index++;
        //}

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    visualEffect.SetInt("Alive", 2);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    visualEffect.SetInt("Alive", 7);
        //}
    }

    private void LateUpdate()
    {
        eventIndex = 0;
    }
}
