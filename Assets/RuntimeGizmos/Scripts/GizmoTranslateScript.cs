using UnityEngine;
using System.Collections;

/// <summary>
///     Simple script to handle the functionality of the Translate Gizmo (i.e. move the gizmo
///     and the target object along the axis the user is dragging towards)
/// </summary>
/// 
/// <author>
///     Michael Hillman - thisishillman.co.uk
/// </author>
/// 
/// <version>
///     1.0.0 - 01st January 2016
/// </version>
public class GizmoTranslateScript : MonoBehaviour {

	/// <summary>
	/// Event handler which can be used to handle a position changed event.
	/// </summary>
	public delegate void PositionChangedHandler(Vector3 v3PosChange);
	public event PositionChangedHandler positionChanged;

	/// <summary>
    ///     X axis of gizmo
    /// </summary>
    public GameObject xAxisObject;

    /// <summary>
    ///     Y axis of gizmo
    /// </summary>
    public GameObject yAxisObject;

    /// <summary>
    ///     Z axis of gizmo
    /// </summary>
    public GameObject zAxisObject;

    /// <summary>
    ///     Target for translation
    /// </summary>
    public GameObject translateTarget;

    [HideInInspector]
    public bool handlePressed = false;

    /// <summary>
    ///     Array of detector scripts stored as [x, y, z]
    /// </summary>
    private GizmoClickDetection[] detectors;

	private Vector3 lastPos;

    /// <summary>
    ///     On wake up
    /// </summary>
    public void Awake() {

        // Get the click detection scripts
        detectors = new GizmoClickDetection[3];
        detectors[0] = xAxisObject.GetComponent<GizmoClickDetection>();
        detectors[1] = yAxisObject.GetComponent<GizmoClickDetection>();
        detectors[2] = zAxisObject.GetComponent<GizmoClickDetection>();

        // Set the same position for the target and the gizmo
		if (translateTarget != null) {
			transform.position = translateTarget.transform.position;
		}
    }

	public void init()
	{
		if (translateTarget != null) {
			transform.position = translateTarget.transform.position;
		}
        reset ();
    }

    public void reset () {
        handlePressed = false;
        detectors[0].reset ();
        detectors[1].reset ();
        detectors[2].reset ();
    }

    public void mouseDown () {
        handlePressed = false;
        int i;
        for (i = 0; i < 3; i++) {
            detectors[i].mouseDown ();
            if (detectors[i].pressing) {
                handlePressed = true;
            }
        }
    }

    public void mouseUp() {
        handlePressed = false;
        int i;
        for (i = 0; i < 3; i++) {
            detectors[i].mouseUp ();
        }
    }

    /// <summary>
    ///     Once per frame
    /// </summary>
    public void Update()
    {
        transform.position = translateTarget.transform.position;
        transform.forward  = translateTarget.transform.forward;

		lastPos = translateTarget.transform.position;

        if (!Input.GetMouseButton (0)) {
            return;
        }

        for (int i = 0; i < 3; i++) {
            if (detectors[i].pressing) { // && Input.GetMouseButton(0)

                // Get the distance from the camera to the target (used as a scaling factor in translate)
                float distance = Vector3.Distance(Camera.main.transform.position, translateTarget.transform.position);
                distance = distance * 2.0f;

                // Will store translate values
                Vector3 offset = Vector3.zero;

                switch (i) {
                    // X Axis
                    case 0:
                        {
                            // If the user is pressing the plane, move along Y and Z, else move along X

                            if (detectors[i].pressingPlane) {
                                float deltaY = Input.GetAxis("Mouse Y") * (Time.deltaTime * distance);
								offset = Vector3.up * deltaY; //translateTarget.transform.up * deltaY;
                                offset = new Vector3(0.0f, offset.y, 0.0f);
                                translateTarget.transform.Translate(offset);

                                float deltaZ = Input.GetAxis("Mouse X") * (Time.deltaTime * distance);
								offset = Vector3.forward * deltaZ; //translateTarget.transform.forward * deltaZ;
                                offset = new Vector3(0.0f, 0.0f, offset.z);
                                translateTarget.transform.Translate(offset);

                            } else {
                                float delta = Input.GetAxis("Mouse X") * (Time.deltaTime * distance);
								offset = Vector3.left * delta; //(translateTarget.transform.right*-1) * delta;
                                offset = new Vector3(offset.x, 0.0f, 0.0f);
                                translateTarget.transform.Translate(offset);
                            }
                        }
                        break;

                    // Y Axis
                    case 1:
                        {
                            // If the user is pressing the plane, move along X and Z, else just move along X

                            if (detectors[i].pressingPlane) {
                                float deltaX = Input.GetAxis("Mouse X") * (Time.deltaTime * distance);
								offset = Vector3.right * deltaX; //translateTarget.transform.right * deltaX;
                                offset = new Vector3(offset.x, 0.0f, 0.0f);
                                translateTarget.transform.Translate(offset);

                                float deltaZ = Input.GetAxis("Mouse Y") * (Time.deltaTime * distance);
								offset = Vector3.forward * deltaZ; //translateTarget.transform.forward * deltaZ;
                                offset = new Vector3(0.0f, 0.0f, -offset.z);
                                translateTarget.transform.Translate(offset);

                            } else {
                                float delta = Input.GetAxis("Mouse Y") * (Time.deltaTime * distance);
								offset = Vector3.up * delta; //translateTarget.transform.up * delta;
                                offset = new Vector3(0.0f, offset.y, 0.0f);
                                translateTarget.transform.Translate(offset);
                            }
                        }
                        break;

                    // Z Axis
                    case 2:
                        {
                            // If the user is pressing the plane, move along X and Y, else just move along Z

                            if (detectors[i].pressingPlane) {
                                float deltaX = Input.GetAxis("Mouse X") * (Time.deltaTime * distance);
								offset = Vector3.right * deltaX; //translateTarget.transform.right * deltaX;
                                offset = new Vector3(offset.x, 0.0f, 0.0f);
                                translateTarget.transform.Translate(offset);

                                float deltaY = Input.GetAxis("Mouse Y") * (Time.deltaTime * distance);
								offset = Vector3.up * deltaY; //translateTarget.transform.up * deltaY;
                                offset = new Vector3(0.0f, offset.y, 0.0f);
                                translateTarget.transform.Translate(offset);

                            } else {
                                float delta = Input.GetAxis("Mouse X") * (Time.deltaTime * distance);
								offset = Vector3.forward * delta; //translateTarget.transform.forward * delta;
                                offset = new Vector3(0.0f, 0.0f, offset.z);
                                translateTarget.transform.Translate(offset);
                            }
                        }
                        break;
                }

                // Move the gizmo to match the target position
                transform.position = translateTarget.transform.position;

				if (positionChanged != null) {
					positionChanged (transform.position - lastPos);
				}

                break;
            }
        }
    }

}
// End of script.
