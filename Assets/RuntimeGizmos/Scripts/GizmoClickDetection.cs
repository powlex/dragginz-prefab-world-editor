using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
///     Simple script to detect if the attached gameObject is being clicked upon by the user, applied
///     to an individual gizmo handle/element.
/// </summary>
/// 
/// <author>
///     Michael Hillman - thisishillman.co.uk
/// </author>
/// 
/// <version>
///     1.0.0 - 01st January 2016
/// </version>
public class GizmoClickDetection : MonoBehaviour {

    /// <summary>
    ///     Have we already click on one handle?
    /// </summary>
    public static bool ALREADY_CLICKED;

    /// <summary>
    ///     Camera that has been set to the gizmo layer
    /// </summary>
    public Camera gizmoCamera;

    /// <summary>
    ///     Layer upon which gizmos sit
    /// </summary>
    public LayerMask gizmoLayer;

    /// <summary>
    ///     Targets to detect clicks upon, and highlight when a click is detected
    /// </summary>
    public GameObject[] targets;

    /// <summary>
    ///     Highlight material
    /// </summary>
    public Material highlight;

    /// <summary>
    ///     Dictionary of previous materials before a highlight
    /// </summary>
    public Dictionary<MeshRenderer, Material> previousMaterials;

    /// <summary>
    ///     Is the user currently pressing
    /// </summary>
    [HideInInspector]
    public bool pressing = false;

    /// <summary>
    ///     Is the user pressing the plane area?
    /// </summary>
    [HideInInspector]
    public bool pressingPlane = false;

    private Ray ray;
    //private int layerMask = 1 << 11;

    /// <summary>
    ///     On wake-up
    /// </summary>
    public void Awake() {
        previousMaterials = new Dictionary<MeshRenderer, Material>();
    }

    public void mouseDown ()
    {
        if (!ALREADY_CLICKED) {

            // Detect the object(s) the user has clicked
            ray = gizmoCamera.ScreenPointToRay (Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f, gizmoLayer);
            bool detected = false;
            pressingPlane = false;

            // Check if object are our targets (skipping the collision if the renderer isn't enabled)
            foreach (RaycastHit hit in hits) {
                if (Array.IndexOf (targets, hit.collider.gameObject) >= 0) {
                    //Debug.Log("hit.collider.gameObject: " + hit.collider.gameObject.name);
                    if (!hit.collider.gameObject.GetComponent<Renderer> ().enabled) continue;
                    if (hit.collider.gameObject.name.Contains ("_plane_")) pressingPlane = true;
                    detected = true;
                    pressing = true;
                }
            }

            if (detected) {
                // Store the current materials of the targets, then highlight them
                if (previousMaterials != null) previousMaterials.Clear ();

                foreach (GameObject target in targets) {

                    try {
                        foreach (MeshRenderer renderer in target.GetComponentsInChildren<MeshRenderer> (false)) {
                            previousMaterials[renderer] = renderer.sharedMaterial;
                            renderer.material = highlight;
                        }
                    }
                    catch (NullReferenceException exception) {
                        // Perhaps no previous materials could be found?
                        Debug.Log (exception.Message);
                    }

                }

                ALREADY_CLICKED = true;
            }
        }
    }

    public void mouseUp()
    {
        // If the left mouse button was released and we haven't un-highlighted yet 
        if (previousMaterials.Count > 0) {

            foreach (MeshRenderer renderer in previousMaterials.Keys) {
                renderer.material = previousMaterials[renderer];
            }
            previousMaterials.Clear ();
            pressing = false;
            pressingPlane = false;
            ALREADY_CLICKED = false;
        }
    }

    /// <summary>
    ///     Checks for hits on the target objects, highlighting when found
    /// </summary>
    /*public void Update () {

        // If the left mouse button is pressed      
        if (Input.GetMouseButtonDown (0))
        {
            if (!ALREADY_CLICKED) {

                // Detect the object(s) the user has clicked
                ray = gizmoCamera.ScreenPointToRay (Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 100f, gizmoLayer);
                bool detected = false;
                pressingPlane = false;

                // Check if object are our targets (skipping the collision if the renderer isn't enabled)
                foreach (RaycastHit hit in hits) {
                    if (Array.IndexOf (targets, hit.collider.gameObject) >= 0) {
                        //Debug.Log("hit.collider.gameObject: " + hit.collider.gameObject.name);
                        if (!hit.collider.gameObject.GetComponent<Renderer> ().enabled) continue;
                        if (hit.collider.gameObject.name.Contains ("_plane_")) pressingPlane = true;
                        detected = true;
                        pressing = true;
                    }
                }

                if (detected) {
                    // Store the current materials of the targets, then highlight them
                    if (previousMaterials != null) previousMaterials.Clear ();

                    foreach (GameObject target in targets) {

                        try {
                            foreach (MeshRenderer renderer in target.GetComponentsInChildren<MeshRenderer> (false)) {
                                previousMaterials[renderer] = renderer.sharedMaterial;
                                renderer.material = highlight;
                            }
                        } catch (NullReferenceException exception) {
                            // Perhaps no previous materials could be found?
                            Debug.Log (exception.Message);
                        }

                    }

                    ALREADY_CLICKED = true;
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            // If the left mouse button was released and we haven't un-highlighted yet 
            if (previousMaterials.Count > 0) {

                foreach (MeshRenderer renderer in previousMaterials.Keys) {
                    renderer.material = previousMaterials[renderer];
                }
                previousMaterials.Clear ();
                pressing = false;
                pressingPlane = false;
                ALREADY_CLICKED = false;
            }
        }
	}*/

    public void reset() {
        pressing = false;
        pressingPlane = false;
        ALREADY_CLICKED = false;
    }
}
// End of script.
