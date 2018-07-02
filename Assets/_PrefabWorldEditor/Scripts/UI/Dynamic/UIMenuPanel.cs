//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class UIMenuPanel : MonoBehaviour
    {
		#region PublicAttributes

		public Text txtHeader;
		public ScrollRect scrollRect;
		public Transform btnContainer;

		public delegate void ClickHandler(int index);
		public event ClickHandler clickHandler;

		#endregion

		//

		#region PrivateAttributes

		private List<Button> _goButtons;
		private int _numButtons;

		#endregion

		//

		//#region GettersAndSetters
		//#endregion

		//

		#region PublicMethods

		// ---------------------------------------------------------------------------------------------
		public void init()
		{
			_goButtons = new List<Button> ();
			foreach (Transform child in btnContainer) {
				_goButtons.Add (child.GetComponent<Button>());
			}
			_numButtons = _goButtons.Count;
		}

		// ---------------------------------------------------------------------------------------------
		public void show(bool state)
		{
			gameObject.SetActive (state);
		}

		// ---------------------------------------------------------------------------------------------
		public void populate(string header, string[] options, Color[] colors, int selectedOption)
		{
			clear ();

			txtHeader.text = header;

			int i, len = options.Length;
			for (i = 0; i < len; ++i)
			{
				if (i < _numButtons)
				{
					_goButtons [i].gameObject.SetActive (true);

					_goButtons[i].interactable = (i != selectedOption);

					ColorBlock colorBlock = _goButtons [i].colors;
					colorBlock.normalColor = colors[i];
					_goButtons [i].colors = colorBlock;

					Transform trfmText = _goButtons [i].transform.Find ("Text");
					if (trfmText != null) {
						trfmText.GetComponent<Text> ().text = options [i];
					}
				}
			}

            scrollRect.movementType = (len > 11 ? ScrollRect.MovementType.Elastic : ScrollRect.MovementType.Clamped);
        }

		// ---------------------------------------------------------------------------------------------
		public void clear()
		{
			int i, len = _goButtons.Count;
			for (i = 0; i < len; ++i) {
				_goButtons [i].gameObject.SetActive (false);
			}
		}

		#endregion

		#region PrivateMethods

		//

		#endregion

		//

		#region EventHandlers

		// ------------------------------------------------------------------------
		public void onButtonClick(int index) {

			if (clickHandler != null) {
				clickHandler (index);
			}
		}

		#endregion
	}
}