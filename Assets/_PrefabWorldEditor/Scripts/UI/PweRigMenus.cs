//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class PweRigMenus : MonoSingleton<PweRigMenus>
    {
		public VRController _vrController;

		public Transform panelAssetTypes;
		public Transform panelAssets;

		#region Getters

		public bool panelAssetTypesVisible {
			get { return panelAssetTypes.gameObject.activeSelf; }
		}

		public bool panelAssetsVisible {
			get { return panelAssets.gameObject.activeSelf; }
		}

		#endregion

		//

		#region PublicMethods

		// ------------------------------------------------------------------------
		public void init()
		{
			showAssetPanels (false, false);
		}

		// ------------------------------------------------------------------------
		public void showAssetPanels(bool type, bool asset)
		{
			panelAssetTypes.gameObject.SetActive (type);
			panelAssets.gameObject.SetActive (asset);
		}

		#endregion

		//

		#region EventHandlers

		// ------------------------------------------------------------------------
		public void onAssetTypeSelect(int index) {

			showAssetPanels (true, true);
		}

		// ------------------------------------------------------------------------
		public void onAssetSelect(int index) {

			LevelController.Instance.placeDungeonPrefab (index);
			showAssetPanels (false, false);
		}

		#endregion
    }
}