//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace PrefabWorldEditor
{
    public static class CombineMeshes
    {
        public static void run(GameObject go)
        {
            go.transform.localPosition = Vector3.zero;

            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 0;
            while (i < meshFilters.Length) {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }

            GameObject goNew = new GameObject("combinedMesh");
            goNew.transform.SetParent(go.transform);

            MeshFilter mf = goNew.AddComponent<MeshFilter>();
            mf.mesh = new Mesh();
            mf.mesh.CombineMeshes(combine);
            MeshRenderer mr = goNew.AddComponent<MeshRenderer>();
            Material mat = Resources.Load<Material>("MDC/Materials/CATA_BrickWallA2");
            mr.material = mat;

            AssetDatabase.CreateAsset(mf.mesh, Globals.resourcesPath+"Models/Dynamic/"+go.name+"-combinedMeshes.mat");

            //Vector3 extent = mf.mesh.bounds.extents;
            //goNew.transform.position = new Vector3(-extent.x, -extent.y, -extent.z);

            goNew.AddComponent<MeshCollider>();

            PrefabUtility.CreatePrefab(Globals.resourcesPath+"Prefabs/Dynamic/"+go.name+"-combinedMeshes.prefab", go);
        }
    }
}

#endif