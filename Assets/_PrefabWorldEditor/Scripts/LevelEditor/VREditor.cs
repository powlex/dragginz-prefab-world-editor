//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.XR;

using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class VREditor : MonoSingleton<VREditor>
    {
		#region PublicProperties

		public Transform assetSelection;

		#endregion

		#region PrivateProperties

		private PrefabLevelEditor.Part _curEditPart;
		private GameObject _goEditPart;

		#endregion

		#region Getters

		//public GameObject container {
		//	get { return _container.gameObject; }
		//}

		#endregion

		//

		#region SystemMethods

        void Start()
        {
			_curEditPart = new PrefabLevelEditor.Part ();
			_goEditPart  = null;

			if (XRSettings.enabled)
			{
				//
			}
        }

		// ------------------------------------------------------------------------
		/*void LateUpdate()
		{
			if (!XRSettings.enabled) {
				return;
			}

			if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Menu))
			{
				Debug.Log("ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Menu)");
			}

			if (ViveInput.GetPressUpEx (HandRole.LeftHand, ControllerButton.Menu)) {
				Debug.Log("ViveInput.GetPressUpEx(HandRole.LeftHand, ControllerButton.Menu)");
			}
		}*/

		#endregion

		//

		#region PublicMethods

		public void setAssetType()
		{
		}

		public void setAssetId()
		{
		}

		#endregion

		//

		#region PrivateMethods

		// ------------------------------------------------------------------------
		private void toggleEditPart(float mousewheel)
		{
			/*
			int index = _assetTypeIndex [_assetType];
			int max = _assetTypeList [_assetType].Count;

			if (mousewheel > 0) {
				if (++index >= max) {
					index = 0;
				}
			} else {
				if (--index < 0) {
					index = max - 1;
				}
			}
			_assetTypeIndex [_assetType] = index;

			setNewEditPart(_assetTypeList[_assetType][index]);
			*/
		}

		// ------------------------------------------------------------------------
		private void setNewEditPart(PrefabLevelEditor.Part part)
		{
			/*
			_curEditPart = part;

			if (_goEditPart != null) {
				Destroy (_goEditPart);
			}
			_goEditPart = null;

			_goEditPart = createPartAt(_curEditPart.id, -10, -10, -10);

			LevelController.Instance.setMeshCollider(_goEditPart, false);
			*/
		}

		// ------------------------------------------------------------------------
		public GameObject createPartAt(Globals.PartList partId, float x, float y, float z)
		{
            GameObject go = null;

			if (!PrefabLevelEditor.Instance.parts.ContainsKey(partId)) {
                return go;
            }

			go = Instantiate(PrefabLevelEditor.Instance.parts[partId].prefab);
            if (go != null) {
				go.name = "vr_asset_selection";
				go.transform.SetParent(transform);
				go.transform.position = Vector3.zero;
				go.transform.localScale = new Vector3 (.25f, .25f, .25f);
            }

            return go;
        }

		#endregion
	}
}