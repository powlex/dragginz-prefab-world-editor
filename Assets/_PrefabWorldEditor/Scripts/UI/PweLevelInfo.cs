//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class PweLevelInfo : MonoSingleton<PweLevelInfo>
    {
		public Transform infoPanel;

        public Text levelName;
        public Text levelSize;
        public Text levelPos;
        public Text levelUpdated;

        #region SystemMethods

        void Awake () {

			showInfoPanel (false);
        }

		#endregion

		#region PublicMethods

		public void showInfoPanel(bool state) {

            if (infoPanel != null) {
                infoPanel.gameObject.SetActive(state);
            }
		}

        #endregion

        #region PrivateMethods

        #endregion
    }
}