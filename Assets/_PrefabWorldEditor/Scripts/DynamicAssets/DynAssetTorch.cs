//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections.Generic;


using UnityEngine;

namespace PrefabWorldEditor
{
    public class DynAssetTorch : DynamicAsset
    {
        public GameObject myLight;

        private bool _isActive;

        // ------------------------------------------------------------------------
        private void Awake()
        {
            _isActive = true;
        }

        // ------------------------------------------------------------------------
        void Start()
        {
            // 0
            createToggle("Is Active", _isActive);
        }

        // ------------------------------------------------------------------------
        public override void updateToggleValue(int elementIndex, bool value)
        {
            if (elementIndex == 0) {
                _isActive = value;
            }

            setLight ();
        }

        // ------------------------------------------------------------------------
        private void setLight()
        {
            if (myLight == null) {
                return;
            }

            myLight.SetActive (_isActive);
        }
    }
}