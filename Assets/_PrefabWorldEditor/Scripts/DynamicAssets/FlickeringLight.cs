//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections.Generic;
//using System.Reflection;

using UnityEngine;

namespace PrefabWorldEditor
{
    public class FlickeringLight : DynamicAsset
    {
        public Light myLight;

        public enum WaveForm {none, sin, tri, sqr, saw, inv, noise};
        public WaveForm waveform = WaveForm.sin;

        [Range(0.0f, 1.0f)]
        public float baseStart = 0.0f; // start 

        [Range(0.1f, 5f)]
        public float amplitude = 1.0f; // amplitude of the wave

        [Range(0.0f, 5f)]
        public float phase = 0.0f; // start point inside on wave cycle

        [Range(0.1f, 5f)]
        public float frequency = 0.5f; // cycle frequency per second

        //

        private bool _isActive;
        private Color _originalColor;

        float _x;
        float _y;
        
        // ------------------------------------------------------------------------
        private void Awake()
        {
            _isActive = true;
            _originalColor = myLight.color;

        }

        // ------------------------------------------------------------------------
        void Start()
        {
            // 0
            createToggle("Is Active", _isActive);

            // 1
            List<string> options = new List<string>();
            foreach (string wf in Enum.GetNames(typeof(WaveForm))) {
                options.Add(wf);
            }
            createDropdown("Wave Form", options);

            // 2
            createSlider("Frequency", 0.1f, 0.5f, 0.1f);
            
            // 3
            createSlider("Amplitude", 0.1f, 10.0f, 1.0f);
        }

        // ------------------------------------------------------------------------
        public override void updateSliderValue(int elementIndex, float value)
        {
            if (elementIndex == 2) {
                frequency = value;
            }
            else if (elementIndex == 3) {
                amplitude = value;
            }
        }

        // ------------------------------------------------------------------------
        public override void updateToggleValue(int elementIndex, bool value)
        {
            if (elementIndex == 0) {
                _isActive = value;
                myLight.gameObject.SetActive(_isActive);
            }
        }

        // ------------------------------------------------------------------------
        public override void updateDropdownValue(int elementIndex, int value)
        {
            if (elementIndex == 1) {
                waveform = (WaveForm)value;
            }
        }

        // ------------------------------------------------------------------------
        void Update()
        {
            if (_isActive && waveform != WaveForm.none) {
                myLight.color = _originalColor * (EvalWave());
            }
        }

        // ------------------------------------------------------------------------
        private float EvalWave()
        {
            _x = (Time.time + phase) * frequency;
            _x = _x - Mathf.Floor(_x); // normalized value (0..1)

            if (waveform == WaveForm.sin) {
                _y = Mathf.Sin(_x * 2 * Mathf.PI);
            }
            else if (waveform == WaveForm.tri) {
                if (_x < 0.5f)
                    _y = 4.0f * _x - 1.0f;
                else
                    _y = -4.0f * _x + 3.0f;
            }
            else if (waveform == WaveForm.sqr) {
                if (_x < 0.5f)
                    _y = 1.0f;
                else
                    _y = -1.0f;
            }
            else if (waveform == WaveForm.saw) {
                _y = _x;
            }
            else if (waveform == WaveForm.inv) {
                _y = 1.0f - _x;
            }
            else if (waveform == WaveForm.noise) {
                _y = 1f - (UnityEngine.Random.value * 2);
            }
            else {
                _y = 1.0f;
            }

            return (_y * amplitude) + baseStart;
        }

        // ------------------------------------------------------------------------
        /*private void showFields() {

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance; //BindingFlags.NonPublic |  | BindingFlags.Instance | BindingFlags.Static

            FieldInfo[] fields = this.GetType().GetFields(flags);
            foreach (FieldInfo fieldInfo in fields) {
                Debug.Log("FieldInfo Obj: " + this.name + ", Field: " + fieldInfo.Name + ", type: " + fieldInfo.GetType() + ", value: " + fieldInfo.GetValue(fieldInfo));
            }

        }*/
    }
}