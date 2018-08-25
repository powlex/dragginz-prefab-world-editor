//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using System.Collections.Generic;

using UnityEngine;

using AssetsShared;

namespace PrefabWorldEditor
{
    public struct Part
    {
        public Globals.PartList id;
        public Globals.AssetType type;
        public GameObject prefab;

        public float w;
        public float h;
        public float d;

        public Vector3Int canRotate;
        public bool isStatic;
        public bool usesGravity;

        public string name;
        public string extra;
    };

    public class AssetManager : Singleton<AssetManager>
	{
        private Dictionary<Globals.PartList, Part> _parts;
        private Dictionary<Globals.AssetType, List<Part>> _assetTypeList;
        private Dictionary<Globals.AssetType, int> _assetTypeIndex;

        #region Getters

        public Dictionary<Globals.PartList, Part> parts
        {
            get { return _parts; }
        }

        public Dictionary<Globals.AssetType, List<Part>> assetTypeList
        {
            get { return _assetTypeList; }
        }

        public Dictionary<Globals.AssetType, int> assetTypeIndex
        {
            get { return _assetTypeIndex; }
        }

        #endregion

        //

        #region PublicMethods

        public void init()
		{
            _parts = new Dictionary<Globals.PartList, Part> ();

            // Walls
            string assetPath = "Bundle-0/Prefabs/Asset-Type-0/";
            createPart (Globals.PartList.Floor_1, Globals.AssetType.Floor, assetPath + "Floor_1", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 1");
            createPart (Globals.PartList.Floor_2, Globals.AssetType.Floor, assetPath + "Floor_2", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 2");
            createPart (Globals.PartList.Floor_3, Globals.AssetType.Floor, assetPath + "Floor_3", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 3");
            createPart (Globals.PartList.Floor_4, Globals.AssetType.Floor, assetPath + "Floor_4", 4.00f, 0.10f, 4.00f, Vector3Int.zero, false, "Floor 4");

            // Floors
            assetPath = "Bundle-0/Prefabs/Asset-Type-1/";
            createPart (Globals.PartList.Wall_Z, Globals.AssetType.Wall, assetPath + "Wall_Z", 1.00f, 1.00f, 0.25f, Vector3Int.zero, false, "Wall Left",  "Z");
            createPart (Globals.PartList.Wall_X, Globals.AssetType.Wall, assetPath + "Wall_X", 0.25f, 1.00f, 1.00f, Vector3Int.zero, false, "Wall Right", "X");

            // Chunks
            assetPath = "Bundle-0/Prefabs/Asset-Type-2/";
            createPart (Globals.PartList.Chunk_Rock_1,       Globals.AssetType.Chunk, assetPath + "Chunk_Rock_1",       4.00f, 3.50f, 4.00f, Vector3Int.one, false, "Rock 1");
            createPart (Globals.PartList.Chunk_Rock_2,       Globals.AssetType.Chunk, assetPath + "Chunk_Rock_2",       4.00f, 2.40f, 4.00f, Vector3Int.one, false, "Rock 2");
            createPart (Globals.PartList.Chunk_Rock_3,       Globals.AssetType.Chunk, assetPath + "Chunk_Rock_3",       5.00f, 5.00f, 5.00f, Vector3Int.one, false, "Rock 3");
            createPart (Globals.PartList.Chunk_Rock_4,       Globals.AssetType.Chunk, assetPath + "Chunk_Rock_4",       4.00f, 5.50f, 4.00f, Vector3Int.one, false, "Rock 4");
            createPart (Globals.PartList.Chunk_Stalagmite_1, Globals.AssetType.Chunk, assetPath + "Chunk_Stalagmite_1", 2.75f, 4.50f, 2.75f, Vector3Int.one, false, "Stalagmite 1");
            createPart (Globals.PartList.Chunk_Stalagmite_2, Globals.AssetType.Chunk, assetPath + "Chunk_Stalagmite_2", 4.30f, 6.00f, 3.60f, Vector3Int.one, false, "Stalagmite 2");
            createPart (Globals.PartList.Chunk_Stalagmite_3, Globals.AssetType.Chunk, assetPath + "Chunk_Stalagmite_3", 7.25f, 8.80f, 6.25f, Vector3Int.one, false, "Stalagmite 3");
            createPart (Globals.PartList.Chunk_Cliff_1,      Globals.AssetType.Chunk, assetPath + "Chunk_Cliff_1",      8.00f, 8.00f, 4.00f, Vector3Int.one, false, "Cliff 1");
            createPart (Globals.PartList.Chunk_Cliff_2,      Globals.AssetType.Chunk, assetPath + "Chunk_Cliff_2",     10.00f, 8.00f, 7.00f, Vector3Int.one, false, "Cliff 2");
            createPart (Globals.PartList.Chunk_WallEdge,     Globals.AssetType.Chunk, assetPath + "Chunk_WallEdge",     0.25f, 3.00f, 0.30f, Vector3Int.one, false, "Wall Edge");
            createPart (Globals.PartList.Chunk_LargeBricks,  Globals.AssetType.Chunk, assetPath + "Chunk_LargeBricks",  6.00f, 0.75f, 0.75f, Vector3Int.one, false, "Large Bricks");
            createPart (Globals.PartList.Chunk_Block,        Globals.AssetType.Chunk, assetPath + "Chunk_Block",        2.00f, 0.75f, 2.00f, Vector3Int.one, false, "Weird Block");
            createPart (Globals.PartList.Chunk_Corner,       Globals.AssetType.Chunk, assetPath + "Chunk_Corner",       4.00f, 2.00f, 4.00f, Vector3Int.one, false, "Corner Chunk");
            createPart (Globals.PartList.Chunk_Base,         Globals.AssetType.Chunk, assetPath + "Chunk_Base",         4.00f, 2.00f, 4.00f, Vector3Int.one, false, "Rounded Base");

            // Props
            assetPath = "Bundle-0/Prefabs/Asset-Type-3/";
            createPart (Globals.PartList.Prop_Toilet,    Globals.AssetType.Prop, assetPath + "Prop_Toilet",    0.50f, 1.00f, 0.74f, Vector3Int.one, true, "Dirty Toilet");
            createPart (Globals.PartList.Prop_BonePile,  Globals.AssetType.Prop, assetPath + "Prop_BonePile",  2.00f, 0.75f, 2.00f, Vector3Int.one, false, "Bone Pile");
            createPart (Globals.PartList.Prop_Debris,    Globals.AssetType.Prop, assetPath + "Prop_Debris",    3.30f, 1.20f, 3.70f, Vector3Int.one, false, "Debris");
            createPart (Globals.PartList.Prop_Grave_1,   Globals.AssetType.Prop, assetPath + "Prop_Grave_1",   1.00f, 0.88f, 3.00f, Vector3Int.one, true, "Grave");
            createPart (Globals.PartList.Prop_TombStone, Globals.AssetType.Prop, assetPath + "Prop_TombStone", 3.00f, 1.60f, 0.25f, Vector3Int.one, true, "Tomb Stone");
            createPart (Globals.PartList.Pillar_1,       Globals.AssetType.Prop, assetPath + "Pillar_1",       2.00f, 3.00f, 2.00f, Vector3Int.one, true, "Pillar 1");
            createPart (Globals.PartList.Pillar_2,       Globals.AssetType.Prop, assetPath + "Pillar_2",       1.50f, 1.50f, 4.75f, Vector3Int.one, true, "Pillar 2");
            createPart (Globals.PartList.Pillar_3,       Globals.AssetType.Prop, assetPath + "Pillar_3",       1.50f, 1.50f, 1.50f, Vector3Int.one, true, "Pillar Base");

            // Dungeons
            assetPath = "Bundle-0/Prefabs/Asset-Type-4/";
            createPart (Globals.PartList.Dungeon_Floor,     Globals.AssetType.Dungeon, assetPath + "Dungeon_Floor",     2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Floor");
            createPart (Globals.PartList.Dungeon_Wall_L,    Globals.AssetType.Dungeon, assetPath + "Dungeon_Wall_L",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Wall");
            createPart (Globals.PartList.Dungeon_Wall_LR,   Globals.AssetType.Dungeon, assetPath + "Dungeon_Wall_LR",   2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Walls");
            createPart (Globals.PartList.Dungeon_Corner,    Globals.AssetType.Dungeon, assetPath + "Dungeon_Corner",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Corner");
            createPart (Globals.PartList.Dungeon_DeadEnd,   Globals.AssetType.Dungeon, assetPath + "Dungeon_DeadEnd",   2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Dead End");
            createPart (Globals.PartList.Dungeon_Turn,      Globals.AssetType.Dungeon, assetPath + "Dungeon_Turn",      2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Turn");
            createPart (Globals.PartList.Dungeon_T,         Globals.AssetType.Dungeon, assetPath + "Dungeon_T",         2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon T Intersection");
            createPart (Globals.PartList.Dungeon_Stairs_1,  Globals.AssetType.Dungeon, assetPath + "Dungeon_Stairs_1",  2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Stairs Lower");
            createPart (Globals.PartList.Dungeon_Stairs_2,  Globals.AssetType.Dungeon, assetPath + "Dungeon_Stairs_2",  2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Stairs Upper");
            createPart (Globals.PartList.Dungeon_Ramp_1,    Globals.AssetType.Dungeon, assetPath + "Dungeon_Ramp_1",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Ramp Lower");
            createPart (Globals.PartList.Dungeon_Ramp_2,    Globals.AssetType.Dungeon, assetPath + "Dungeon_Ramp_2",    2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Ramp Upper");
            createPart (Globals.PartList.Dungeon_Wall_L_NF, Globals.AssetType.Dungeon, assetPath + "Dungeon_Wall_L_NF", 2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Wall No Floor");
            createPart (Globals.PartList.Dungeon_Corner_NF, Globals.AssetType.Dungeon, assetPath + "Dungeon_Corner_NF", 2.00f, 2.00f, 2.00f, Vector3Int.one, false, "Dungeon Corner No Floor");

            // Lights
            assetPath = "Bundle-0/Prefabs/Asset-Type-5/";
            createPart (Globals.PartList.Light_Lantern, Globals.AssetType.Lights, assetPath + "Light_Lantern", 0.25f, 0.50f, 0.25f, Vector3Int.one, false, "Lantern");
            createPart (Globals.PartList.Light_Torch,   Globals.AssetType.Lights, assetPath + "Light_Torch",   0.25f, 1.50f, 0.25f, Vector3Int.one, false, "Torch");

            // Misc
            assetPath = "Bundle-0/Prefabs/Asset-Type-6/";
            createPart (Globals.PartList.Moving_Platform, Globals.AssetType.Misc, assetPath + "MovingPlatform", 2.00f, 0.50f, 2.00f, Vector3Int.one, false, "Moving Platform");
            createPart (Globals.PartList.Impling,         Globals.AssetType.Misc, assetPath + "Impling",        1.00f, 0.75f, 1.00f, Vector3Int.one, true,  "Impling");

            //

            createAssetTypeCount ();
        }

        #endregion

        //

        #region PrivateMethods

        // ------------------------------------------------------------------------
        private void createPart(
            Globals.PartList id, Globals.AssetType type, string prefab,
            float w, float h, float d, Vector3Int cr, bool ug, string n, string e = "")
        {
            Part p = new Part();

            p.id = id;
            p.type = type;
            p.prefab = Resources.Load<GameObject> (prefab);

            p.w = w;
            p.h = h;
            p.d = d;

            p.canRotate = cr;
            p.usesGravity = ug;
            p.isStatic = !p.usesGravity;

            p.name = n;
            p.extra = e;

            _parts.Add (id, p);
        }

        // ------------------------------------------------------------------------
        private void createAssetTypeCount ()
        {
            _assetTypeList = new Dictionary<Globals.AssetType, List<Part>> ();
            _assetTypeIndex = new Dictionary<Globals.AssetType, int> ();

            foreach (KeyValuePair<Globals.PartList, Part> part in _parts) {

                if (!_assetTypeList.ContainsKey (part.Value.type)) {
                    _assetTypeList.Add (part.Value.type, new List<Part> ());
                    _assetTypeIndex.Add (part.Value.type, 0);
                }

                _assetTypeList[part.Value.type].Add (part.Value);
            }

            //foreach (KeyValuePair<Globals.AssetType, List<Part>> pair in _assetTypeList) {
            //	Debug.Log ("num assets for type "+pair.Key+" = "+pair.Value.Count);
            //}
        }

        #endregion
    }
}