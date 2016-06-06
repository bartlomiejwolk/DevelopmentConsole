using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {

    public class ValueVisualizer : MonoBehaviour {

        [SerializeField]
        private GraphDrawer _graphDrawerTemplate;

        private readonly GraphManager _graphManager = new GraphManager();

        public event EventHandler<GraphDrawerInstantiatedEventArgs> GraphDrawerInstantiated;

        #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(_graphDrawerTemplate);
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
            Transform container) {

            var go = InstantiateGraphDrawer(container);
            var valueGraph = _graphManager.AddGraph(valueName, valueDelegate, go);
            valueGraph.Enabled = true;
        }

        private GameObject InstantiateGraphDrawer(Transform container) {
            var go = Instantiate(_graphDrawerTemplate.gameObject);
            go.transform.SetParent(container);
            
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

    // todo extract to file
    public class GraphDrawerInstantiatedEventArgs : EventArgs {
    }
}
