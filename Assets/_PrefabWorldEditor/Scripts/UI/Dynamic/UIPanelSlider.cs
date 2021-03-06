﻿//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;

namespace PrefabWorldEditor
{
	public class UIPanelSlider : MonoBehaviour
    {
		public Text label;
		public Slider slider;

		public delegate void ChangeHandler(int elementIndex, float value);
		public event ChangeHandler changeHandler;

        public int elementIndex;

		// ------------------------------------------------------------------------
		public void onValueChange(float value) {

			if (changeHandler != null) {
                changeHandler(elementIndex, value);
			}
		}
	}
}