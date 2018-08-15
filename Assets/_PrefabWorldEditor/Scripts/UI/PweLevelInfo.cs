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

        public void setLevelName(string s) {
            if (levelName != null) {
                levelName.text = s;
            }
        }

        public void setLevelSize (string s) {
            if (levelSize != null) {
                levelSize.text = "Size: " + s;
            }
        }

        public void setLevelPos (string s) {
            if (levelPos != null) {
                levelPos.text = "Pos: " + s;
            }
        }

        public void setLevelUpdated (string s) {
            if (levelUpdated != null) {
                levelUpdated.text = "Updated: " + s;
            }
        }

        //
        // Events
        //

        public void onInfoIconClick () {
            if (infoPanel != null) {
                showInfoPanel (!infoPanel.gameObject.activeSelf);
            }
            PweSettings.Instance.showSettingsPanels (false);
        }

        #endregion

        #region PrivateMethods

        #endregion
    }
}