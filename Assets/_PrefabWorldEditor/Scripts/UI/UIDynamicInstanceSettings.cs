//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace PrefabWorldEditor
{
    public class UIDynamicInstanceSettings : MonoBehaviour
    {
        #region PublicFields

        public RectTransform rectTransform;

        public Text header;
        public Transform container;

        #endregion

        //

        #region PrivateFields

        private struct UIElement
        {
            public GameObject go;
            public Globals.UIElementType type;
            public MonoBehaviour script;
        };

        private List<Globals.UIElementSetup> _setupList;

        private List<UIElement> _uiElements;

        private float _yPos;

        #endregion

        //

        #region SystemMethods

        // ---------------------------------------------------------------------------------------------
        void Awake() {

            _uiElements = new List<UIElement>();
        }

        #endregion

        //

        #region PublicMethods

        // ---------------------------------------------------------------------------------------------
        public void init(List<Globals.UIElementSetup> setupList)
        {
            _setupList = setupList;

            _yPos = 10;

            removeElements();

            int i, len = setupList.Count;
            for (i = 0; i < len; ++i)
            {
                Globals.UIElementSetup esu = setupList[i];
                if (esu.type == Globals.UIElementType.Slider)
                {
                    _uiElements.Add( createSlider(i, esu) );
                }
                else if (esu.type == Globals.UIElementType.Toggle)
                {
                    _uiElements.Add( createToggle(i, esu) );
                }
                else if (esu.type == Globals.UIElementType.Dropdown)
                {
                    _uiElements.Add(createDropdown(i, esu));
                }
            }

            rectTransform.sizeDelta = new Vector2(180, _yPos+40);
        }

        #endregion

        //

        #region PrivateMethods

        // ---------------------------------------------------------------------------------------------
        private UIElement createSlider(int elementIndex, Globals.UIElementSetup esu)
        {
            UIElement e = new UIElement();

            GameObject goSliderPrefab = Resources.Load<GameObject>("Prefabs/UI/UIPanelSlider");
            if (goSliderPrefab != null)
            {
                e.go = Instantiate(goSliderPrefab);
                e.go.transform.SetParent(container, false);
                e.go.transform.localPosition = new Vector3(e.go.transform.localPosition.x + 10, e.go.transform.localPosition.y - _yPos, 0);

                e.type = esu.type;

                UIPanelSlider script = e.go.GetComponent<UIPanelSlider>();
                script.label.text = esu.label;
                script.slider.value = (esu.defaultValue - esu.rangeMin) / (esu.rangeMax - esu.rangeMin);

                script.elementIndex = elementIndex;
                script.changeHandler += onSliderChange;

                e.script = script;
            }

            _yPos += 40;

            return e;
        }

        // ---------------------------------------------------------------------------------------------
        private UIElement createToggle(int elementIndex, Globals.UIElementSetup esu)
        {
            UIElement e = new UIElement();

            GameObject goTogglePrefab = Resources.Load<GameObject>("Prefabs/UI/UIPanelToggle");
            if (goTogglePrefab != null)
            {
                e.go = Instantiate(goTogglePrefab);
                e.go.transform.SetParent(container, false);
                e.go.transform.localPosition = new Vector3(e.go.transform.localPosition.x + 10, e.go.transform.localPosition.y - _yPos, 0);

                e.type = esu.type;

                UIPanelToggle script = e.go.GetComponent<UIPanelToggle>();
                script.label.text = esu.label;
                script.toggle.isOn = esu.isOn;

                script.elementIndex = elementIndex;
                script.changeHandler += onToggleChange;

                e.script = script;
            }

            _yPos += 40;

            return e;
        }

        // ---------------------------------------------------------------------------------------------
        private UIElement createDropdown(int elementIndex, Globals.UIElementSetup esu)
        {
            UIElement e = new UIElement();

            GameObject goDropdownPrefab = Resources.Load<GameObject>("Prefabs/UI/UIPanelDropdown");
            if (goDropdownPrefab != null)
            {
                e.go = Instantiate(goDropdownPrefab);
                e.go.transform.SetParent(container, false);
                e.go.transform.localPosition = new Vector3(e.go.transform.localPosition.x + 10, e.go.transform.localPosition.y - _yPos, 0);

                e.type = esu.type;

                UIPanelDropdown script = e.go.GetComponent<UIPanelDropdown>();
                script.label.text = esu.label;
                int i, len = esu.dropdownOptions.Count;
                for (i = 0; i < len; ++i) {
                    script.dropdown.options.Add(new Dropdown.OptionData() { text = esu.dropdownOptions[i] });
                }

                script.elementIndex = elementIndex;
                script.changeHandler += onDropdownChange;

                e.script = script;
            }

            _yPos += 60;

            return e;
        }

        // ---------------------------------------------------------------------------------------------
        private void removeElements()
        {
            int i, len = _uiElements.Count;
            for (i = 0; i < len; ++i) {
                UIElement e = _uiElements[i];
                if (e.script != null) {
                    if (e.type == Globals.UIElementType.Slider) {
                        ((UIPanelSlider)e.script).changeHandler -= onSliderChange;
                    }
                    else if (e.type == Globals.UIElementType.Toggle) {
                        ((UIPanelToggle)e.script).changeHandler -= onToggleChange;
                    }
                    else if (e.type == Globals.UIElementType.Toggle) {
                        ((UIPanelDropdown)e.script).changeHandler -= onDropdownChange;
                    }
                }
                e.script = null;
                if (e.go != null) {
                    DestroyImmediate(e.go);
                }
                e.go = null;
            }

            _uiElements.Clear();
        }

        #endregion

        //

        #region EventHandlers

        // ---------------------------------------------------------------------------------------------
        private void onSliderChange(int elementIndex, float value) {

            //Debug.Log("elementIndex "+ elementIndex+" - onSliderChange: " + value);

            Globals.UIElementSetup esu = _setupList[elementIndex];

            // forward change to asset!
            DynamicAsset dynAssetScript = LevelController.Instance.selectedElement.go.GetComponent<DynamicAsset>();
            if (dynAssetScript != null) {
                float realValue = esu.rangeMin + (esu.rangeMax - esu.rangeMin) * value;
                dynAssetScript.updateSliderValue(elementIndex, realValue);
            }
        }

        // ---------------------------------------------------------------------------------------------
        private void onToggleChange(int elementIndex, bool value) {

            //Debug.Log("elementIndex " + elementIndex + " - onToggleChange: " + value);

            Globals.UIElementSetup esu = _setupList[elementIndex];

            // forward change to asset!
            DynamicAsset dynAssetScript = LevelController.Instance.selectedElement.go.GetComponent<DynamicAsset>();
            if (dynAssetScript != null) {
                dynAssetScript.updateToggleValue(elementIndex, value);
            }
        }

        // ---------------------------------------------------------------------------------------------
        private void onDropdownChange(int elementIndex, int value) {

            //Debug.Log("elementIndex " + elementIndex + " - onDropdownChange: " + value);

            Globals.UIElementSetup esu = _setupList[elementIndex];

            // forward change to asset!
            DynamicAsset dynAssetScript = LevelController.Instance.selectedElement.go.GetComponent<DynamicAsset>();
            if (dynAssetScript != null) {
                dynAssetScript.updateDropdownValue(elementIndex, value);
            }
        }

        #endregion
    }
}