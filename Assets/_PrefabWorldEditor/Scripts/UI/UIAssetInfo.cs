//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using AssetsShared;

namespace PrefabWorldEditor
{
    public class UIAssetInfo : MonoBehaviour
    {
        public Text assetName;

        public Toggle rotateX;
        public Toggle rotateY;
        public Toggle rotateZ;

        public Toggle toggleStatic;
        public Toggle toggleGravity;

        // ---------------------------------------------------------------------------------------------
        public void init(PrefabLevelEditor.Part part, LevelController.LevelElement element) {

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
                toggleStatic.isOn = part.isStatic;
                toggleStatic.interactable = false;
            }

            if (toggleGravity != null) {
                toggleGravity.isOn = part.usesGravity;
                toggleGravity.interactable = false;
            }
        }
    }
}