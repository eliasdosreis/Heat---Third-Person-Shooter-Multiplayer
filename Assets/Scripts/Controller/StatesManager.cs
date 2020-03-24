using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA{
	public class StatesManager : MonoBehaviour 
	{
		public ControllerStates states;
		public ControllerStats stats;

		public InputsVariables inp;

		[System.Serializable]
		public class InputsVariables
		{
			public float horizontal;
			public float vertical;
			public float moveAmount;
			public Vector3 moveDirection;
			public Vector3 aimPosition;
		}

		[System.Serializable]
		public class ControllerStates {
			public bool onGround;
			public bool isAiming;
			public bool isCrouch;
			public bool isRunning;
			public bool isInteracting;
		}	


		public Animator anim;
		public GameObject activeModel;
		[HideInInspector]	
		public Rigidbody rigid;
		[HideInInspector]  
		public Collider controllerCollider;

		List<Collider> ragdollColliders = new List<Collider>();
		List<Rigidbody> ragdollRigids = new List<Rigidbody>();

		public LayerMask ignoreLayers; 
		public LayerMask ignoreForGround;

		public Transform mTransform;
		public CharStates charStates;

		void Start()
		{
			Init ();
		}
		public void Init()
		{
			mTransform = this.transform;
			SetupAnimator ();
			rigid = GetComponent<Rigidbody> ();
			rigid.isKinematic = false;
			rigid.drag = 4;
			rigid.angularDrag = 999;
			rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			controllerCollider = GetComponent<Collider> ();
			SetupRagdoll ();	
			ignoreLayers = ~(1 << 9);
			ignoreForGround = ~(1 << 9 | 1 << 10);

		}

		void SetupAnimator()
		{
			if (activeModel == null) {
				anim = GetComponentInChildren<Animator> ();
				activeModel = anim.gameObject;
			}

			if (anim == null) {
				activeModel = activeModel.GetComponent<GameObject> ();
			}

			anim.applyRootMotion = false; 
		}

		public Rigidbody[] rigids;
		void SetupRagdoll()
		{
			rigids = activeModel.GetComponentsInChildren<Rigidbody> ();

			foreach (Rigidbody r in rigids) 
			{
				if(r == rigid)
					continue;

				Collider c = r.gameObject.GetComponent<Collider> ();
				c.isTrigger = true;

				ragdollRigids.Add(r);
				ragdollColliders.Add(c);
				r.isKinematic = true;
				r.gameObject.layer = 10;
			}

		}

		void MovementNormal()
		{

			if (inp.moveAmount > 0.05f) {
				rigid.drag = 0;
			} else {
				rigid.drag = 4; 
			}
			float speed = stats.moveSpeed;
			if (states.isRunning) {
				speed = stats.sprintSpeed;
			}	
			if (states.isCrouch) {
				speed = stats.crouchSpeed;
			}

			Vector3 dir = Vector3.zero;
			dir = mTransform.forward * (speed * inp.moveAmount);
			rigid.velocity = dir;
		}

		void MovementAiming()
		{

		}

		public void FixedTick()
		{
			switch (charStates) {
			case CharStates.normal:
				states.onGround = OnGround();
				MovementNormal ();
				break;
			case CharStates.onAir:
				rigid.drag = 0;
				states.onGround = OnGround();
				break;
			case CharStates.cover:
				break;
			case CharStates.vaulting:  
				break;
			default:
				break;
			}
		}

		public void Tick()
		{

		}

		public bool OnGround(){
			Vector3 origin = mTransform.position;
			origin.y += 0.6f;
			Vector3 dir = Vector3.up;
			float dis = 0.7f;

			RaycastHit hit;
			if(Physics.Raycast(origin,dir, out hit, dis, ignoreForGround)){
				Vector3 tp = hit.point;
				mTransform.position = tp;
				return true;
			}
				
			return false;
		}

	}

	public enum CharStates
	{
		normal,onAir,cover,vaulting
	}	 
}
