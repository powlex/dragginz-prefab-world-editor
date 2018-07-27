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
            public int   overwriteStatic;
            public int   overwriteGravity;
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
			
			public PlacementTool.PlacementMode placement;
			public DungeonTool.DungeonPreset dungeon;
			public RoomTool.RoomPattern room;
			
			public int  width;
			public int  height;
			public int  depth;
			public bool ceiling;
			
			public int  radius;
			public int  interval;
			public int  density;
			public bool inverse;
		};

        public struct SelectedElementComponents
        {
            public GameObject         go;
            public List<GameObject>   children;
            public List<Rigidbody>    rigidBodies;
            public List<Collider>     colliders;
            public List<MeshRenderer> meshRenderers;
            public List<Material>     materials;
            public Bounds             bounds;
            public bool               hasSnowShader;

            public void reset()
            {
                go            = null;
                children      = new List<GameObject> ();
                rigidBodies   = new List<Rigidbody> ();
                colliders     = new List<Collider> ();
                meshRenderers = new List<MeshRenderer> ();
                materials     = new List<Material> ();
                hasSnowShader = false;
            }

            public void init (List<GameObject> listGos)
            {
                reset ();

                children = listGos;
                if (children.Count > 0)
                {
                    go = children[0];

                    int i, len = children.Count;
                    for (i = 0; i < len; ++i) {
                        if (children[i].GetComponent<Rigidbody> ()) {
                            rigidBodies.Add (children[i].GetComponent<Rigidbody> ());
                        }
                        if (children[i].GetComponent<Collider> ()) {
                            colliders.Add (children[i].GetComponent<Collider> ());
                        }
                        if (children[i].GetComponent<MeshRenderer> ()) {
                            meshRenderers.Add (children[i].GetComponent<MeshRenderer> ());
                        }
                        int j, len2 = meshRenderers.Count;
                        for (j = 0; j < len2; ++j) {
                            materials.Add (meshRenderers[j].material);
                            if (meshRenderers[j].material.shader.name == Globals.snowShaderName) {
                                hasSnowShader = true;
                            }
                        }
                    }

                    getMeshRendererBounds ();
                }
            }

            public void getMeshRendererBounds ()
            {
                bounds = new Bounds ();

                int i, len = meshRenderers.Count;
                for (i = 0; i < len; ++i) {
                    if (i == 0) {
                        bounds = meshRenderers[i].bounds;
                    }
                    else {
                        bounds.Encapsulate (meshRenderers[i].bounds);
                    }
                }
            }
        };

        #endregion

        //

        #region PrivateAttributes

        public Dictionary<string, LevelElement> levelElements { get; private set; }

        public List<ElementGroup> aElementGroups { get; private set; }
        public int iSelectedGroupIndex { get; set; }

        public LevelElement selectedElement { get; set; }
        private SelectedElementComponents _selectedElementComponents;

        //private List<MeshRenderer> _selectedMeshRenderers;
        //private List<Material> _selectedMaterials;

        private List<GameObject> _tempListOfChildren;
        //private List<GameObject> _childrenOfSelectedElement;

        #endregion

        //

        #region GettersAndSetters

        public Bounds selectedElementBounds { get { return _selectedElementComponents.bounds;  } }

        public bool hasSnowShader { get { return _selectedElementComponents.hasSnowShader; } }

        #endregion

        //

        #region PublicMethods

        // ------------------------------------------------------------------------
        public void init()
		{
			levelElements = new Dictionary<string, LevelElement> ();

			aElementGroups = new List<ElementGroup> ();
			iSelectedGroupIndex = -1;

			//_selectedMeshRenderers = new List<MeshRenderer> ();

            //_selectedMaterials = new List<Material>();

            _tempListOfChildren = new List<GameObject> ();
            //_childrenOfSelectedElement = new List<GameObject>();

            //hasSnowShader = false;

            _selectedElementComponents = new SelectedElementComponents ();
            //selectedElementComponents.reset ();

            resetSelectedElement ();
        }

        // ------------------------------------------------------------------------
        public void clearLevel()
		{
			foreach (KeyValuePair<string, LevelElement> element in levelElements) {
				GameObject.Destroy (element.Value.go);
			}

			levelElements.Clear ();
			aElementGroups.Clear ();

            _selectedElementComponents.reset ();
            //hasSnowShader = false;
        }

        // ------------------------------------------------------------------------
        private void setSelectedElementComponents (bool colliderEnabled, bool useGravity)
        {
            if (selectedElement.go != null) {

                int i, len = _selectedElementComponents.colliders.Count;
                for (i = 0; i < len; ++i) {
                    _selectedElementComponents.colliders[i].enabled = colliderEnabled;
                }

                len = _selectedElementComponents.rigidBodies.Count;
                for (i = 0; i < len; ++i) {
                    _selectedElementComponents.rigidBodies[i].useGravity     = useGravity;
                    _selectedElementComponents.rigidBodies[i].velocity       = Vector3.zero;
                    _selectedElementComponents.rigidBodies[i].freezeRotation = !useGravity;
                    _selectedElementComponents.rigidBodies[i].freezeRotation = !useGravity;
                }
            }
        }

        // ------------------------------------------------------------------------
        public void setComponents (GameObject go, bool colliderEnabled, bool useGravity)
        {
            if (go == null) {
                return;
            }

            SelectedElementComponents comps = new SelectedElementComponents();
            comps.init (getChildrenRecursive (go));
            //Debug.Log ("setComponents for " + go.name + ", total game objects: " +comps.children.Count + ", set: "+colliderEnabled+", "+useGravity);

            int i, len = comps.colliders.Count;
            //Debug.Log ("comps.colliders.Count: "+ comps.colliders.Count);
            for (i = 0; i < len; ++i) {
                comps.colliders[i].enabled = colliderEnabled;
			}

            len = comps.rigidBodies.Count;
            //Debug.Log ("comps.rigidBodies.Count: " + comps.rigidBodies.Count);
            for (i = 0; i < len; ++i) {
                comps.rigidBodies[i].useGravity = useGravity;
                comps.rigidBodies[i].velocity = Vector3.zero;
                comps.rigidBodies[i].freezeRotation = !useGravity;
                comps.rigidBodies[i].freezeRotation = !useGravity;
            }

            comps.reset ();
        }

        // ------------------------------------------------------------------------
        /*public void setMeshColliders (bool state)
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
		}*/

        // ------------------------------------------------------------------------
        /*public void setRigidBody (GameObject go, bool state) {

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
		}*/

        // ------------------------------------------------------------------------
        public void setSnowLevel(GameObject go, float value)
        {
            if (go == null) {
                return;
            }

            float shaderValue = 1f - (2f * value);

            SelectedElementComponents comps = new SelectedElementComponents();
            comps.init (getChildrenRecursive (go));

            int i, len = comps.materials.Count;
            for (i = 0; i < len; ++i) {
                if (comps.materials[i].shader.name == Globals.snowShaderName) {
                    comps.materials[i].SetFloat ("_SnowLevel", shaderValue);
                }
            }

            comps.reset ();
        }

        // ------------------------------------------------------------------------
        public void selectElement (string name)
		{
            //hasSnowShader = false;

            //_childrenOfSelectedElement.Clear();
            //_childrenOfSelectedElement = getChildrenRecursive(selectedElement.go);

            selectedElement = levelElements [name];
            _selectedElementComponents.init (getChildrenRecursive (selectedElement.go));
            setSelectedElementComponents (false, false);
            //setMeshCollider(selectedElement.go, false);
			//setRigidBody (selectedElement.go, false);

			//getSelectedMeshRenderers (selectedElement.go, iSelectedGroupIndex);
			//getSelectedMeshRendererBounds ();

            // snow shader
            /*
            _selectedMaterials.Clear();
            int i, len = _selectedMeshRenderers.Count;
            for (i = 0; i < len; ++i) {
                if (_selectedMeshRenderers[i].material.shader.name == Globals.snowShaderName) {
                    _selectedMaterials.Add(_selectedMeshRenderers[i].material);
                    hasSnowShader = true;
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
        public void changeSelectedElementSnowLevel(float value) {

            float shaderValue = 1f - (2f * value);
            int i, len = _selectedElementComponents.materials.Count;
            for (i = 0; i < len; ++i) {
                if (_selectedElementComponents.materials[i].shader.name == Globals.snowShaderName) {
                    _selectedElementComponents.materials[i].SetFloat ("_SnowLevel", shaderValue);
                }
            }
        }

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
            _selectedElementComponents.reset ();

            //hasSnowShader = false;
        }

        // ------------------------------------------------------------------------
        public void resetSelectedElement()
		{
            LevelElement e = new LevelElement();
			e.part = Globals.PartList.End_Of_List;
            e.overwriteStatic = 0;
            e.overwriteGravity = 0;
            e.shaderSnow = 0;
            e.lightIntensity = 0;

            selectedElement = e;

            _selectedElementComponents.reset ();
            //_selectedMeshRenderers.Clear ();
			//selectedElementBounds = new Bounds();

            //hasSnowShader = false;
        }

        // ------------------------------------------------------------------------
        public void resetElementComponents()
		{
			if (selectedElement.go != null) {

				PrefabLevelEditor.Part part = PrefabLevelEditor.Instance.parts [selectedElement.part];

				//setMeshCollider(selectedElement.go, true);
                bool gravity = (selectedElement.overwriteGravity == 0 ? part.usesGravity : (selectedElement.overwriteGravity == 1 ? true : false));
                //setRigidBody (selectedElement.go, gravity);
                setSelectedElementComponents (true, gravity);
			}
		}

		// ------------------------------------------------------------------------
		public void updatedSelectedObjectPosition(Vector3 posChange)
		{
            //getSelectedMeshRendererBounds ();
            _selectedElementComponents.getMeshRendererBounds ();

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
		/*public void getSelectedMeshRenderers (GameObject go, int iSelectedGroupIndex)
		{
			_selectedMeshRenderers.Clear ();

			_tempListOfChildren.Clear ();

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

			len = _tempListOfChildren.Count;
			for (i = 0; i < len; ++i) {
				if (_tempListOfChildren [i].GetComponent<MeshRenderer> ()) {
					_selectedMeshRenderers.Add(_tempListOfChildren [i].GetComponent<MeshRenderer> ());
				}
			}
		}*/

		// ------------------------------------------------------------------------
		/*private void getSelectedMeshRendererBounds()
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
		}*/

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
		private List<GameObject> getChildrenRecursive(GameObject go, int count = 0)
		{
			if (go == null) {
				return new List<GameObject>();
			}

            if (count == 0) {
                _tempListOfChildren.Clear ();
            }

            _tempListOfChildren.Add (go);

			foreach (Transform child in go.transform)
			{
				if (child != null) {
					_tempListOfChildren.Add (child.gameObject);
					getChildrenRecursive (child.gameObject, ++count);
				}
			}

            return _tempListOfChildren;
		}

		#endregion
	}
}