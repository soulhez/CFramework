using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 挂载到摄像机上
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    /// <summary>看向的目标 </summary>
    public Transform target;
    /// <summary>摄像机平滑旋转</summary>
    public float smoothCameraRotation = 12f;
    /// <summary>是否锁定相机 </summary>
    public bool lockCamera;
    /// <summary>右侧偏移 </summary>
    public float rightOffset = 0f;
    /// <summary>默认距离 </summary>
    public float defaultDistance = 2.5f;
    /// <summary>高度 </summary>
    public float height = 1.4f;
    /// <summary>平滑跟随 </summary>
    public float smoothFollow = 10f;
    /// <summary>X轴鼠标移速</summary>
    public float xMouseSensitivity = 3f;
    /// <summary>Y轴鼠标移速 </summary>
    public float yMouseSensitivity = 3f;
    /// <summary>旋转角度限制，最小 </summary>
    public float yMinLimit = -40f;
    /// <summary>旋转角度限制，最大 </summary>
    public float yMaxLimit = 80f;



    [HideInInspector]
    public float offSetPlayerPivot;
    [HideInInspector]
    public Transform currentTarget;
    [HideInInspector]
    public Vector2 movementSpeed;

    private Transform targetLookAt;
    private Vector3 currentTargetPos;
    private Vector3 current_cPos;
    private Vector3 desired_cPos;
    private Camera _camera;
    private float distance = 5f;
    private float mouseY = 0f;
    private float mouseX = 0f;
    private float currentHeight;
    private float cullingDistance;
    private float checkHeightRadius = 0.4f;
    private float clipPlaneMargin = 0f;
    private float forward = -1f;
    private float xMinLimit = -360f;
    private float xMaxLimit = 360f;
    private float cullingHeight = 0.2f;
    private float cullingMinDist = 0.1f;




    void Start()
    {
        Init();
    }

    void Init()
    {
        if (target == null)
            return;

        _camera = GetComponent<Camera>();
        currentTarget = target;
        currentTargetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z);

        targetLookAt = new GameObject("targetLookAt").transform;
        targetLookAt.position = currentTarget.position;
        targetLookAt.hideFlags = HideFlags.HideInHierarchy;
        targetLookAt.rotation = currentTarget.rotation;

        mouseY = currentTarget.eulerAngles.x;
        mouseX = currentTarget.eulerAngles.y;

        distance = defaultDistance;
        currentHeight = height;
    }
    private void Update()
    {
        var Y = Input.GetAxis("Mouse X");
        var X = Input.GetAxis("Mouse Y");

        RotateCamera(X, Y);
    }
    private void FixedUpdate()
    {
        if (target == null || targetLookAt == null) return;

        CameraMovement();
    }
    /// <summary>
    /// 摄像机移动
    /// </summary>
    void CameraMovement()
    {
        if (currentTarget == null)
            return;

        distance = Mathf.Lerp(distance, defaultDistance, smoothFollow * Time.deltaTime);
        cullingDistance = Mathf.Lerp(cullingDistance, distance, Time.deltaTime);
        var camDir = (forward * targetLookAt.forward) + (rightOffset * targetLookAt.right);

        camDir = camDir.normalized;

        var targetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z);
        currentTargetPos = targetPos;
        desired_cPos = targetPos + new Vector3(0, height, 0);
        current_cPos = currentTargetPos + new Vector3(0, currentHeight, 0);


        currentHeight = height;

        var lookPoint = current_cPos + targetLookAt.forward * 2f;
        lookPoint += (targetLookAt.right * Vector3.Dot(camDir * (distance), targetLookAt.right));
        targetLookAt.position = current_cPos;

        Quaternion newRot = Quaternion.Euler(mouseX, mouseY, 0);
        targetLookAt.rotation = Quaternion.Slerp(targetLookAt.rotation, newRot, smoothCameraRotation * Time.deltaTime);
        transform.position = current_cPos + (camDir * (distance));
        var rotation = Quaternion.LookRotation((lookPoint) - transform.position);

        transform.rotation = rotation;
        movementSpeed = Vector2.zero;
    }
    /// <summary>
    /// 设置摄像机旋转
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void RotateCamera(float x, float y)
    {
        mouseX += x * xMouseSensitivity;
        mouseY -= y * yMouseSensitivity;

        movementSpeed.x = x;
        movementSpeed.y = y;
        if (!lockCamera)
        {
            mouseY = ClampAngle(mouseY, yMinLimit, yMaxLimit);
            mouseX = ClampAngle(mouseX, xMinLimit, xMaxLimit);
        }
        else
        {
            mouseY = currentTarget.root.localEulerAngles.x;
            mouseX = currentTarget.root.localEulerAngles.y;
        }
    }
    /// <summary>
    /// 限制角度
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float ClampAngle(float angle, float min, float max)
    {
        do
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
        } while (angle < -360 || angle > 360);

        return Mathf.Clamp(angle, min, max);
    }
}
