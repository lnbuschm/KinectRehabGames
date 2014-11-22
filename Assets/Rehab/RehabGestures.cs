using UnityEngine;
using System.Collections;
using System;
using System.Timers;

public class RehabGestures : MonoBehaviour {
	public static Boolean rightHandDominant = true;
	public static Boolean rightHandActive = true;
	public float velocityAvgDampfactor = 25.0f;
	public float positionAvgDampfactor = 15.0f;
	public static float xHandOffset = 140.0f;


	public float timedBufferSize = 0.5f;
	public float minSteadyTime = 0.1f;

	TimedBuffer<Vector3> points;
	public float stationaryTimeFrame = 1.0f;
	public float stationaryMaxDistance = 100;

	public float wrongDirectionVelocityThreshold = 0.125f;

	bool IsStationary(float t1, Vector3 p1, float t2, Vector3 p2) {
		Vector3 delta = (p2 - p1);
		delta.z = 0;
		return ((t2 - t1 < stationaryTimeFrame) && (delta.magnitude < stationaryMaxDistance));
	}

	private float[] transformVelocity;
	private float[][] transformPosition;
	private float[][] transformRelPosition;
	private ZigInputJoint[] prevJoints;
	private Vector3 prevLeftHand;
	private Vector3 prevRightHand;
	private DateTime prevTime;
	public float noiseThreshold = 6.0f;

	Boolean isRightHandHigher(ZigInputJoint LH, ZigInputJoint RH) {


		return true;
	}

	float TrackAvgXVelocity(ZigInputJoint joint1, ZigInputJoint joint2, float timeDelta) {

		if (joint1.Id != joint2.Id)
						Debug.LogWarning (" Attemping to calculate velocity of two unequal joints");
		if (joint2.GoodPosition == false || joint1.GoodPosition == false) {
			Debug.LogWarning("Attemping to calculate distance from bad position");
						return 0.0f;
				}
	//	float instVelocity = Math.Abs ((joint2.Position.magnitude-joint1.Position.magnitude) / timeDelta);
	//	float distance = Vector3.Distance (joint2.Position, joint1.Position);
	//	joint2.Position.normalized
		float distance = Math.Abs (joint2.Position.x - joint1.Position.x);

		// filter out noise -- only seems to be needed when using the z component in the distance
		if (distance > noiseThreshold)
						return transformVelocity [(int)joint2.Id];

		float instVelocity = distance / timeDelta;

		float prevVelocity = transformVelocity[(int)joint2.Id];
		transformVelocity [(int)joint2.Id] = (instVelocity + prevVelocity * velocityAvgDampfactor) / (velocityAvgDampfactor + 1);
//		Debug.Log ("Avg vel: " + transformVelocity [(int)joint2.Id] + ", inst vel: " + instVelocity + ", distance: " + 
//		           Vector3.Distance (joint2.Position, joint1.Position) + ", jointID: " + (int)joint2.Id);
		return transformVelocity [(int)joint2.Id];
	}

	float TrackAvgYVelocity(ZigInputJoint joint1, ZigInputJoint joint2, float timeDelta) {
		if (joint1.Id != joint2.Id)
			Debug.LogWarning (" Attemping to calculate velocity of two unequal joints");
		if (joint2.GoodPosition == false || joint1.GoodPosition == false) {
			Debug.LogWarning("Attemping to calculate distance from bad position");
			return 0.0f;
		}

		float distance = Math.Abs (joint2.Position.y - joint1.Position.y);

		// filter out noise -- only seems to be needed when using the z component in the distance
		if (distance > noiseThreshold)
			return transformVelocity [(int)joint2.Id];
		
		float instVelocity = distance / timeDelta;
		
		float prevVelocity = transformVelocity[(int)joint2.Id];
		transformVelocity [(int)joint2.Id] = (instVelocity + prevVelocity * velocityAvgDampfactor) / (velocityAvgDampfactor + 1);
		return transformVelocity [(int)joint2.Id];
	}

	float TrackAvgPosition(ZigInputJoint joint) {

		if (joint.GoodPosition == false) {
			Debug.LogWarning("Attemping to calculate distance from bad position");
			//return 0.0f;
		}
		float prevPositionX = transformPosition[(int)joint.Id][0];

		float prevPositionY = transformPosition[(int)joint.Id][1];
		transformPosition [(int)joint.Id][0] = (joint.Position.x + prevPositionX * positionAvgDampfactor) / (positionAvgDampfactor + 1);

		transformPosition [(int)joint.Id][1] = (joint.Position.y + prevPositionY * positionAvgDampfactor) / (positionAvgDampfactor + 1);
		return transformPosition [(int)joint.Id][1];  // returns y 
	}


	
	float[][] TrackAvgRelPosition(ZigSkeleton userSkeleton) {
	//	if (joint.GoodPosition == false) {
	//		Debug.LogWarning("Attemping to calculate distance from bad position");
			//return 0.0f;
	//	}
		float prevRelPositionX = transformPosition[(int)ZigJointId.LeftHand][0];
		float prevRelPositionY = transformPosition[(int)ZigJointId.LeftHand][1];
		//float relPos = joint.Position.x
		//transformRelPosition [(int)joint.Id][0] = (joint.Position.x + prevRelPositionX * positionAvgDampfactor) / (positionAvgDampfactor + 1);
		
	//	transformRelPosition [(int)joint.Id][1] = (joint.Position.y + prevRelPositionY * positionAvgDampfactor) / (positionAvgDampfactor + 1);
		return transformRelPosition;  // returns y 
	}


	void Zig_UpdateUser(ZigTrackedUser user)
	{
		if (user.SkeletonTracked)
		{
			ZigInputJoint LHjoint = user.Skeleton[(int)ZigJointId.LeftHand];
			ZigInputJoint RHjoint = user.Skeleton[(int)ZigJointId.RightHand];
			//  Keep a running average of each hand's velocity to decide which hand is dominant

			float timeDelta = DateTime.Now.Millisecond - prevTime.Millisecond;
			if (prevJoints[(int)LHjoint.Id] != null) {
				/*
				 * // try comparing hands with velocity
				 * // Check for too fast motion in y direction
				float velRH = TrackAvgXVelocity(prevJoints[(int)RHjoint.Id], RHjoint, timeDelta);
				float velLH = TrackAvgXVelocity(prevJoints[(int)LHjoint.Id], LHjoint, timeDelta);
				if (velRH > velLH && rightHandActive==false) {
					Debug.Log ("RIGHT HAND DOMINANT ACTIVATED!");
					rightHandActive = true;
				//	SendMessage (
				}
				else if (velLH > velRH && rightHandActive==true){
					Debug.Log ("LEFT HAND DOMINANT ACTIVATED!");
					rightHandActive = false;
				}
*/
				if (Time.timeScale == 0) return;  // return if game is not yet active (summary or nextround screen)
				// try comparing hands with position
				float yRH =  TrackAvgPosition( RHjoint);
				float yLH =  TrackAvgPosition(LHjoint);
				if (yRH > yLH && rightHandActive==false) {
					Debug.Log ("RIGHT HAND DOMINANT ACTIVATED!");
					rightHandActive = true;
				}
					//	SendMessage (
				 else if (yLH > yRH && rightHandActive==true){
					Debug.Log ("LEFT HAND DOMINANT ACTIVATED!");
					rightHandActive = false;
				                                         }
				//if (RHjoint.Position.x > LHjoint.Position.x)

				if (rightHandActive) {
					float velRHy = TrackAvgYVelocity(prevJoints[(int)RHjoint.Id], RHjoint, timeDelta);
	//				Debug.Log("y RH Vel: " + velRHy);
		//			if (velRHy > wrongDirectionVelocityThreshold ) RehabMenu.EnableRHWarning();
				//	if (velRHy > wrongDirectionVelocityThreshold ) myRehabMenu.EnableRHWarning();
					if (velRHy > wrongDirectionVelocityThreshold) {
					//	Debug.Log("thrsholdddddddd!!!");
					     GameObject.Find ("RehabMenu").SendMessage("EnableRHWarning");
					}
				}
				else {
					float velLHy = TrackAvgYVelocity(prevJoints[(int)LHjoint.Id], LHjoint, timeDelta);
					if (velLHy > wrongDirectionVelocityThreshold) {
					//	Debug.Log("thrsholdddddddd!!!");
						GameObject.Find ("RehabMenu").SendMessage("EnableLHWarning");
					}

//					Debug.Log("y LH Vel: " + velLHy);
				}

			}

			prevTime = DateTime.Now;
			prevJoints[(int)LHjoint.Id] = LHjoint;
			prevJoints[(int)RHjoint.Id] = RHjoint;


		//	if (prevTime.Millisecond + stationaryTimeFrame >= DateTime.Now.Millisecond)
		//	{
	//			long timeDelta = DateTime.Now.Millisecond - prevTime.Millisecond;
		//	}
			/*	
			foreach (ZigInputJoint joint in user.Skeleton)
			{
				// create a running average velocity to decide on a dominant hand
				if (joint.Id == ZigJointId.RightHand) {
					if (prevRightHand != null) {
						if (IsStationary (DateTime.Now.Millisecond, prevRightHand, prevTime.Millisecond, joint.Position)) 
							Debug.Log ("Stationary RH DETECTED!!!"); // + stationaryTimeFrame);
					}
				//	Vector3 rhvec = user.Skeleton.GetValue(ZigJointId.RightHand);
					Debug.Log ("MAg: " + LHjoint.Position.magnitude);
					prevTime = DateTime.Now;
					prevRightHand = joint.Position;
				}

				if (joint.Id == ZigJointId.RightHand) return;
				//		if (joint.GoodPosition) UpdatePosition(joint.Id, joint.Position);
				//		if (joint.GoodRotation) UpdateRotation(joint.Id, joint.Rotation);
			}
			*/
		}
	//	Debug.Log ("Zig update user");
	}
	/*
	void SteadyDetector_Steady() {
		
		Debug.Log ("STeADY");
	}

	public GameObject[] ShowDuringSession;
	public GameObject[] HideDuringSession;
	void Session_Start()
	{
		//Debug.Log("Session Start from MenuController");
		foreach (GameObject go in ShowDuringSession)
		{
			go.SetActiveRecursively(true);
		}
		foreach (GameObject go in HideDuringSession)
		{
			go.SetActiveRecursively(false);
		}
	}
	void Session_End()
	{
		//Debug.Log("Session End from MenuController");
		foreach (GameObject go in ShowDuringSession)
		{
			go.SetActiveRecursively(false);
		}
		foreach (GameObject go in HideDuringSession)
		{
			go.SetActiveRecursively(true);
		}
	//	items[currentItem].renderer.material.color = origColor;
	}
*/
	//RehabMenu myRehabMenu;
	// Use this for initialization
	void Start () {
//		points = new TimedBuffer<Vector3>(timedBufferSize);
		int jointCount = Enum.GetNames(typeof(ZigJointId)).Length;
		
		transformVelocity = new float[jointCount];
		transformPosition = new float[jointCount][];
		transformPosition [(int)ZigJointId.LeftHand] = new float[2];
		transformPosition [(int)ZigJointId.RightHand] = new float[2];

		for (int i=0; i<jointCount; i++)
						transformVelocity [i] = 0.0f;
		prevTime = DateTime.Now;
		prevJoints = new ZigInputJoint[jointCount];
	//	myRehabMenu = transform.parent.GetComponent<RehabMenu>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
