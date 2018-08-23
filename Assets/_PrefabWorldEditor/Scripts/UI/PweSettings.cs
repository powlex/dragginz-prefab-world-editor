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
	public class PweSettings : MonoSingleton<PweSettings>
    {
		public Transform settingsPanel;

        public Toggle toggleAmbientLight;
        public Toggle toggleSpotLight;
        public Toggle toggleSnapToGrid;

        #region SystemMethods

        void Awake () {

			showSettingsPanels (false);
        }

		#endregion

		#region PublicMethods

		//
		public void showSettingsPanels(bool state) {

            if (settingsPanel != null) {
                settingsPanel.gameObject.SetActive(state);
            }
		}

        public void setAmbientLightToggle (bool state) {
            if (toggleAmbientLight != null) {
                toggleAmbientLight.isOn = state;
            }
        }

        //
        // Events
        //

        public void onSettingsIconClick() {
            if (settingsPanel != null) {
                showSettingsPanels (!settingsPanel.gameObject.activeSelf);
            }
            PweLevelInfo.Instance.showInfoPanel (false);
        }

        //
        public void onToggleAmbientLight (bool state) {
            PrefabLevelEditor.Instance.goLights.SetActive (state);
        }

        //
        public void onToggleSpotLight (bool state) {
            PrefabLevelEditor.Instance.setSpotLights (state);
        }

        //
        public void onToggleSnapToGrid (bool state) {
            PrefabLevelEditor.Instance.setSnapToGrid (state);
        }

        //
        public void onToggleExtraEditorLights (bool state) {
            if (PrefabLevelEditor.Instance.goLightsEditor != null) {
                PrefabLevelEditor.Instance.goLightsEditor.SetActive (state);
            }
        }

        #endregion

        #region PrivateMethods

        #endregion
    }
}