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

        public Transform viveRig;
        //public Transform assetSelection;

		#endregion

		#region PrivateProperties

		private PrefabLevelEditor.Part _curEditPart;
		private GameObject _goEditPart;

        private bool _hideEditPart = false;

		#endregion

		#region Getters

		//public GameObject container {
		//	get { return _container.gameObject; }
		//}

		#endregion

		//

		#region SystemMethods

		// ------------------------------------------------------------------------
		void Update()
		{
            if (_goEditPart != null && _goEditPart.activeSelf) {

                _goEditPart.transform.Rotate(Vector3.up * (Time.deltaTime * 2f));
            }

            if (_hideEditPart) {
                _hideEditPart = false;
                hideEditPart();
            }

            /*if (ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Menu))
			{
				Debug.Log("ViveInput.GetPressUpEx(HandRole.RightHand, ControllerButton.Menu)");
			}

			if (ViveInput.GetPressUpEx (HandRole.LeftHand, ControllerButton.Menu)) {
				Debug.Log("ViveInput.GetPressUpEx(HandRole.LeftHand, ControllerButton.Menu)");
			}*/
        }

        #endregion

        //

        #region PublicMethods

        // ------------------------------------------------------------------------
        public void init()
        {
            _curEditPart = new PrefabLevelEditor.Part();
            _goEditPart = null;
        }

        // ------------------------------------------------------------------------
        public void hideEditPart() {

            if (_goEditPart != null) {
                _goEditPart.SetActive(false);
            }
        }

        // ------------------------------------------------------------------------
        public void setAssetType(int typeIndex, int assetIndex)
		{
            Globals.AssetType type = (Globals.AssetType)typeIndex;
            createAsset(PrefabLevelEditor.Instance.assetTypeList[type][assetIndex]);
        }

        public void setAsset(int typeIndex, int assetIndex)
        {
            Globals.AssetType type = (Globals.AssetType)typeIndex;
            createAsset(PrefabLevelEditor.Instance.assetTypeList[type][assetIndex]);
        }

        #endregion

        //

        #region PrivateMethods

        // ------------------------------------------------------------------------
        public void createAsset(PrefabLevelEditor.Part part)
		{
            _curEditPart = part;

            if (_goEditPart != null) {
                Destroy(_goEditPart);
            }
            _goEditPart = null;

            if (!PrefabLevelEditor.Instance.parts.ContainsKey(part.id)) {
                return;
            }

            _goEditPart = Instantiate(PrefabLevelEditor.Instance.parts[part.id].prefab);
            if (_goEditPart != null)
            {
                _goEditPart.name = "vr_asset_selection";
                _goEditPart.transform.SetParent(transform);
                _goEditPart.transform.position = PweDynamicMenusVR.Instance.curMenuPos;

                // get max. scale
                float maxSize = 2.5f;
                float biggestAssetSize = Mathf.Max(part.w, part.h);
                biggestAssetSize = Mathf.Max(biggestAssetSize, part.d);
                float scale = maxSize / biggestAssetSize;
                scale = Mathf.Min(1.0f, scale);

                _goEditPart.transform.localScale = new Vector3 (scale, scale, scale);
                _goEditPart.transform.rotation = Quaternion.Euler(new Vector3(-24f, -24f, 24f));

                //LevelController.Instance.setMeshCollider(_goEditPart, false);
                LevelController.Instance.setRigidBody(_goEditPart, false);

                Draggable draggable = _goEditPart.AddComponent<Draggable>();
                draggable.afterGrabberGrabbed += onAssetGrabbed;
                draggable.beforeGrabberReleased += onAssetReleased;
            }
        }

        public void onAssetGrabbed() {

            PweDynamicMenusVR.Instance.showMenuPanels(false, false);
        }

        public void onAssetReleased() {
                        
            if (_curEditPart.type == Globals.AssetType.Floor) {
                PrefabLevelEditor.Instance.selectAssetType(_curEditPart.type);
                PrefabLevelEditor.Instance.setNewEditPart(_curEditPart);
                PrefabLevelEditor.Instance.fillY(Vector3.zero);
            }

            _hideEditPart = true;
        }

        #endregion
    }
}