using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class VRAvatarController : MonoBehaviour
{
    [SerializeField]
    private VRIK _avatarPrefab;
    [SerializeField]
    private GameObject _VRControllerPrefab;
    [SerializeField]
    private GameObject _VRRigPrefab;
    [SerializeField]
    private bool showControllers;

    private GameObject _containerObject;
    public VRIK _actualAvatar;
    private Collider[] _ownColliders;
    private GameObject _VRRigObject;
    private MultiVRSetup _multiVR;
    private VRTK.VRTK_SDKManager _sdkManager;
    private VRTK.VRTK_BezierPointerRenderer _rightControllerTeleport;
    private VRTK.VRTK_BezierPointerRenderer _leftControllerTeleport;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private bool _haveIStarted = false;

    private void Start()
    {
        VRSetup(this.transform.position, this.transform.rotation);
        _updatePlayAreaTransform();
    }

    private void Initialize()
    {
        // We should let the server know we this avatar controller exist on this client side so we can receive the root transform.
        // This also lets the server know that we know this player exists, so that we can receive updates for it in the future.
        Debug.Log(transform.position + " . " + transform.rotation);

        _haveIStarted = true;
    }

    private void OnEnable()
    {
        VRTK.VRTK_DashTeleport.hasFinishedMoving += test;
    }

    private void test()
    {
        if (_avatarPrefab != null)
        {
            IKSolver solver = _avatarPrefab.GetIKSolver();
            IKSolverVR solverVR = solver as IKSolverVR;
            solverVR.Reset();
        }
    }

    /// <summary>
    /// We received a server update with a position and rotation.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void ProcessServerUpdate(Vector3 position, Quaternion rotation)
    {
        if (!_haveIStarted) return;

        // If we already exist, we need to update our values.
        if (_containerObject != null)
        {
            _updateRootTransform(position, rotation);
            return;
        }

        // Otherwise we initialize our play area.
        VRSetup(position, rotation);
    }

    /// <summary>
    /// Update the positions of our container and VR rig (if applicable)
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    protected void _updateRootTransform(Vector3 position, Quaternion rotation)
    {
        Transform container = _containerObject.transform;

        container.position = position;
        container.rotation = rotation;

        // If we are the local player, we must sync the container to our play area.
        Transform playArea = _multiVR.playAreaAlias.transform;

        playArea.position = container.position;
        playArea.rotation = container.rotation;

        _capturePlayAreaTransform();
    }

    /// <summary>
    /// Initialize the VR rig for this avatar.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    private void VRSetup(Vector3 position, Quaternion rotation)
    {
        _containerObject = new GameObject("VRContainer");
        _containerObject.transform.position = position;
        _containerObject.transform.rotation = rotation;

        transform.SetParent(_containerObject.transform, true);
        
        //Current client owns this player
        //create camera rig and attach player model to it

        _VRRigObject = Instantiate(_VRRigPrefab, transform.position, transform.rotation);
        _VRRigObject.transform.SetParent(_containerObject.transform, false);
        _VRRigObject.transform.localPosition = Vector3.zero;

        _sdkManager = _VRRigObject.GetComponentInChildren<VRTK.VRTK_SDKManager>();
        _multiVR = _VRRigObject.GetComponentInChildren<MultiVRSetup>();

        GameObject leftController = Instantiate(_VRControllerPrefab, _containerObject.transform);
        
        GameObject rightController = Instantiate(_VRControllerPrefab, _containerObject.transform);
        
        leftController.name = "LeftController (Clone)";
        rightController.name = "RightController (Clone)";

        // Attach left hand
        MultiVRUtil.MakeReferral(_multiVR.leftHandAlias.gameObject);
        _multiVR.leftHandAlias.transform.SetParent(leftController.transform);

        // Attach right hand
        MultiVRUtil.MakeReferral(_multiVR.rightHandAlias.gameObject);
        _multiVR.rightHandAlias.transform.SetParent(rightController.transform);

        ToggleController(_multiVR.leftHandAlias.gameObject);
        ToggleController(_multiVR.rightHandAlias.gameObject);

        _sdkManager.scriptAliasLeftController = leftController;
        _sdkManager.scriptAliasRightController = rightController;

        //Get Teleport
        _rightControllerTeleport = rightController.GetComponent<VRTK.VRTK_BezierPointerRenderer>();
        _leftControllerTeleport = leftController.GetComponent<VRTK.VRTK_BezierPointerRenderer>();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        _ownColliders = transform.root.GetComponentsInChildren<Collider>();

        #region Avatar Setup
        if (_avatarPrefab != null)
        {
            _actualAvatar = Instantiate(_avatarPrefab, Vector3.zero, Quaternion.identity);
            _actualAvatar.solver.spine.headTarget = transform;
            _actualAvatar.transform.SetParent(_containerObject.transform, false);
            _actualAvatar.transform.rotation = _actualAvatar.transform.rotation * Quaternion.Inverse(rotation);
            _actualAvatar.solver.leftArm.target = leftController.transform;
            _actualAvatar.solver.rightArm.target = rightController.transform;
            // Set actual avatar transforms.
            _actualAvatar.solver.spine.headTarget = _multiVR.headAlias.avatarOffset.transform;
            _actualAvatar.solver.leftArm.target = _multiVR.leftHandAlias.avatarOffset.transform;
            _actualAvatar.solver.rightArm.target = _multiVR.rightHandAlias.avatarOffset.transform;
            this.transform.SetParent(_actualAvatar.solver.spine.headTarget, false);
        }
        #endregion

        _capturePlayAreaTransform();

        Debug.Log("Avatar: " + _actualAvatar);
    }

    protected virtual void LateUpdate()
    {
        _updatePlayAreaTransform();
    }

    private void _capturePlayAreaTransform()
    {
        Transform target = _multiVR.playAreaAlias.transform;
        _lastPosition = target.position;
        _lastRotation = target.rotation;
    }

    private void _updatePlayAreaTransform()
    {
        if (_multiVR == null)
            return;

        Transform target = _multiVR.playAreaAlias.transform;

        if (!_lastPosition.Equals(target.position) || !_lastRotation.Equals(target.rotation))
        {
            // Capture new values.
            _capturePlayAreaTransform();

            ProcessServerUpdate(target.position, target.rotation);
        }
    }

    private void ToggleController(GameObject alias)
    {
        HideController hideController = alias.GetComponentInChildren<HideController>();
        if (hideController != null)
        {
            hideController.ToggleShowControllers(showControllers);
        }
    }
}
