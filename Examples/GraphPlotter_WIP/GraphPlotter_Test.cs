﻿using UnityEngine;
using System.Collections;
using DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule;
using UnityEngine.Assertions;
#pragma warning disable 649

public class GraphPlotter_Test : MonoBehaviour {

    [SerializeField]
    private ValueVisualizer _valueVisualizer;

	void Start () {
        Assert.IsNotNull(_valueVisualizer);

        _valueVisualizer.RegisterValue("Random value", () => RandomValueGenerator(1, 10), Vector3.zero);
        _valueVisualizer.VisualizeValue("Random value");
	}
	
	void Update () {
	
	}

    private float RandomValueGenerator(int min, int max) {
        var randomValue = Random.Range(min, max);
        return randomValue;
    }
}
