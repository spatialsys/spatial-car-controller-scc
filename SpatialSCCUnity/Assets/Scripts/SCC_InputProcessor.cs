using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCC_InputProcessor : MonoBehaviour{

    public SCC_Inputs inputs = new SCC_Inputs();        //  Target inputs.
    public bool smoothInputs = true;        //  Smoothly lerp the inputs?
    public float smoothingFactor = 5f;      //  Smoothing factor.

    public void OverrideInputs(SCC_Inputs newInputs) {
        if (!smoothInputs) {
            inputs = newInputs;
        } else {
            inputs.throttleInput = Mathf.MoveTowards(inputs.throttleInput, newInputs.throttleInput, Time.deltaTime * smoothingFactor);
            inputs.steerInput = Mathf.MoveTowards(inputs.steerInput, newInputs.steerInput, Time.deltaTime * smoothingFactor);
            inputs.brakeInput = Mathf.MoveTowards(inputs.brakeInput, newInputs.brakeInput, Time.deltaTime * smoothingFactor);
            inputs.handbrakeInput = Mathf.MoveTowards(inputs.handbrakeInput, newInputs.handbrakeInput, Time.deltaTime * smoothingFactor);

        }
    }

}
