using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DevelopmentConsole.Core.CodeCompletionModule {

	public class PositionCalculator {

		private Text _target;
		private float _containerHeight;

		private Vector3 TargetGlobalPos {
			get {
				if (_target == null) {
					return Vector3.zero;
				}
				return _target.transform.position;
			}
		}

		private float TargetTextHeight {
			get {
				if (_target == null) {
					return 0;
				}
				return _target.rectTransform.rect.height;
			}
		}

		public Vector3 CalculatePosition(Text target, float containerHeight) {
			_target = target;
			_containerHeight = containerHeight;

			var charGlobalPos = CalculateLastCharGlobalPosition();
			Vector3 resultPos;
			if (CanPositionBelowTarget()) {
				var textLowerEdgePos = TargetGlobalPos.y - (TargetTextHeight/2);
				resultPos = new Vector3(charGlobalPos.x, textLowerEdgePos, 0);
			}
			else {
				var textUpperEdgePos = TargetGlobalPos.y + (TargetTextHeight / 2);
				var containerUpperPos = textUpperEdgePos + _containerHeight;
				resultPos = new Vector3(charGlobalPos.x, containerUpperPos, 0);
			}
			return resultPos;
		}

		private Vector2 CalculateLastCharGlobalPosition() {
			var gen = _target.cachedTextGenerator;
			var charInfo = gen.characters.Last();
			var x = (charInfo.cursorPos.x + charInfo.charWidth) / _target.pixelsPerUnit;
			var y = charInfo.cursorPos.y / _target.pixelsPerUnit;
			var localPos = new Vector2(x, y);
			var hLocalPos = TargetGlobalPos.x + localPos.x;
			var globalPos = new Vector2(hLocalPos, y);
			return globalPos;
		}

		private bool CanPositionBelowTarget() {
			var textLowerEdgePos = TargetGlobalPos.y - (TargetTextHeight / 2);
			var containerBottomEdge = textLowerEdgePos - _containerHeight;
			if (containerBottomEdge < 0) {
				return false;
			}
			return true;
		}
	}
}