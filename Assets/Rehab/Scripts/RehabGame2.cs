using UnityEngine;
using System.Collections;
using System;

public class RehabGame2 : MonoBehaviour {
	public float RotationDamping = 30.0f;
	private Quaternion[] initialRotations;
	float dz;
	void Zig_UpdateUser(ZigTrackedUser user)
	{
		if (user.SkeletonTracked) {

			// adjust z rotation of hammer according to right hand
		//	user.Skeleton[(int)ZigJointId.RightHand].Position.x 
			//	transform.position += new Vector3(0, fallSpeed, 0);
		//	dz += 1.0f;
			float dPos = Mathf.Abs ( user.Skeleton[(int)ZigJointId.RightElbow].Position.y  - user.Skeleton[(int)ZigJointId.Torso].Position.y );
	//		transform.rotation.Set(0,0,90+dz,0);


	//		if (joint.GoodRotation) UpdateRotation(joint.Id, joint.Rotation);

			Quaternion orientation = user.Skeleton[(int)ZigJointId.RightElbow].Rotation;

			Quaternion newRotation = transform.rotation * orientation * initialRotations[(int)ZigJointId.RightHand];

   //         transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * RotationDamping);
	

			//	transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.time * speed);
	//		transform.rotation = Quaternion.Slerp(transform.rotation.z, transform.rotation.z+dPos, Time.time * 1.0f);
	//		transform.Rotate(new Vector3(0,0,1));
		}
		
	}

	/*
	 *  void UpdateRotation(ZigJointId joint, Quaternion orientation)
    {
        joint = mirror ? mirrorJoint(joint) : joint;
        // make sure something is hooked up to this joint
        if (!transforms[(int)joint])
        {
            return;
        }

        if (UpdateOrientation)
        {
            Quaternion newRotation = transform.rotation * orientation * initialRotations[(int)joint];
            if (mirror)
            {
                newRotation.y = -newRotation.y;
                newRotation.z = -newRotation.z;
            }
            transforms[(int)joint].rotation = Quaternion.Slerp(transforms[(int)joint].rotation, newRotation, Time.deltaTime * RotationDamping);
        }
    }
    */
	public void Awake()
	{
		int jointCount = Enum.GetNames(typeof(ZigJointId)).Length;
		initialRotations = new Quaternion[jointCount];
		initialRotations[(int)ZigJointId.RightElbow] = Quaternion.Inverse(transform.rotation) * transform.rotation;
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
