//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;

namespace PrefabWorldEditor
{
	public class UIPanelDropdown : MonoBehaviour
    {
		public Text label;
		public Dropdown dropdown;

		public delegate void ChangeHandler(int elementIndex, int value);
		public event ChangeHandler changeHandler;

        public int elementIndex;

		// ------------------------------------------------------------------------
		public void onValueChange(int value) {

			if (changeHandler != null) {
                changeHandler(elementIndex, value);
			}
		}
	}
}