using UnityEngine;
using System.Collections;
using DevelopmentConsole.Extensions.ValueExposerModule.Core;

public class ValueExposerTest : MonoBehaviour {
    void Start() {
        ExposedValueManager.Instance.RegisterValue("framecount", "cat 1", () => Time.frameCount);
        ExposedValueManager.Instance.RegisterValue("timesincelevelload", "cat 1", () => Time.timeSinceLevelLoad);
        ExposedValueManager.Instance.RegisterValue("string", "cat 2", () => "some string");
    }

    void Update() {}
}