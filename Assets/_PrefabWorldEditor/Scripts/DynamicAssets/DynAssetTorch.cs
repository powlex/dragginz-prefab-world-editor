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
        public GameObject goParticles;
        public ParticleSystem particleSystemLight;
        public Light myLight;
        public Light myLight2;

        private bool _isActive = true;

        private float _intensity = 1f;
        private float _range = 10f;

        // light flicker effect
        private int smoothing = 20;
        private Queue<float> _smoothQueue;
        private float _lastSum;
        private float _newVal;

        // ------------------------------------------------------------------------
        void Start ()
        {
            // 0
            createToggle("Is Active", _isActive);

            // 1
            createSlider ("Intensity", 0.5f, 1.5f, _intensity);

            // 2
            createSlider ("Range", 1.0f, 10.0f, _range);

            // light flicker effect
            _smoothQueue = new Queue<float> (smoothing);
            _lastSum = 0;
        }

        // ------------------------------------------------------------------------
        void Update ()
        {
            while (_smoothQueue.Count >= smoothing) {
                _lastSum -= _smoothQueue.Dequeue ();
            }

            // Generate random new item, calculate new average
            _newVal = UnityEngine.Random.Range(_intensity-0.2f, _intensity+0.2f);
            _smoothQueue.Enqueue (_newVal);
            _lastSum += _newVal;

            myLight.intensity = myLight2.intensity = _lastSum / (float)_smoothQueue.Count;
        }

        // ------------------------------------------------------------------------
        private void resetFlickerEffect ()
        {
            _smoothQueue.Clear ();
            _lastSum = 0;
        }

        // ------------------------------------------------------------------------
        public override void updateSliderValue (int elementIndex, float value)
        {
            if (elementIndex == 1) {
                _intensity = value;
            }
            else if (elementIndex == 2) {
                _range = value;
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
            if (len > 2) {
                _range = float.Parse (data[2]);
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

            if (!_isActive) {
                resetFlickerEffect ();
            }

            /*if (particleSystemLight != null) {
                ParticleSystem.LightsModule l = particleSystemLight.lights;
                l.intensityMultiplier = _intensity;
            }*/

            myLight.intensity = _intensity;
            myLight.range = _range;

            myLight2.intensity = _intensity;
            myLight2.range = _range;
        }
    }
}