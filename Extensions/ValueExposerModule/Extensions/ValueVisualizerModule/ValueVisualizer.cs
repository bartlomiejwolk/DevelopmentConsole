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

        private void Awake() {
            Assert.IsNotNull(_graphDrawerTemplate);
            GraphDrawerInstantiated += OnGraphDrawerInstantiated;
        }

        private void Update() {
            
        }

        private void OnGraphDrawerInstantiated(
            object sender, 
            GraphDrawerInstantiatedEventArgs args) {

            var go = args.GraphDrawerGo;
            // add go to GraphManager
        }

        public void VisualizeValue(
            string valueName,
            Func<object> valueDelegate,
            Transform container) {

            var go = InstantiateGraphDrawer(container);
            _graphManager.AddGraph(valueName, valueDelegate, go);
        }

        private GameObject InstantiateGraphDrawer(Transform container) {
            var go = Instantiate(_graphDrawerTemplate.gameObject);
            go.transform.SetParent(container);
            var args = new GraphDrawerInstantiatedEventArgs() {
                GraphDrawerGo = go
            };
            InvokeGraphDrawerInstantiatedEvent(args);
            return go;
        }

        protected virtual void InvokeGraphDrawerInstantiatedEvent(
            GraphDrawerInstantiatedEventArgs e) {

            var handler = GraphDrawerInstantiated;
            if (handler != null) handler(this, e);
        }
    }

    public class GraphDrawerInstantiatedEventArgs : EventArgs {
        public GameObject GraphDrawerGo { get; set; }
    }
}
