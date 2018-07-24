//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;

using UnityEngine;

namespace PrefabWorldEditor
{
    public class DynAssetTorch : DynamicAsset
    {
        public GameObject goParticles;
        public ParticleSystem particleSystemLight;

        private bool _isActive = true;

        private float _intensity = 1f;

        // ------------------------------------------------------------------------
        void Start()
        {
            // 0
            createToggle("Is Active", _isActive);

            // 1
            createSlider ("Intensity", 0.5f, 5.0f, _intensity);
        }

        // ------------------------------------------------------------------------
        public override void updateSliderValue (int elementIndex, float value)
        {
            if (elementIndex == 1) {
                _intensity = value;
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
                _intensity = float.Parse (data[1]);
            }

            setLight ();
        }

        // ------------------------------------------------------------------------
        // Private Methods
        // ------------------------------------------------------------------------
        private void setLight()
        {
            if (goParticles != null) {
                goParticles.SetActive (_isActive);
            }

            if (particleSystemLight != null) {
                ParticleSystem.LightsModule l = particleSystemLight.lights;
                l.intensityMultiplier = _intensity;
            }
        }
    }
}