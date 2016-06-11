using System;
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

        public void EnableDrawing(string valueName) {
            var graphInfo = _graphManager.GetGraphByName(valueName);
            if (graphInfo != null) {
                graphInfo.Enable(true);
            }
        }

        public void DisableDrawing(string valueName) {
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
