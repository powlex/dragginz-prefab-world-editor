//
// Author  : Oliver Brodhage
// Company : Decentralised Team of Developers
//

using UnityEngine;
using UnityEditor;

namespace PrefabWorldEditor
{
    public class BoundsLineRenderer : MonoBehaviour
    {
        [SerializeField]
        public LineRenderer lineRenderer;

        [HideInInspector]
        public Bounds bounds;

        void Awake () {

            bounds = new Bounds ();
            if (lineRenderer == null) {
                lineRenderer = gameObject.AddComponent<LineRenderer> ();
            }
        }

        void Start() {

            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth   = 0.02f;
            lineRenderer.positionCount = 19;
        }

        public void updateBounds(Bounds b) {

            bounds = b;

            Vector3 pos = new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z);

            // -z facing
            lineRenderer.SetPosition (0, pos);
            lineRenderer.SetPosition (1, pos + new Vector3 (0,        b.size.y, 0));
            lineRenderer.SetPosition (2, pos + new Vector3 (b.size.x, b.size.y, 0));
            lineRenderer.SetPosition (3, pos + new Vector3 (b.size.x, 0,        0));
            lineRenderer.SetPosition (4, pos);

            // -x facing
            lineRenderer.SetPosition (5, pos + new Vector3 (0, 0,        b.size.z));
            lineRenderer.SetPosition (6, pos + new Vector3 (0, b.size.y, b.size.z));
            lineRenderer.SetPosition (7, pos + new Vector3 (0, b.size.y, 0));
            lineRenderer.SetPosition (8, pos);

            // +z facing
            lineRenderer.SetPosition ( 9, pos + new Vector3 (0,        0,        b.size.z)); // move to next start point
            lineRenderer.SetPosition (10, pos + new Vector3 (0,        b.size.y, b.size.z));
            lineRenderer.SetPosition (11, pos + new Vector3 (b.size.x, b.size.y, b.size.z));
            lineRenderer.SetPosition (12, pos + new Vector3 (b.size.x, 0,        b.size.z));
            lineRenderer.SetPosition (13, pos + new Vector3 (0,        0,        b.size.z));

            // +x facing
            lineRenderer.SetPosition (14, pos + new Vector3 (b.size.x, 0,        b.size.z)); // move to next start point
            lineRenderer.SetPosition (15, pos + new Vector3 (b.size.x, b.size.y, b.size.z));
            lineRenderer.SetPosition (16, pos + new Vector3 (b.size.x, b.size.y, 0));
            lineRenderer.SetPosition (17, pos + new Vector3 (b.size.x, 0,        0));
            lineRenderer.SetPosition (18, pos + new Vector3 (b.size.x, 0,        b.size.z));
        }
    }
}