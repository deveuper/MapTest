using UnityEngine;
using System.Collections;
namespace xurun
{
    public class SimplePCController : MonoBehaviour
    {
        // Move speed
        public float speed = 5.0f;
        private float walkSpeed;
        [Range(5, 100)]
        //Run speed
        public float runSpeed = 100f;
        private bool isRunning = false;

		public Transform m_FirstCamera;
		public Transform m_ThridCamera;

		private Transform m_Camera;
        private Transform m_Character;
        //Rotate the Y Axis use mouse X
        private Quaternion m_CharacterTargetRot;
        //Rotate the X Axis for Camera use mouse Y
        private Quaternion m_CameraTargetRot;
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;

        private float smoothTime = 5f;
        public bool smoothMouse = false;

        private Rigidbody rigidBody;

        public MouseMoveRotationModel mouseModelRotate = MouseMoveRotationModel.MMORPGGameModel;
        private void InitCamera()
        {
			if (m_Camera == null) {
				switch (mouseModelRotate) {
				case MouseMoveRotationModel.FPSGameModel:
					m_Camera = m_FirstCamera;
					m_ThridCamera.gameObject.SetActive (false);
					break;
				case MouseMoveRotationModel.MMORPGGameModel:
					m_Camera = m_ThridCamera;
					m_FirstCamera.gameObject.SetActive (false);
					break;
				default:
					break;
				}
				m_Camera = Camera.main.transform;
			}
			if (m_Character == null) 
            {
				m_Character = transform;
			}
        }

        // Use this for initialization
        void Start()
        {
            InitCamera();
            //rigidBody = GetComponent<Rigidbody>();
            walkSpeed = speed;
            m_CharacterTargetRot = transform.localRotation;//Get the rotation
            m_CameraTargetRot = m_Camera.localRotation;//Get the rotation
        }
        void Update()
        {
            InputControl();
            MouseRotationModel();
        }
        private void MouseRotationModel()
        {
            switch (mouseModelRotate)
            {
                case MouseMoveRotationModel.FPSGameModel:
                    MouseLook();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                case MouseMoveRotationModel.MMORPGGameModel:
                    if (Input.GetMouseButton(1))
                    {
                        MouseLook();
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                    else
                    {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }

                    break;
                default:
                    break;
            }

        }
        private void MouseLook()
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;
            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);
            if (smoothMouse)
            {
                m_Character.localRotation = Quaternion.Slerp(m_Character.localRotation, m_CharacterTargetRot, smoothTime * Time.deltaTime);
                m_Camera.localRotation = Quaternion.Slerp(m_Camera.localRotation, m_CameraTargetRot, smoothTime * Time.deltaTime);
            }
            else
            {
                m_Character.localRotation = m_CharacterTargetRot;
                m_Camera.localRotation = m_CameraTargetRot;
            }
        }
        private void InputControl()
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Space))
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }
            speed = isRunning ? runSpeed : walkSpeed;
        }

    }
    public enum MouseMoveRotationModel
    {
        FPSGameModel,//use mouse to rotate player
        MMORPGGameModel//down right mouse button and move mouse to rotate player
    }
}