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

        // todo remove. Replace with list.
        private readonly GraphManager _graphManager = new GraphManager();

        public event EventHandler<GraphDrawerInstantiatedEventArgs> GraphDrawerInstantiated;

        #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(_graphPlotterTemplate);
            Assert.IsNotNull(_container);

            GraphDrawerInstantiated += OnGraphDrawerInstantiated;
        }

        private void Update() {
            foreach (var valueGraph in _graphManager.ValueGraphs) {
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

        public void RegisterValue(
            string valueName,
            Func<object> valueDelegate,
            Vector3 position) {

            // todo check if plotter does not exist already. If it does, override existing GraphInfo (remove it first from the _graphManager).
            //var go = InstantiateGraphPlotter(position);

            var valueGraph = new GraphInfo() {
                ValueName = valueName,
                ValueDelegate = valueDelegate,
                Position = position
                //Go = go
            };
            _graphManager.AddGraph(valueGraph);
        }

        public void RegisterValue(
            string valueName,
            Func<object> valueDelegate,
            Vector3 position,
            bool enable) {

            var size = new Vector2(80, 20);
            RegisterValue(valueName, valueDelegate, position, size, enable);
        }

        public void RegisterValue(
            string valueName,
            Func<object> valueDelegate,
            Vector3 position,
            Vector2 size,
            bool enable) {

            RegisterValue(valueName, valueDelegate, position);
            _graphManager.EnableGraph(valueName);
        }

        // todo rename to ShowValue
        public void VisualizeValue(string valueName) {
            var graphInfo = _graphManager.GetGraphByName(valueName);
            if (graphInfo != null) {
                graphInfo.Enabled = true;
            }
        }

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

            var graphInfo = _graphManager.ValueGraphs.Last();
            graphInfo.Go = args.Go;
        }

        #endregion
    }
}
