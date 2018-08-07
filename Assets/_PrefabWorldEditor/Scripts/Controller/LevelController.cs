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
			public Globals.PartList partId;
            public PrefabLevelEditor.Part part;
            public int   overwriteStatic;
            public int   overwriteGravity;
            public bool  isLocked;
            public float shaderSnow;

            public bool gravity() {
                return (overwriteGravity == 0 ? part.usesGravity : (overwriteGravity == 1 ? true : false));
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

            // bounds

            public List<GameObject> allGameObjects;
            public Bounds bounds;
            public List<MeshRenderer> meshRenderers;

            public void updateBounds()
            {
                bounds = new Bounds ();
                
                if (meshRenderers != null) {

                    bool firstOneSet = false;
                    int i, len = meshRenderers.Count;
                    for (i = 0; i < len; ++i) {
                        if (meshRenderers[i] == null) {
                            continue;
                        }
                        if (!firstOneSet) {
                            bounds = meshRenderers[i].bounds;
                            //Debug.Log ("first bounds renderer: "+bounds);
                            firstOneSet = true;
                        }
                        else {
                            bounds.Encapsulate (meshRenderers[i].bounds);
                        }
                    }
                }
            }
        };

        public struct SelectedElementComponents
        {
            public GameObject         go;
            public List<GameObject>   children;
            public List<Rigidbody>    rigidBodies;
            public List<Collider>     colliders;
            public List<MeshRenderer> meshRenderers;
            public List<Material>     materials;
            public List<Animator>     animators;
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
                animators     = new List<Animator> ();
                bounds        = new Bounds ();
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
                        if (children[i].GetComponent<Animator> ()) {
                            animators.Add (children[i].GetComponent<Animator> ());
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

        private List<GameObject> _tempListOfChildren;

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

            _tempListOfChildren = new List<GameObject> ();

            _selectedElementComponents = new SelectedElementComponents ();

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
        }

        // ------------------------------------------------------------------------
        private void setSelectedElementComponents (bool colliderEnabled, bool useGravity)
        {
            if (selectedElement.go != null) {

                int i, len = _selectedElementComponents.colliders.Count;
                for (i = 0; i < len; ++i) {
                    _selectedElementComponents.colliders[i].enabled = colliderEnabled;
                }

                len = _selectedElementComponents.animators.Count;
                for (i = 0; i < len; ++i) {
                    _selectedElementComponents.animators[i].enabled = colliderEnabled;
                }

                len = _selectedElementComponents.rigidBodies.Count;
                for (i = 0; i < len; ++i) {
                    _selectedElementComponents.rigidBodies[i].useGravity     = useGravity;
                    _selectedElementComponents.rigidBodies[i].velocity       = Vector3.zero;
                    if (useGravity) {
                        _selectedElementComponents.rigidBodies[i].constraints = RigidbodyConstraints.None;
                    }
                    else {
                        _selectedElementComponents.rigidBodies[i].constraints = RigidbodyConstraints.FreezeAll;
                    }
                }
            }
        }

        // ------------------------------------------------------------------------
        public LevelElement createLevelElement (GameObject go, Globals.PartList partId)
        {
            LevelElement e     = new LevelElement();
            e.go               = go;
            e.partId           = partId;
            e.part             = PrefabLevelEditor.Instance.parts [partId];
            e.overwriteStatic  = 0;
            e.overwriteGravity = 0;
            e.isLocked         = false;
            e.shaderSnow       = 0;

            return e;
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

            len = comps.animators.Count;
            for (i = 0; i < len; ++i) {
                comps.animators[i].enabled = colliderEnabled;
            }

            len = comps.rigidBodies.Count;
            //Debug.Log ("comps.rigidBodies.Count: " + comps.rigidBodies.Count);
            for (i = 0; i < len; ++i) {
                comps.rigidBodies[i].useGravity = useGravity;
                comps.rigidBodies[i].velocity = Vector3.zero;
                if (useGravity) {
                    comps.rigidBodies[i].constraints = RigidbodyConstraints.None;
                }
                else {
                    comps.rigidBodies[i].constraints = RigidbodyConstraints.FreezeAll;
                }
            }

            comps.reset ();
        }

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
            selectedElement = levelElements [name];
            _selectedElementComponents.init (getChildrenRecursive (selectedElement.go));
            setSelectedElementComponents (false, false);
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
        }

        // ------------------------------------------------------------------------
        public void resetSelectedElement()
		{
            LevelElement e = new LevelElement();
			e.partId = Globals.PartList.End_Of_List;
            e.overwriteStatic = 0;
            e.overwriteGravity = 0;
            e.isLocked = false;
            e.shaderSnow = 0;

            selectedElement = e;

            _selectedElementComponents.reset ();
        }

        // ------------------------------------------------------------------------
        public void resetElementComponents()
		{
			if (selectedElement.go != null) {

                bool gravity = (selectedElement.overwriteGravity == 0 ? selectedElement.part.usesGravity : (selectedElement.overwriteGravity == 1 ? true : false));
                setSelectedElementComponents (true, gravity);
			}
		}

		// ------------------------------------------------------------------------
		public void updatedSelectedObjectPosition(Vector3 posChange)
		{
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
        public void updatedSelectedObjectBounds ()
        {
            _selectedElementComponents.getMeshRendererBounds ();
        }

        // ------------------------------------------------------------------------
        public void setElementGroupBounds (ref ElementGroup eg) {

            List<GameObject> allGOsInGroup = new List<GameObject>();

            int i, len = eg.gameObjects.Count;
            for (i = 0; i < len; ++i) {
                List<GameObject> aTemp = getChildrenRecursive(eg.gameObjects[i]);
                int j, len2 = aTemp.Count;
                for (j = 0; j < len2; ++j) {
                    allGOsInGroup.Add (aTemp[j]);
                }
            }

            eg.allGameObjects = allGOsInGroup;
            eg.meshRenderers = new List<MeshRenderer> ();
            len = allGOsInGroup.Count;
            for (i = 0; i < len; ++i) {
                if (allGOsInGroup[i].GetComponent<MeshRenderer> ()) {
                    eg.meshRenderers.Add (allGOsInGroup[i].GetComponent<MeshRenderer> ());
                }
            }

            eg.updateBounds ();
        }

        // ------------------------------------------------------------------------
        //
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

		// ------------------------------------------------------------------------
		public List<GameObject> getChildrenRecursive(GameObject go, int count = 0)
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
				if (child != null && child.gameObject.activeSelf) {
					getChildrenRecursive (child.gameObject, ++count);
				}
			}

            return _tempListOfChildren;
		}

		#endregion
	}
}