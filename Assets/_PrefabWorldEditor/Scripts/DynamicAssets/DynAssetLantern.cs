//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections.Generic;


using UnityEngine;

namespace PrefabWorldEditor
{
    public class DynAssetLantern : DynamicAsset
    {
        public Light myLight;

        private bool _isActive;

        private float _intensity;
        private float _range;

        private float _colorR;
        private float _colorG;
        private float _colorB;

        // ------------------------------------------------------------------------
        private void Awake()
        {
            _isActive = true;

            _intensity = 1.0f;
            _range = 10.0f;

            _colorR = 1.0f;
            _colorG = 1.0f;
            _colorB = 1.0f;
        }

        // ------------------------------------------------------------------------
        void Start()
        {
            // 0
            createToggle("Is Active", _isActive);

            // 1
            createSlider("Intensity", 0.5f, 2.0f, _intensity);
            
            // 2
            createSlider("Range", 1.0f, 10.0f, _range);

            // 3
            createSlider ("Color Red", 0.0f, 1.0f, _colorR);

            // 4
            createSlider ("Color Green", 0.0f, 1.0f, _colorG);

            // 3
            createSlider ("Color Blue", 0.0f, 1.0f, _colorB);
        }

        // ------------------------------------------------------------------------
        public override void updateSliderValue(int elementIndex, float value)
        {
            if (elementIndex == 1) {
                _intensity = value;
            }
            else if (elementIndex == 2) {
                _range = value;
            }
            else if (elementIndex == 3) {
                _colorR = value;
            }
            else if (elementIndex == 4) {
                _colorG = value;
            }
            else if (elementIndex == 5) {
                _colorB = value;
            }

            setLight ();
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

            myLight.gameObject.SetActive (_isActive);

            myLight.intensity = _intensity;
            myLight.range = _range;

            myLight.color = new Color (_colorR, _colorG, _colorB);
        }
    }
}