using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 169
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    /// <summary>
    /// Class responsible for handling command lines displayed on the screen.
    /// </summary>
    public class LineManager : MonoBehaviour {

        public event EventHandler<LineInstantiatedEventArgs> LineInstantiated;

	    private const string Prompt = "> ";

	    [SerializeField]
		private CommandLine firstLine;

	    [SerializeField]
	    private GameObject commandLineTemplate;

		// todo not needed anymore
	    [SerializeField]
	    private Transform container;

	    private readonly List<CommandLine> lines = new List<CommandLine>();

	    public CommandLine LastLine {
            get { return lines.LastOrDefault(); }
        }

        public CommandLine PenultimateLine
        {
            get {
                if (lines.Count > 1) {
                    var penultimate = lines[lines.Count - 2];
                    return penultimate;
                }
                return null;
            }
        }

	    public string CommandString {
		    get {
			    var cmd = LastLine.Text.Substring(Prompt.Length);
			    return cmd;
		    }
	    }

	    #region UNITY MESSAGES

	    private void Awake() {
            Assert.IsNotNull(commandLineTemplate);
			Assert.IsNotNull(firstLine);

			LineInstantiated += LineInstantiatedHandler;
			lines.Add(firstLine);
        }

	    private void OnEnable() {
		    LastLine.GetFocus();
			LastLine.MoveCarretToEnd();
	    }

	    #endregion

	    /// <summary>
        /// Handles all the stuff related to creation of a new command line
        /// </summary>
        public void AddNewLine() {
            InstantiateLine();
            RepositionLines();
        }

        private void RepositionLines() {
            // calculate offset (relative to global (0; 0))
            var correctPos = LastLine.Height/2;
            var pos = LastLine.transform.position.y;
            var offset = pos - correctPos;
            if (offset > 0) {
                return;
            }

            // update lines container position
            var verticalPos = container.position.y + Mathf.Abs(offset);
            container.position = new Vector3(container.position.x, verticalPos, container.position.z);
        }

        private void InstantiateLine() {
            var cmdLineGo = Instantiate(commandLineTemplate);
            cmdLineGo.gameObject.SetActive(true);
            cmdLineGo.transform.SetParent(container, false);

            OnLineInstantiated(cmdLineGo);
        }

        private void PositionLine() {
            var newLineRectTransform = LastLine.GetComponent<RectTransform>();
            var targetPos = CalculateLinePosition();
            newLineRectTransform.anchoredPosition = targetPos;
        }

        // calculates position to place line on the screen
        private Vector2 CalculateLinePosition() {
			var verticalOffset = (PenultimateLine.Height / 2) + (LastLine.Height / 2);
			var verticalPos = PenultimateLine.RectTransform.anchoredPosition.y - verticalOffset;
			var newPos = new Vector2(
				PenultimateLine.RectTransform.anchoredPosition.x,
				verticalPos);

			return newPos;
        }

        #region EVENT INVOCATORS

        protected virtual void OnLineInstantiated(GameObject instantiatedGo) {
            var handler = LineInstantiated;
            var args = new LineInstantiatedEventArgs(instantiatedGo);
            if (handler != null) handler(this, args);
        }

        #endregion

        #region EVENT HANDLERS

        private void LineInstantiatedHandler(object sender, LineInstantiatedEventArgs eventArgs) {
            var go = eventArgs.InstantiatedGo;
            var cmdLine = go.GetComponent<CommandLine>();

            lines.Add(cmdLine);
            PositionLine();
        }

        #endregion

	    public void GetFocus() {
		    LastLine.GetFocus();
	    }
    }

    public class LineInstantiatedEventArgs : EventArgs {

        public GameObject InstantiatedGo { get; private set; }

        public LineInstantiatedEventArgs(GameObject instantiatedGo) {
            InstantiatedGo = instantiatedGo;
        }
    }

}