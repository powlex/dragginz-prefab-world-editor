//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;
using UnityEngine.UI;

namespace PrefabWorldEditor
{
    public class UIInstanceInfo : MonoBehaviour
    {
        public Text assetName;

        public Toggle rotateX;
        public Toggle rotateY;
        public Toggle rotateZ;

        public Toggle toggleStatic;
        public Toggle toggleGravity;

        public Slider sliderSnowLevel;

        public UIDynamicInstanceSettings dynamicSettings;

        private bool initUIComponents;

        // ---------------------------------------------------------------------------------------------
        public void init(PrefabLevelEditor.Part part, LevelController.LevelElement element)
        {
            initUIComponents = true;

            if (name != null) {
                assetName.text = part.name;
            }

            if (rotateX != null) {
                rotateX.isOn = (part.canRotate.x == 1);
                rotateX.interactable = false;
            }
            if (rotateY != null) {
                rotateY.isOn = (part.canRotate.y == 1);
                rotateY.interactable = false;
            }
            if (rotateZ != null) {
                rotateZ.isOn = (part.canRotate.z == 1);
                rotateZ.interactable = false;
            }

            if (toggleStatic != null) {
                toggleStatic.isOn = (element.overwriteStatic == 0 ? part.isStatic : (element.overwriteStatic == 1 ? true : false));
            }

            if (toggleGravity != null) {
                if (element.go != null) {
                    toggleGravity.isOn = (element.overwriteGravity == 0 ? part.usesGravity : (element.overwriteGravity == 1 ? true : false));
                }
                else {
                    toggleGravity.isOn = part.usesGravity;
                }
                toggleGravity.interactable = (part.type != Globals.AssetType.Floor && part.type != Globals.AssetType.Wall && part.type != Globals.AssetType.Dungeon);
            }
            toggleGravity.interactable = !toggleStatic.isOn;

            if (sliderSnowLevel != null) {
                sliderSnowLevel.value = element.shaderSnow;
                sliderSnowLevel.transform.parent.gameObject.SetActive(LevelController.Instance.hasSnowShader);
            }

            // dynamic asset settings
            DynamicAsset dynAssetScript = element.go.GetComponent<DynamicAsset>();
            if (dynAssetScript != null) {
                showDynamicSettings(true);
                dynamicSettings.init(dynAssetScript.setupList);
            } else {
                showDynamicSettings (false);
            }

            initUIComponents = false;
        }

        // ---------------------------------------------------------------------------------------------
        public void showDynamicSettings(bool state) {
            dynamicSettings.gameObject.SetActive(state);
        }

        // ---------------------------------------------------------------------------------------------
        public void onGravityValueChange(bool value) {

            Debug.Log ("onGravityValueChange");
            if (LevelController.Instance.selectedElement.go != null) {

                LevelController.LevelElement e = LevelController.Instance.selectedElement;
                e.overwriteGravity = (toggleGravity.isOn ? 1 : 2);
                LevelController.Instance.selectedElement = e;

                LevelController.Instance.saveSelectElement();
            }
        }

        // ---------------------------------------------------------------------------------------------
        public void onStaticValueChange (bool value) {

            if (initUIComponents) {
                return;
            }

            Debug.Log ("onStaticValueChange");
            if (LevelController.Instance.selectedElement.go != null) {

                LevelController.LevelElement e = LevelController.Instance.selectedElement;
                e.overwriteStatic = (toggleStatic.isOn ? 1 : 2);
                LevelController.Instance.selectedElement = e;

                LevelController.Instance.saveSelectElement ();

                if (e.overwriteStatic == 1) {
                    toggleGravity.isOn = false;
                }
                toggleGravity.interactable = !toggleStatic.isOn;
            }
        }

        // -------------------------------------------------------------------------------------
        public void onSliderSnowLevelChange(Single value) {

            if (initUIComponents) {
                return;
            }

            LevelController.Instance.changeSnowLevel((float)value);

            LevelController.LevelElement e = LevelController.Instance.selectedElement;
            e.shaderSnow = (float)value;
            LevelController.Instance.selectedElement = e;

            LevelController.Instance.saveSelectElement();
        }
    }
}