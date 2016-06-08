using System;
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
            foreach (var valueGraph in _graphManager.ValueGraphs) {
                if (!valueGraph.Enabled) {
                    continue;
                }
                var value = valueGraph.ValueDelegate();
                valueGraph.GraphPlotter.DrawValuePoint(value);
            }
        }

        #endregion

        public void VisualizeValue(
            string valueName,
            Func<object> valueDelegate,
            Vector3 position) {

            // todo check if plotter does not exist already
            var go = InstantiateGraphPlotter(position);

            var valueGraph = new GraphInfo() {
                ValueName = valueName,
                ValueDelegate = valueDelegate,
                Go = go
            };
            _graphManager.AddGraph(valueGraph);
            valueGraph.Enabled = true;
        }

        public void VisualizeValue(string valueName) {
            throw new NotImplementedException();
        }

        public void StopVisualizingValue(string valueName) {
            throw new NotImplementedException();
        }

        private GameObject InstantiateGraphPlotter(Vector3 position) {
            var go = Instantiate(_graphPlotterTemplate);
            go.transform.SetParent(_container.transform);
            go.transform.position = position;
            
            var args = new GraphDrawerInstantiatedEventArgs();
            InvokeGraphDrawerInstantiatedEvent(args);
            
            return go;
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
            GraphDrawerInstantiatedEventArgs args) {}

        #endregion
    }
}
