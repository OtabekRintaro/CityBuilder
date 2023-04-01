using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _xForm_Camera;
    private Transform _xForm_Parent;

    private Vector3 _LocalRotation;

    public float MoveSpeed = 30f;
    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 10f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public float minZoomDist = 10f;
    public float maxZoomDist = 30f;

    public Cursor cursor;

    public bool CameraDisabled = true;

    private void Awake()
    {
        this._xForm_Camera = this.transform;
        this._xForm_Parent = this.transform.parent;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CameraDisabled)
            Move();
        if (Input.GetKeyDown(KeyCode.R)) 
            setDefault();
    }

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
            this._xForm_Parent.rotation = Quaternion.Lerp(this._xForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        }

    }

    void Move()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if(horizontalInput != 0 || verticalInput != 0)
            this._xForm_Parent.Translate(new Vector3(horizontalInput, 0, verticalInput) * MoveSpeed * Time.deltaTime);
    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(_xForm_Parent.position, _xForm_Camera.position);
        if (dist < minZoomDist && scrollInput > 0.0f)
            return;
        else if (dist > maxZoomDist && scrollInput < 0.0f)
            return;

        _xForm_Camera.position +=  scrollInput * ScrollSensitivity * _xForm_Camera.forward;
    }

    void setDefault()
    {
        this._xForm_Camera.SetLocalPositionAndRotation(new Vector3(0, 20f, -17f), new Quaternion(0.500000000000f, 0, 0, 1));
        this._xForm_Parent.localRotation = new Quaternion(0, 0, 0, 1);

        _LocalRotation.x = 0;
        _LocalRotation.y = 0;
    }
}
