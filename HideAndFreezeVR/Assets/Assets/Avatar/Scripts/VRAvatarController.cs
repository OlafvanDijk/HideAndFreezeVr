using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class VRAvatarController : MonoBehaviour
{
    public bool showControllers;
    public bool enableButtonSelecting;
    public List<VRIK> avatarPrefab;
    [SerializeField]
    private GameObject VRControllerPrefab;
    [SerializeField]
    private GameObject VRRigPrefab;

    public int indexActualAvatar;
    public VRIK actualAvatarVRIK;
    private GameObject containerObject;
    private Collider[] ownColliders;
    private GameObject VRRigObject;
    private MultiVRSetup multiVR;
    private VRTK.VRTK_SDKManager sdkManager;
    private VRTK.VRTK_BezierPointerRenderer rightControllerTeleport;
    private VRTK.VRTK_BezierPointerRenderer leftControllerTeleport;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private bool haveIStarted = false;

    private void Start()
    {
        VRSetup(this.transform.position, this.transform.rotation);
        UpdatePlayAreaTransform();
        haveIStarted = true;
    }

    /// <summary>
    /// After every teleport call the ResetIKSolver method.
    /// </summary>
    private void OnEnable()
    {
        VRTK.VRTK_DashTeleport.hasFinishedMoving += ResetIKSolver;
    }

    /// <summary>
    /// Resets the solver.
    /// </summary>
    private void ResetIKSolver()
    {
        if (avatarPrefab != null)
        {
            IKSolver solver = actualAvatarVRIK.GetIKSolver();
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
        if (!haveIStarted) return;

        // If we already exist, we need to update our values.
        if (containerObject != null)
        {
            UpdateRootTransform(position, rotation);
            return;
        }

        // Otherwise we initialize our play area.
        //VRSetup(position, rotation);
    }

    /// <summary>
    /// Update the positions of our container and VR rig (if applicable)
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    protected void UpdateRootTransform(Vector3 position, Quaternion rotation)
    {
        Transform container = containerObject.transform;
        container.position = position;
        container.rotation = rotation;

        // If we are the local player, we must sync the container to our play area.
        Transform playArea = multiVR.playAreaAlias.transform;

        playArea.position = container.position;
        playArea.rotation = container.rotation;

        CapturePlayAreaTransform();
    }

    /// <summary>
    /// Initialize the VR rig for this avatar.
    /// If there is no avatar then just setup the basic structure.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    private void VRSetup(Vector3 position, Quaternion rotation)
    {
        containerObject = new GameObject("VRContainer");
        containerObject.transform.position = position;
        containerObject.transform.rotation = rotation;

        transform.SetParent(containerObject.transform, true);
        
        //Current client owns this player
        //create camera rig and attach player model to it

        VRRigObject = Instantiate(VRRigPrefab, transform.position, transform.rotation);
        VRRigObject.transform.SetParent(containerObject.transform, false);
        VRRigObject.transform.localPosition = Vector3.zero;

        sdkManager = VRRigObject.GetComponentInChildren<VRTK.VRTK_SDKManager>();
        multiVR = VRRigObject.GetComponentInChildren<MultiVRSetup>();

        GameObject leftController = Instantiate(VRControllerPrefab, containerObject.transform);
        
        GameObject rightController = Instantiate(VRControllerPrefab, containerObject.transform);
        
        leftController.name = "LeftController (Clone)";
        rightController.name = "RightController (Clone)";

        // Attach left hand
        MultiVRUtil.MakeReferral(multiVR.leftHandAlias.gameObject);
        multiVR.leftHandAlias.transform.SetParent(leftController.transform);

        // Attach right hand
        MultiVRUtil.MakeReferral(multiVR.rightHandAlias.gameObject);
        multiVR.rightHandAlias.transform.SetParent(rightController.transform);

        ToggleController(multiVR.leftHandAlias.gameObject);
        ToggleController(multiVR.rightHandAlias.gameObject);

        sdkManager.scriptAliasLeftController = leftController;
        sdkManager.scriptAliasRightController = rightController;

        //Get Teleport
        rightControllerTeleport = rightController.GetComponent<VRTK.VRTK_BezierPointerRenderer>();
        leftControllerTeleport = leftController.GetComponent<VRTK.VRTK_BezierPointerRenderer>();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        ownColliders = transform.root.GetComponentsInChildren<Collider>();

        //leftController.GetComponentInParent<CreatePhotonView>().AddPhotonView(1);
        //rightController.GetComponentInParent<CreatePhotonView>().AddPhotonView(2);

        //multiVR.headAlias.gameObject.AddComponent<CreatePhotonView>().AddPhotonView(3);

        #region Avatar Setup
        if (avatarPrefab != null)
        {
            ApplyAvatar(Random.Range(0, avatarPrefab.Capacity));
        }
        #endregion

        CapturePlayAreaTransform();

        Debug.Log("Avatar: " + actualAvatarVRIK);
    }

    private void ApplyAvatar(int index)
    {
        this.indexActualAvatar = index;
        actualAvatarVRIK = Instantiate(avatarPrefab[index], Vector3.zero, Quaternion.identity);
        actualAvatarVRIK.solver.spine.headTarget = transform;
        actualAvatarVRIK.transform.SetParent(containerObject.transform, false);
        actualAvatarVRIK.transform.rotation = actualAvatarVRIK.transform.rotation * Quaternion.Inverse(containerObject.transform.rotation);
        actualAvatarVRIK.solver.leftArm.target = sdkManager.scriptAliasLeftController.transform;
        actualAvatarVRIK.solver.rightArm.target = sdkManager.scriptAliasRightController.transform;
        // Set actual avatar transforms.
        actualAvatarVRIK.solver.spine.headTarget = multiVR.headAlias.avatarOffset.transform;
        actualAvatarVRIK.solver.leftArm.target = multiVR.leftHandAlias.avatarOffset.transform;
        actualAvatarVRIK.solver.rightArm.target = multiVR.rightHandAlias.avatarOffset.transform;
        this.transform.SetParent(actualAvatarVRIK.solver.spine.headTarget, false);
    }

    protected virtual void LateUpdate()
    {
        UpdatePlayAreaTransform();
    }

    /// <summary>
    /// Get's the current transform of the PlayerArea
    /// </summary>
    private void CapturePlayAreaTransform()
    {
        Transform target = multiVR.playAreaAlias.transform;
        lastPosition = target.position;
        lastRotation = target.rotation;
    }

    /// <summary>
    /// Updates the PlayArea transform if you have moved.
    /// </summary>
    private void UpdatePlayAreaTransform()
    {
        if (multiVR == null)
            return;

        Transform target = multiVR.playAreaAlias.transform;

        if (!lastPosition.Equals(target.position) || !lastRotation.Equals(target.rotation))
        {
            CapturePlayAreaTransform();
            ProcessServerUpdate(target.position, target.rotation);
        }
    }

    /// <summary>
    /// Toggles the visibility of the given controller alias.
    /// </summary>
    /// <param name="alias"></param>
    private void ToggleController(GameObject alias)
    {
        HideController hideController = alias.GetComponentInChildren<HideController>();
        if (hideController != null)
        {
            Debug.Log("hide");
            hideController.ToggleShowControllers(showControllers, enableButtonSelecting);
        }
    }

    public void ChangeAvatar(int indexNewAvatar)
    {
        Destroy(actualAvatarVRIK.gameObject);
        ApplyAvatar(indexNewAvatar);
    }
}
