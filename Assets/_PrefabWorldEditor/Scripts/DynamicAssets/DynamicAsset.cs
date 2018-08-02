﻿//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections.Generic;

using UnityEngine;

namespace PrefabWorldEditor
{
    public class DynamicAsset : MonoBehaviour
    {
        public List<Globals.UIElementSetup> setupList = new List<Globals.UIElementSetup>();

        protected string _delimiter = ";";

        #region ProtectedMethods

        // ------------------------------------------------------------------------
        protected void createSlider(string label, float min, float max, float value)
        {
            Globals.UIElementSetup e = new Globals.UIElementSetup() {
                type = Globals.UIElementType.Slider,
                label = label,
                rangeMin = min,
                rangeMax = max,
                defaultValue = value
            };

            setupList.Add(e);
        }

        // ------------------------------------------------------------------------
        protected void createToggle(string label, bool isOn)
        {
            Globals.UIElementSetup e = new Globals.UIElementSetup() {
                type = Globals.UIElementType.Toggle,
                label = label,
                isOn = isOn
            };

            setupList.Add(e);
        }

        // ------------------------------------------------------------------------
        protected void createDropdown(string label, List<string> options)
        {
            Globals.UIElementSetup e = new Globals.UIElementSetup() {
                type = Globals.UIElementType.Dropdown,
                label = label,
                dropdownOptions = options
            };

            setupList.Add(e);
        }

        #endregion

        //

        #region PublicOverrideMethods

        // ------------------------------------------------------------------------
        //
        // ------------------------------------------------------------------------
        public virtual void updateSliderValue(int elementIndex, float value)
        {
            // OVERRIDE ME
        }

        // ------------------------------------------------------------------------
        public virtual void updateToggleValue(int elementIndex, bool value)
        {
            // OVERRIDE ME
        }

        // ------------------------------------------------------------------------
        public virtual void updateDropdownValue(int elementIndex, int value)
        {
            // OVERRIDE ME
        }

        // ------------------------------------------------------------------------
        //
        // ------------------------------------------------------------------------
        public virtual float getCurSliderValue (int elementIndex)
        {
            // OVERRIDE ME
            return 0;
        }

        // ------------------------------------------------------------------------
        public virtual bool getCurToggleValue (int elementIndex)
        {
            // OVERRIDE ME
            return false;
        }

        // ------------------------------------------------------------------------
        public virtual int getCurDropdownValue (int elementIndex)
        {
            // OVERRIDE ME
            return 0;
        }

        // ------------------------------------------------------------------------
        //
        // ------------------------------------------------------------------------
        public virtual string dataToString ()
        {
            // OVERRIDE ME
            return "";
        }

        // ------------------------------------------------------------------------
        public virtual void stringToData (string s)
        {
            // OVERRIDE ME
        }

        #endregion
    }
}