using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RootMotion.FinalIK {
	
public class VRIKApplier : SimpleUI {
		private VRIK.References myVRIK;

        private enum BoneStructure
        {
            normal = 0,
            toon_kids
        }

        [SerializeField]
        private BoneStructure _boneStructure = BoneStructure.toon_kids;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Button("Apply components")]
	[Order(0)]
	public void applyVRIKComponents(){
            if(_boneStructure == BoneStructure.normal)
            {
                myVRIK = this.GetComponent<VRIK>().references;
                myVRIK.root = this.transform;
                myVRIK.pelvis = findVRIKComponent(this.transform, "Hips");
                myVRIK.spine = findVRIKComponent(this.transform, "Spine");
                //			Debug.Log (findVRIKComponent (this.transform, "Spine"));
                myVRIK.chest = findVRIKComponent(this.transform, "Spine1");
                //			Debug.Log (findVRIKComponent (this.transform, "Spine1"));
                myVRIK.neck = findVRIKComponent(this.transform, "Neck");
                myVRIK.head = findVRIKComponent(this.transform, "Head");
                myVRIK.leftShoulder = findVRIKComponent(this.transform, "LeftShoulder");
                myVRIK.leftUpperArm = findVRIKComponent(this.transform, "LeftArm");
                myVRIK.leftForearm = findVRIKComponent(this.transform, "LeftForeArm");
                myVRIK.leftHand = findVRIKComponent(this.transform, "LeftHand");
                myVRIK.rightShoulder = findVRIKComponent(this.transform, "RightShoulder");
                myVRIK.rightUpperArm = findVRIKComponent(this.transform, "RightArm");
                myVRIK.rightForearm = findVRIKComponent(this.transform, "RightForeArm");
                myVRIK.rightHand = findVRIKComponent(this.transform, "RightHand");
                myVRIK.leftThigh = findVRIKComponent(this.transform, "LeftUpLeg");
                myVRIK.leftCalf = findVRIKComponent(this.transform, "LeftLeg");
                myVRIK.leftFoot = findVRIKComponent(this.transform, "LeftFoot");
                myVRIK.leftToes = findVRIKComponent(this.transform, "LeftToeBase");
                myVRIK.rightThigh = findVRIKComponent(this.transform, "RightUpLeg");
                myVRIK.rightCalf = findVRIKComponent(this.transform, "RightLeg");
                myVRIK.rightFoot = findVRIKComponent(this.transform, "RightFoot");
                myVRIK.rightToes = findVRIKComponent(this.transform, "RightToeBase");
            }
            else if(_boneStructure == BoneStructure.toon_kids)
            {
                myVRIK = this.GetComponent<VRIK>().references;
                myVRIK.root = this.transform;
                myVRIK.pelvis = findVRIKComponent(this.transform, "Hips");
                myVRIK.spine = findVRIKComponent(this.transform, "Spine");
                //			Debug.Log (findVRIKComponent (this.transform, "Spine"));
                myVRIK.chest = findVRIKComponent(this.transform, "Spine1");
                //			Debug.Log (findVRIKComponent (this.transform, "Spine1"));
                myVRIK.neck = findVRIKComponent(this.transform, "Neck");
                myVRIK.head = findVRIKComponent(this.transform, "Head");
                myVRIK.leftShoulder = findVRIKComponent(this.transform, "L Clavicle");
                myVRIK.leftUpperArm = findVRIKComponent(this.transform, "L UpperArm");
                myVRIK.leftForearm = findVRIKComponent(this.transform, "L Forearm");
                myVRIK.leftHand = findVRIKComponent(this.transform, "L Hand");
                myVRIK.rightShoulder = findVRIKComponent(this.transform, "R Clavicle");
                myVRIK.rightUpperArm = findVRIKComponent(this.transform, "R UpperArm");
                myVRIK.rightForearm = findVRIKComponent(this.transform, "R Forearm");
                myVRIK.rightHand = findVRIKComponent(this.transform, "R Hand");
                myVRIK.leftThigh = findVRIKComponent(this.transform, "L Thigh");
                myVRIK.leftCalf = findVRIKComponent(this.transform, "L Calf");
                myVRIK.leftFoot = findVRIKComponent(this.transform, "L Foot");
                myVRIK.leftToes = findVRIKComponent(this.transform, "L Toe0");
                myVRIK.rightThigh = findVRIKComponent(this.transform, "R Thigh");
                myVRIK.rightCalf = findVRIKComponent(this.transform, "R Calf");
                myVRIK.rightFoot = findVRIKComponent(this.transform, "R Foot");
                myVRIK.rightToes = findVRIKComponent(this.transform, "R Toe0");
            }


				
	}


		/* Searches recursively through all children and also their children 
		*for a specific string in the end of an object's name */
		public Transform findVRIKComponent(Transform whereToFind, string nameToFind){
			Transform foundComponent = null;
			foreach (Transform child in whereToFind) {
				if (child.name.EndsWith (nameToFind)) { 
					return child;
				}
				if (foundComponent != null)
					break;
				if (child.childCount > 0) {
					foundComponent = findVRIKComponent (child, nameToFind);
				}
			}
			return foundComponent;
		}



	}

}
