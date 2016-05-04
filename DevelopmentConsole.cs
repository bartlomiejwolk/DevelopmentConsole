using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Extensions.DevelopmentConsole {

    [RequireComponent(typeof(LineManager))]
    public class DevelopmentConsole : MonoBehaviour {

        [SerializeField]
        private LineManager lineManager;

        private void Awake() {
            Assert.IsNotNull(lineManager);
        }

    }
}