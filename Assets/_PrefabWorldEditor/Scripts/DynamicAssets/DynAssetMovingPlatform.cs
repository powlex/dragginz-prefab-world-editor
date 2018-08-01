//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;

using Gamekit3D.GameCommands;

namespace PrefabWorldEditor
{
    public class DynAssetMovingPlatform : DynamicAsset
    {
        public SimpleTranslator translatorScript;

        private bool _isActive = true;

        private float _height = 2f;
        private float _duration = 5f;

        // ------------------------------------------------------------------------
        void Start ()
        {
            // 0
            createToggle("Is Active", _isActive);

            // 1
            createSlider ("Height", -0.25f, 10.0f, _height);

            // 2
            createSlider ("Duration", 1.0f, 10.0f, _duration);
        }

        // ------------------------------------------------------------------------
        //void Update ()
        //{
        //}

        // ------------------------------------------------------------------------
        public override void updateSliderValue (int elementIndex, float value)
        {
            if (elementIndex == 1) {
                _height = value;
            }
            else if (elementIndex == 2) {
                _duration = value;
            }

            setPlatform ();
        }
        
        // ------------------------------------------------------------------------
        public override void updateToggleValue(int elementIndex, bool value)
        {
            if (elementIndex == 0) {
                _isActive = value;
            }

            setPlatform ();
        }

        // ------------------------------------------------------------------------
        public override string dataToString ()
        {
            string s = "";

            s += (_isActive ? "1" : "0");
            s += _delimiter + _height.ToString ();
            s += _delimiter + _duration.ToString ();

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
                _height = float.Parse (data[1]);
            }
            if (len > 2) {
                _duration = float.Parse (data[2]);
            }

            setPlatform ();
        }

        // ------------------------------------------------------------------------
        // Private Methods
        // ------------------------------------------------------------------------
        private void setPlatform ()
        {
            if (translatorScript != null) {
                translatorScript.activate = _isActive;
                translatorScript.end = new Vector3 (translatorScript.end.x, _height, translatorScript.end.z);
                translatorScript.duration = _duration;
            }
        }
    }
}