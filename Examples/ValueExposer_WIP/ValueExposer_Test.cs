using UnityEngine;
using System.Collections;
using DevelopmentConsole.Extensions.ValueExposerModule.Core;
using DevelopmentConsole.Extensions.ValueExposerModule.Extensions;
using UnityEngine.Assertions;

#pragma warning disable 649

public class ValueExposer_Test : MonoBehaviour {
    [SerializeField]
    private ExtendedValueExposer _valueExposer;

    void Awake() {
        Assert.IsNotNull(_valueExposer);
    }

    void Start() {
        ExposedValueManager.Instance.RegisterValue("deltaTime", "cat 1", () => Time.timeSinceLevelLoad%0.5f);
        ExposedValueManager.Instance.RegisterValue("other", "cat 1", () => Time.timeSinceLevelLoad%0.9f);

        _valueExposer.ShowValue("deltaTime");
        _valueExposer.ShowValue("other");
    }

    void Update() {}
}