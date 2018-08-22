//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

namespace PrefabWorldEditor
{
	public class LevelChunk : MonoSingleton<LevelChunk>
    {
        private AssetManager _assetManager = AssetManager.Instance;

        private int _iCounter = 0;

        // ------------------------------------------------------------------------
        public void create(LevelFile levelFile)
        { 
			if (levelFile.levelObjects != null) {
				
				LevelObject levelObj;
                Globals.PartList partId;
                Part part;

                Vector3 pos = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                Vector3 scale = Vector3.one;

                PrefabLevelEditor prefabLevelEditor = PrefabLevelEditor.Instance;
                LevelController levelController = LevelController.Instance;
                AssetManager assetManager = AssetManager.Instance;

                int i, len = levelFile.levelObjects.Count;
				for (i = 0; i<len; ++i)
				{
					levelObj = levelFile.levelObjects[i];

					partId = (Globals.PartList) levelObj.id;
                     part = assetManager.parts[partId];

					pos.x = levelObj.position.x;
					pos.y = levelObj.position.y;
					pos.z = levelObj.position.z;

					rotation.w = levelObj.rotation.w;
					rotation.x = levelObj.rotation.x;
					rotation.y = levelObj.rotation.y;
					rotation.z = levelObj.rotation.z;

                    scale.x = levelObj.scale.x;
                    scale.y = levelObj.scale.y;
                    scale.z = levelObj.scale.z;

                    GameObject go = createPartAt(partId, pos.x, pos.y, pos.z);
                    if (go != null)
                    {
                        //if (XRSettings.enabled) {
                        //    element.go.AddComponent<Teleportable>();
                        //}

                        levelController.setComponents(go, true, part.usesGravity);

                        levelController.setSnowLevel(go, levelObj.shaderSnow);

                        if (levelObj.customData != "") {
                            DynamicAsset dynAssetScript = go.GetComponent<DynamicAsset>();
                            if (dynAssetScript != null) {
                                dynAssetScript.stringToData(levelObj.customData);
                            }
                        }
                    }
				}
			}
		}

        // ------------------------------------------------------------------------
        public GameObject createPartAt (Globals.PartList partId, float x, float y, float z)
        {
            GameObject go = null;

            if (!_assetManager.parts.ContainsKey (partId)) {
                return go;
            }

            if (_assetManager.parts[partId].prefab != null) {
                go = Instantiate (_assetManager.parts[partId].prefab);
                if (go != null) {
                    go.name = "part_" + (_iCounter++).ToString ();
                    go.transform.SetParent (transform);
                    go.transform.position = new Vector3 (x, y, z);
                }
            }

            return go;
        }
    }
}