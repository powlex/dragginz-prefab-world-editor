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

        public Text header;

        public Transform container;

        #endregion

        //

        #region PrivateFields

        private struct UIElement
        {
            public GameObject go;
            public MonoBehaviour script;

        };

        private List<UIElement> _uiElements;

        #endregion

        //

        #region SystemMethods

        // ---------------------------------------------------------------------------------------------
        void Awake() {

            _uiElements = new List<UIElement>();

            //show(false);
        }

        // ---------------------------------------------------------------------------------------------
        void Start() {

            GameObject goSliderPrefab = Resources.Load<GameObject>("Prefabs/UI/UIPanelSlider");
            if (goSliderPrefab != null)
            {
                UIElement e = new UIElement();
                e.go = Instantiate(goSliderPrefab);
                e.go.transform.SetParent(container, false);
                e.go.transform.localPosition = new Vector3(e.go.transform.localPosition.x+10, e.go.transform.localPosition.y-10, 0);

                UIPanelSlider script = e.go.GetComponent<UIPanelSlider>();
                script.changeHandler += onSliderChange;

                e.script = script;

                _uiElements.Add(e);
            }
        }

        #endregion

        //

        #region PublicMethods

        // ---------------------------------------------------------------------------------------------
        public void init() {

            removeElements();

            show(true);
        }

        // ---------------------------------------------------------------------------------------------
        public void show(bool state) {

            gameObject.SetActive(false);
        }

        #endregion

        //

        #region PrivateMethods

        private void removeElements() {
            _uiElements.Clear();
        }

        private void onSliderChange(float value) {

            Debug.Log("onSliderChange "+value);
        }

        #endregion
    }
}