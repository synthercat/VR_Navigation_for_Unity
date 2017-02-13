using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic Navigation script from Antony "Synthercat" Savvidi

public class GEN_CameraRigControl : MonoBehaviour
{
	[Tooltip("Select main camera. If left empty, it will be auto-filled with the first child camera")]
    public Transform cameraTransform;
	[Tooltip("Tick to use the GVR Viewer mode for this scene, Untick for mouse based camera rotation")]
    public bool useVRMode;
	[Tooltip("Axis for forward/backward walking")]
    public string vertiString = "Vertical"; // Suggests Axis gravity = Infinity
	[Tooltip("Axis for sidestep walking")]
	public string horiString = "Horizontal";// Suggests Axis gravity = Infinity
	[Tooltip("Axis for horizontal non-VR control")]
    public string mouseX = "Mouse X";
	[Tooltip("Axis for vertical non-VR control")]
    public string mouseY = "Mouse Y";
	[Tooltip("Speed in Unity units / second")]
    public float walkSpeed = 5f;
	[Tooltip("Rotation in degrees / second")]
    public float turnSpeed = 180f;
	[Tooltip("Sets limits for non-VR Vertical camera control")]
	public float limitVerticalAxis = 90f;
	[Tooltip("Speed when Auto-Walk is enabled")]
	public float autoWalkSpeed = 1f;

	delegate void MyDelegade();
	MyDelegade Move;

	Rigidbody rb;
    float frontWalk;
    float sideWalk;
    float vertLook = 0f;
    float horLook = 0f;
    float turn;
    float turnUD;
    float turnUDSUM;
    Quaternion turnRotation;
	bool autoWalk;
    Vector3 movement;
    Vector3 udRotation;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
		if (cameraTransform == null)
			cameraTransform = GetComponentInChildren<Camera> ().transform;
    }

    private void Start()
    {
		autoWalk = false;
        Cursor.lockState = CursorLockMode.Locked; // Disables mouse
        movement = Vector3.zero;
        udRotation = Vector3.zero;
        turnUDSUM = 0f;

		if (useVRMode) Move = MoveVR;
		else Move = MoveSTD; // Delegate the apropriate method
	}

    void Update()
    {
        frontWalk = Input.GetAxis(vertiString); // Reads front-back
        sideWalk = Input.GetAxis(horiString);   // Reads side step
		if (autoWalk) frontWalk = 1f;           // bypasses walk input in case of autowalk button enabled
    }

    private void FixedUpdate()
    {
		Move(); // Calls MoveVR or MoveSTD depending on the state of useVRMode boolean
    }

    private void MoveSTD()
    {
        Turn(); // Calls method for mouse controlled camera rotation
        movement.Set(sideWalk, 0f, frontWalk);
        movement = movement.normalized * walkSpeed * Time.deltaTime; // Normilise and fix speed
        movement = cameraTransform.TransformDirection(movement);     // Transforms the Vector3 into Camera Coordinates
		movement.y = 0f;
        rb.MovePosition(rb.position + movement);
    }

    private void MoveVR()
    {
        movement.Set(sideWalk, 0f, frontWalk);
		movement = movement.normalized * ((autoWalk) ? autoWalkSpeed : walkSpeed) * Time.deltaTime; // Selects proper speed for auto walk
		movement = cameraTransform.TransformDirection(movement); // Transforms the Vector3 into Camera Coordinates
        movement.y = 0f;
        rb.MovePosition(rb.position + movement);
    } 

    private void Turn() // Uses mouse to look around if GVR Viewer is not used in the scene
    {
		horLook = Input.GetAxis(mouseX);        // Reads mouse for lookaround in case of a non-VR project
		vertLook = Input.GetAxis(mouseY);       // Reads mouse for lookaround in case of a non-VR project
        turn = horLook * turnSpeed * Time.deltaTime;
        turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        turnUD = vertLook * turnSpeed * Time.deltaTime;
        turnUDSUM += turnUD;
		turnUDSUM = Mathf.Clamp(turnUDSUM, -limitVerticalAxis, limitVerticalAxis	);

        udRotation.Set(-turnUDSUM, cameraTransform.eulerAngles.y, 0f);
        cameraTransform.eulerAngles = udRotation;
    }

	public void ToggleautoWalk()
	{
		autoWalk = !autoWalk;
	}

	public bool ReadWalk()
	{
		return autoWalk;
	}
}
