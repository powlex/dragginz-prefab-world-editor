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
	public class PweWorldSpaceMenus : MonoSingleton<PweWorldSpaceMenus>
    {
		public VRController _vrController;

		public Transform panelHelp;

		#region SystemMethods

        void Awake() {

			//
        }

		#endregion

		// ------------------------------------------------------------------------

		#region PublicMethods

		public void init()
		{
			_vrController.SetRightLaserPointerActive (true);
			_vrController.UpdateActivity ();
		}

		// ------------------------------------------------------------------------
		public void onHelpPanelOkay() {
			panelHelp.gameObject.SetActive (false);
			_vrController.SetRightLaserPointerActive (false);
			_vrController.UpdateActivity ();
		}

		// ------------------------------------------------------------------------
		public void onButtonModePlayClicked() {
			PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Play);
		}
		public void onButtonModeBuildClicked() {
			PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Place);
		}
		public void onButtonModeSelectClicked() {
			PrefabLevelEditor.Instance.setEditMode(PrefabLevelEditor.EditMode.Transform);
		}
		public void onButtonModeClearClicked() {
			PrefabLevelEditor.Instance.clearLevel();
		}

		#endregion
    }
}