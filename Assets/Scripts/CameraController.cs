using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _Transform_camera;
    private Transform _Transform_Parent;

    private Vector3 _LocalRotation;

    public float MoveSpeed = 30f;
    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 10f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public float minZoomDist = 10f;
    public float maxZoomDist = 60f;

    public Cursor cursor;

    public bool CameraDisabled = true;

    private void Awake()
    {
        this._Transform_camera = this.transform;
        this._Transform_Parent = this.transform.parent;
    }
    public Vector3 GetTransform() { return this._Transform_camera.position; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /// <summary>
    /// Updates the camera movement and rotation based on user input.
    /// </summary>
    void Update()
    {
        if (CameraDisabled)
            Move();
        if (Input.GetKeyDown(KeyCode.R))
            setDefault();
    }

    /// <summary>
    /// Updates the camera rotation and zoom based on user input.
    /// </summary>
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            CameraDisabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
            CameraDisabled = true;

        if (!CameraDisabled)
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y += -Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the y Rotation (Setting its limits), same as -30f < _localRotation.y < 30f
                _LocalRotation.y = Mathf.Clamp(_LocalRotation.y, -30f, 30f);
            }
            //Zooming Input from our Mouse Scroll Wheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                Zoom();
            }
            //Setting PivotCamera rotation
            Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
            this._Transform_Parent.rotation = Quaternion.Lerp(this._Transform_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        }

    }

    /// <summary>
    /// Moves the camera based on user input.
    /// </summary>
    public void Move()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (horizontalInput != 0 || verticalInput != 0)
        {
            float currentYaxis = this._Transform_Parent.localPosition.y;
            float zAxis = verticalInput * MoveSpeed * Time.deltaTime;
            float xAxis = horizontalInput * MoveSpeed * Time.deltaTime;
            this._Transform_Parent.Translate(xAxis, 0, zAxis);
            this._Transform_Parent.position = new Vector3(this._Transform_Parent.position.x, currentYaxis, this._Transform_Parent.position.z); ;
        }
    }

    /// <summary>
    /// Zooms the camera based on user input.
    /// </summary>
    public void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(_Transform_Parent.position, _Transform_camera.position);
        if (dist < minZoomDist && scrollInput > 0.0f)
            return;
        else if (dist > maxZoomDist && scrollInput < 0.0f)
            return;

        _Transform_camera.position += scrollInput * ScrollSensitivity * _Transform_camera.forward;
    }

    /// <summary>
    /// Sets the camera to its default position and rotation.
    /// </summary>
    public void setDefault()
    {
        this._Transform_camera.localPosition = new Vector3(0, 20f, -17f);
        this._Transform_camera.localRotation = Quaternion.Euler(0.5f, 0f, 0f);
        this._Transform_Parent.localRotation = Quaternion.identity;
        _LocalRotation.x = 0;
        _LocalRotation.y = 0;
    }
}
