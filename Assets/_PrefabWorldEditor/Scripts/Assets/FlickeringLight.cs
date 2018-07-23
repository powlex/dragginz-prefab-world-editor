//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Reflection;

using UnityEngine;

namespace PrefabWorldEditor
{
    public class FlickeringLight : DynamicAsset
    {
        public Light myLight;

        public enum WaveForm { sin, tri, sqr, saw, inv, noise };
        public WaveForm waveform = WaveForm.sin;

        public float baseStart = 0.0f; // start 

        [Range(0.1f, 5f)]
        public float amplitude = 1.0f; // amplitude of the wave

        [Range(0.0f, 5f)]
        public float phase = 0.0f; // start point inside on wave cycle

        [Range(0.1f, 5f)]
        public float frequency = 0.5f; // cycle frequency per second

        private Color originalColor;

        // ------------------------------------------------------------------------
        void Start()
        {
            Globals.UIElementSetup esu = new Globals.UIElementSetup();
            esu.type = Globals.UIElementType.Dropdown;
            esu.label = "Wave Form";
            esu.dropdownOptions = new System.Collections.Generic.List<string>();
            foreach (string wf in Enum.GetNames(typeof(WaveForm))) {
                esu.dropdownOptions.Add(wf);
            }
            setupList.Add(esu);

            esu = new Globals.UIElementSetup();
            esu.type = Globals.UIElementType.Slider;
            esu.label = "Frequency";
            esu.rangeMin = 0.1f;
            esu.rangeMax = 0.5f;
            esu.defaultValue = 0.1f;
            setupList.Add(esu);

            esu = new Globals.UIElementSetup();
            esu.type = Globals.UIElementType.Toggle;
            esu.label = "Blah Toggle";
            esu.isOn = false;
            setupList.Add(esu);

            esu = new Globals.UIElementSetup();
            esu.type = Globals.UIElementType.Slider;
            esu.label = "Amplitude";
            esu.rangeMin = 0.1f;
            esu.rangeMax = 10.0f;
            esu.defaultValue = 1.0f;
            setupList.Add(esu);

            originalColor = myLight.color;
        }

        // ------------------------------------------------------------------------
        void Update()
        {
            myLight.color = originalColor * (EvalWave());
        }

        // ------------------------------------------------------------------------
        private float EvalWave()
        {
            float x = (Time.time + phase) * frequency;
            float y;
            x = x - Mathf.Floor(x); // normalized value (0..1)

            if (waveform == WaveForm.sin) {

                y = Mathf.Sin(x * 2 * Mathf.PI);
            }
            else if (waveform == WaveForm.tri) {

                if (x < 0.5f)
                    y = 4.0f * x - 1.0f;
                else
                    y = -4.0f * x + 3.0f;
            }
            else if (waveform == WaveForm.sqr) {

                if (x < 0.5f)
                    y = 1.0f;
                else
                    y = -1.0f;
            }
            else if (waveform == WaveForm.saw) {

                y = x;
            }
            else if (waveform == WaveForm.inv) {

                y = 1.0f - x;
            }
            else if (waveform == WaveForm.noise) {

                y = 1f - (UnityEngine.Random.value * 2);
            }
            else {
                y = 1.0f;
            }
            return (y * amplitude) + baseStart;
        }

        // ------------------------------------------------------------------------
        private void showFields() {

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            /*BindingFlags.NonPublic |  | BindingFlags.Instance | BindingFlags.Static*/

            FieldInfo[] fields = this.GetType().GetFields(flags);
            foreach (FieldInfo fieldInfo in fields) {
                Debug.Log("FieldInfo Obj: " + this.name + ", Field: " + fieldInfo.Name + ", type: " + fieldInfo.GetType() + ", value: " + fieldInfo.GetValue(fieldInfo));
            }

            /*
            PropertyInfo[] properties = this.GetType().GetProperties(flags);
            foreach (PropertyInfo propertyInfo in properties) {
                Debug.Log("PropertyInfo Obj: " + this.name + ", Property: " + propertyInfo.Name);
            }
            */
        }
    }
}