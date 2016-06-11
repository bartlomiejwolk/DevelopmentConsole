﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {

    public class ValueVisualizer : MonoBehaviour {

        [SerializeField]
        private GameObject _graphPlotterTemplate;

        [SerializeField]
        private GameObject _container;

        private readonly GraphManager _graphManager = new GraphManager();

        public event EventHandler<GraphDrawerInstantiatedEventArgs> GraphDrawerInstantiated;

        #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(_graphPlotterTemplate);
            Assert.IsNotNull(_container);

            GraphDrawerInstantiated += OnGraphDrawerInstantiated;
        }

        private void Update() {
            foreach (var valueGraph in _graphManager.GraphInfos) {
                if (!valueGraph.Enabled) {
                    continue;
                }
                // todo there should be separate delegates. One for each return type.
                var value = valueGraph.ValueDelegate();
                HandleInstantiateGraphPlotter(valueGraph, valueGraph.Position);
                valueGraph.GraphPlotter.DrawFloatValuePoint((float)value);
            }
        }

        #endregion

        // todo remove redundant overloads
        public void RegisterValue(
            string valueName,
            Func<object> valueDelegate,
            Vector3 position) {

            RegisterValue(valueName, valueDelegate);
        }

        public void RegisterValue(
            string valueName,
            Func<object> valueDelegate,
            Vector3 position,
            bool enable) {

            RegisterValue(valueName, valueDelegate);
        }

        public GraphInfo RegisterValue(
            string valueName,
            Func<object> valueDelegate) {

            var valueGraph = new GraphInfo() {
                ValueName = valueName,
                ValueDelegate = valueDelegate,
            };
            _graphManager.AddGraph(valueGraph);

            return valueGraph;
        }

        // todo rename to EnableDrawing
        public void VisualizeValue(string valueName) {
            var graphInfo = _graphManager.GetGraphByName(valueName);
            if (graphInfo != null) {
                graphInfo.Enable(true);
            }
        }

        // todo rename to DisableDrawing
        public void StopVisualizingValue(string valueName) {
            throw new NotImplementedException();
        }

        private void HandleInstantiateGraphPlotter(GraphInfo valueGraph, Vector3 position) {
            if (valueGraph.Go != null) {
                return;
            }

            var go = Instantiate(_graphPlotterTemplate);
            go.transform.SetParent(_container.transform);
            go.transform.position = position;
            
            var args = new GraphDrawerInstantiatedEventArgs() {
                ValueName = valueGraph.ValueName,
                Go = go
            };
            InvokeGraphDrawerInstantiatedEvent(args);
        }

        #region EVENT INVOCATORS

        protected virtual void InvokeGraphDrawerInstantiatedEvent(
            GraphDrawerInstantiatedEventArgs e) {

            var handler = GraphDrawerInstantiated;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region EVENT HANDLERS

        private void OnGraphDrawerInstantiated(
            object sender,
            GraphDrawerInstantiatedEventArgs args) {

            var graphInfo = _graphManager.GetGraphByName(args.ValueName);
            graphInfo.Go = args.Go;
            // adjust size
            graphInfo.RectTransform.sizeDelta = graphInfo.Size;
        }

        #endregion
    }
}
