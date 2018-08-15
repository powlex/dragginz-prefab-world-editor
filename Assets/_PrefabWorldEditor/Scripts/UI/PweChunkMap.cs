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
	public class PweChunkMap : MonoSingleton<PweChunkMap>
    {
        public Transform mapContainer;
        public Transform mapPanel;

        #region SystemMethods

        void Awake () {
            showmapContainer (false);
        }

		#endregion

		#region PublicMethods

		//
		public void showmapContainer(bool state) {

            if (mapContainer != null) {
                mapContainer.gameObject.SetActive(state);
            }
            if (mapPanel != null) {
                mapPanel.gameObject.SetActive (state);
            }
		}

        //
        // Events
        //

        public void onMapIconClick()
		{
            PrefabLevelEditor.Instance.showWorldMap (true);

            PweLevelInfo.Instance.showInfoPanel (false);
            PweSettings.Instance.showSettingsPanels (false);
        }

        public void onCloseClick () {

            PrefabLevelEditor.Instance.showWorldMap (false);
        }

        #endregion

        #region PrivateMethods

        #endregion
    }
}