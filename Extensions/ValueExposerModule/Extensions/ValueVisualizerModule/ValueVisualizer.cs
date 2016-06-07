using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {

    public class ValueVisualizer : MonoBehaviour {

        [SerializeField]
        private GraphPlotter _graphPlotterTemplate;

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
                valueGraph.DrawValuePoint();
            }
        }

        #endregion

        public void VisualizeValue(
            string valueName,
            Func<object> valueDelegate,
            Vector3 position) {

            var go = InstantiateGraphDrawer(position);
            var valueGraph = _graphManager.AddGraph(valueName, valueDelegate, go);
            valueGraph.Enabled = true;
        }

        private GameObject InstantiateGraphDrawer(Vector3 position) {
            var go = Instantiate(_graphPlotterTemplate.gameObject);
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
