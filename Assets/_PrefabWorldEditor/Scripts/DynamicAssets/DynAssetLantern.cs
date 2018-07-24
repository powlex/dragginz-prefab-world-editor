//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;

namespace PrefabWorldEditor
{
    public class DynAssetLantern : DynamicAsset
    {
        public Light myLight;

        private bool _isActive = true;

        private float _intensity = 1f;
        private float _range = 10f;

        private float _colorR = 1f;
        private float _colorG = 1f;
        private float _colorB = 1f;

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
        public override string dataToString ()
        {
            string s = "";

            s += (_isActive ? "1" : "0");
            s += _delimiter + _intensity.ToString ();
            s += _delimiter + _range.ToString ();
            s += _delimiter + _colorR.ToString ();
            s += _delimiter + _colorG.ToString ();
            s += _delimiter + _colorB.ToString ();

            return s;
        }

        // ------------------------------------------------------------------------
        public override void stringToData (string s)
        {
            string[] data = s.Split(Convert.ToChar(_delimiter));

            int len = data.Length;
            if (len > 0) {
                _isActive = (data[0] == "1" ? true : false);
            }
            if (len > 1) {
                _intensity = float.Parse(data[1]);
            }
            if (len > 2) {
                _range = float.Parse (data[2]);
            }
            if (len > 3) {
                _colorR = float.Parse (data[3]);
            }
            if (len > 4) {
                _colorG = float.Parse (data[4]);
            }
            if (len > 5) {
                _colorB = float.Parse (data[5]);
            }

            setLight ();
        }

        // ------------------------------------------------------------------------
        // Private Methods
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