//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

//using Gamekit3D.WorldBuilding;

namespace PrefabWorldEditor
{
	public class LevelController : Singleton<LevelController>
    {
		#region PublicStructs

		public struct LevelElement
		{
			public GameObject go;
			public Globals.PartList part;
            public int overwriteGravity;
            public float shaderSnow;
            public float lightIntensity;

            public bool gravity() {
                return (overwriteGravity == 0 ? PrefabLevelEditor.Instance.parts[part].usesGravity : (overwriteGravity == 1 ? true : false));
            }
        };

		public struct ElementGroup
		{
			public string groupType;
			public PrefabLevelEditor.Part part;
			public List<GameObject> gameObjects;
			//
			public PlacementTool.PlacementMode placement;
			public DungeonTool.DungeonPreset dungeon;
			public RoomTool.RoomPattern room;
			//
			public int width;
			public int height;
			public int depth;
			public bool ceiling;
			//
			public int radius;
			public int interval;
			public int density;
			public bool inverse;
		};

        #endregion

        //

        #region PrivateAttributes

        public Dictionary<string, LevelElement> levelElements { get; private set; }

        public List<ElementGroup> aElementGroups { get; private set; }
        public int iSelectedGroupIndex { get; set; }

        public LevelElement selectedElement { get; set; }
        public Bounds selectedElementBounds { get; private set; }

        private List<MeshRenderer> _selectedMeshRenderers;
        private List<Material> _selectedMaterials;

        private List<GameObject> _listOfChildren;
        private List<GameObject> _childrenOfSelectedElement;

        public bool hasLightSource { get; private set; }
        public bool hasSnowShader { get; private set; }

        #endregion

        //

        #region PublicMethods

        // ------------------------------------------------------------------------
        public void init()
		{
			levelElements = new Dictionary<string, LevelElement> ();

			aElementGroups = new List<ElementGroup> ();
			iSelectedGroupIndex = -1;

			_selectedMeshRenderers = new List<MeshRenderer> ();

            _selectedMaterials = new List<Material>();

            _listOfChildren = new List<GameObject> ();
            _childrenOfSelectedElement = new List<GameObject>();

            hasLightSource = false;
            hasSnowShader = false;
        }

		// ------------------------------------------------------------------------
		public void clearLevel()
		{
			foreach (KeyValuePair<string, LevelElement> element in levelElements) {
				GameObject.Destroy (element.Value.go);
			}

			levelElements.Clear ();
			aElementGroups.Clear ();

            hasLightSource = false;
            hasSnowShader = false;
        }

        // ------------------------------------------------------------------------
        public void setMeshCollider (GameObject go, bool state) {

			_listOfChildren.Clear ();
			getChildrenRecursive (go);

			int i, len = _listOfChildren.Count;
			for (i = 0; i < len; ++i) {
				if (_listOfChildren [i].GetComponent<Collider> ()) {
					_listOfChildren [i].GetComponent<Collider> ().enabled = state;
				}
			}
		}		

		// ------------------------------------------------------------------------
		public void setMeshColliders (bool state)
		{
			_listOfChildren.Clear ();

			foreach (KeyValuePair<string, LevelController.LevelElement> element in levelElements)
			{
				getChildrenRecursive (element.Value.go);
			}

			int i, len = _listOfChildren.Count;
			for (i = 0; i < len; ++i) {
				if (_listOfChildren [i].GetComponent<Collider> ()) {
					_listOfChildren [i].GetComponent<Collider> ().enabled = state;
				}
			}
		}

		// ------------------------------------------------------------------------
		public void setRigidBody (GameObject go, bool state) {

			if (go.GetComponent<Rigidbody>()) {
				go.GetComponent<Rigidbody>().useGravity = state;
			}
			else {
				foreach (Transform child in go.transform) {
					if (child.gameObject.GetComponent<Rigidbody> ()) {
						child.gameObject.GetComponent<Rigidbody> ().useGravity = state;
					}
				}
			}
		}

        // ------------------------------------------------------------------------
        public void setSnowLevel(GameObject go, float value) {

            float shaderValue = 1f - (2f * value);

            _listOfChildren.Clear();
            getChildrenRecursive(go);

            MeshRenderer renderer;
            int i, len = _listOfChildren.Count;
            for (i = 0; i < len; ++i) {
                renderer = _listOfChildren[i].GetComponent<MeshRenderer>();
                if (renderer != null) {
                    if (renderer.material.shader.name == Globals.snowShaderName) {
                        renderer.material.SetFloat("_SnowLevel", shaderValue);
                    }
                }
            }
        }

        // ------------------------------------------------------------------------
        public void selectElement (string name)
		{
            hasLightSource = false;
            hasSnowShader = false;

            _childrenOfSelectedElement.Clear();
            _childrenOfSelectedElement = getChildrenRecursive(selectedElement.go);

            selectedElement = levelElements [name];

            setMeshCollider(selectedElement.go, false);
			setRigidBody (selectedElement.go, false);

			getSelectedMeshRenderers (selectedElement.go, iSelectedGroupIndex);
			getSelectedMeshRendererBounds ();

            // snow shader
            _selectedMaterials.Clear();
            int i, len = _selectedMeshRenderers.Count;
            for (i = 0; i < len; ++i) {
                if (_selectedMeshRenderers[i].material.shader.name == Globals.snowShaderName) {
                    _selectedMaterials.Add(_selectedMeshRenderers[i].material);
                    hasSnowShader = true;
                }
            }

            // light source
            /*
            len = _childrenOfSelectedElement.Count;
            for (i = 0; i < len; ++i) {
                if (_childrenOfSelectedElement[i].GetComponent<Light>() != null) {
                    hasLightSource = true;
                    break;
                }
            }
            */
        }

        // ------------------------------------------------------------------------
        public void saveSelectElement() {

            if (selectedElement.go != null) {
                string name = selectedElement.go.name;
                if (levelElements.ContainsKey(name)) {
                    levelElements[name] = selectedElement;
                }
            }
        }

        // ------------------------------------------------------------------------
        public void changeSnowLevel(float value) {

            float shaderValue = 1f - (2f * value);
            int i, len = _selectedMaterials.Count;
            for (i = 0; i < len; ++i) {
                _selectedMaterials[i].SetFloat("_SnowLevel", shaderValue);
            }
        }

        // ------------------------------------------------------------------------
        /*public void changeLightIntensity(float value) {

            if (selectedElement.go == null) {
                return;
            }

            float lightIntensity = 0.5f + (2f * value);

            _listOfChildren.Clear();
            getChildrenRecursive(selectedElement.go);

            int i, len = _listOfChildren.Count;
            for (i = 0; i < len; ++i) {
                if (_listOfChildren[i].GetComponent<Light>() != null) {
                    _listOfChildren[i].GetComponent<Light>().intensity = lightIntensity;
                }
            }
        }*/

        // ------------------------------------------------------------------------
        public void deleteSelectedElement ()
		{
            if (selectedElement.go == null) {
                return;
            }

            LevelElement e = selectedElement;

            if (levelElements.ContainsKey (e.go.name)) {
				levelElements.Remove (e.go.name);
			}
			GameObject.Destroy (e.go);
            e.go = null;

			selectedElement = e;

            hasLightSource = false;
            hasSnowShader = false;
        }

        // ------------------------------------------------------------------------
        public void resetSelectedElement()
		{
            LevelElement e = new LevelElement();
			e.part = Globals.PartList.End_Of_List;
            e.overwriteGravity = 0;
            e.shaderSnow = 0;
            e.lightIntensity = 0;

            selectedElement = e;

            _selectedMeshRenderers.Clear ();
			selectedElementBounds = new Bounds();

            hasLightSource = false;
            hasSnowShader = false;
        }

        // ------------------------------------------------------------------------
        public void resetElementComponents()
		{
			if (selectedElement.go != null) {

				PrefabLevelEditor.Part part = PrefabLevelEditor.Instance.parts [selectedElement.part];

				setMeshCollider(selectedElement.go, true);

                bool gravity = (selectedElement.overwriteGravity == 0 ? part.usesGravity : (selectedElement.overwriteGravity == 1 ? true : false));
                setRigidBody (selectedElement.go, gravity);
			}
		}

		// ------------------------------------------------------------------------
		public void updatedSelectedObjectPosition(Vector3 posChange)
		{
			getSelectedMeshRendererBounds ();

			if (iSelectedGroupIndex != -1) {

				Vector3 pos;
				int index = iSelectedGroupIndex;
				int i, len = aElementGroups [index].gameObjects.Count;
				for (i = 0; i < len; ++i) {
					if (aElementGroups [index].gameObjects [i] != null && aElementGroups [index].gameObjects [i] != selectedElement.go) {
						pos = aElementGroups [index].gameObjects [i].transform.position + posChange;
						aElementGroups [index].gameObjects [i].transform.position = pos;
					}
				}
			}
		}

		// ------------------------------------------------------------------------
		public void getSelectedMeshRenderers (GameObject go, int iSelectedGroupIndex)
		{
			_selectedMeshRenderers.Clear ();

			_listOfChildren.Clear ();

			int i, len;

			if (iSelectedGroupIndex != -1)
			{
				len = aElementGroups [iSelectedGroupIndex].gameObjects.Count;
				for (i = 0; i < len; ++i) {
					getChildrenRecursive (aElementGroups [iSelectedGroupIndex].gameObjects [i]);
				}
			}
			else
			{
				getChildrenRecursive (go);
			}

			len = _listOfChildren.Count;
			for (i = 0; i < len; ++i) {
				if (_listOfChildren [i].GetComponent<MeshRenderer> ()) {
					_selectedMeshRenderers.Add(_listOfChildren [i].GetComponent<MeshRenderer> ());
				}
			}
		}

		// ------------------------------------------------------------------------
		public void getSelectedMeshRendererBounds()
		{
			if (selectedElement.go != null) {
				if (selectedElement.part != Globals.PartList.End_Of_List) {

                    if (_selectedMeshRenderers.Count > 0) {
						selectedElementBounds = _selectedMeshRenderers [0].bounds;
						int i, len = _selectedMeshRenderers.Count;
						for (i = 1; i < len; ++i) {
							selectedElementBounds.Encapsulate (_selectedMeshRenderers [i].bounds);
                        }
					}
				}
			}
		}

		// ------------------------------------------------------------------------
		public int findElementInGroup(GameObject go)
		{
			int index = -1;
			int i, len = aElementGroups.Count;
			for (i = 0; i < len; ++i) {
				int j, len2 = aElementGroups [i].gameObjects.Count;
				for (j = 0; j < len2; ++j) {
					if (aElementGroups [i].gameObjects [j] == go) {
						index = i;
						i = len;
						break;
					}
				}
			}

			return index;
		}

		#endregion

		#region PrivateMethods

		// ------------------------------------------------------------------------
		// Private Methods
		// ------------------------------------------------------------------------
		private List<GameObject> getChildrenRecursive(GameObject go)
		{
			if (go == null) {
				return new List<GameObject>();
			}

			_listOfChildren.Add (go);

			foreach (Transform child in go.transform)
			{
				if (child != null) {
					_listOfChildren.Add (child.gameObject);
					getChildrenRecursive (child.gameObject);
				}
			}

            return _listOfChildren;
		}

		#endregion
	}
}