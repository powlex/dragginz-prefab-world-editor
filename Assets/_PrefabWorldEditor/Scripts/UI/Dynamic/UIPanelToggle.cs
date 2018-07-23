//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;

namespace PrefabWorldEditor
{
	public class UIPanelToggle : MonoBehaviour
    {
		public Text label;
		public Toggle toggle;

		public delegate void ChangeHandler(int elementIndex, bool value);
		public event ChangeHandler changeHandler;

        public int elementIndex;

		// ------------------------------------------------------------------------
		public void onValueChange(bool value) {

			if (changeHandler != null) {
                changeHandler(elementIndex, value);
			}
		}
	}
}