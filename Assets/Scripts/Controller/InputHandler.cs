using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA{
	public class InputHandler : MonoBehaviour {
		public float horizontal;
		public float vertical;

		bool aimInput;
		bool sprint;
		bool shootInput;
		bool crouchInpunt;

		bool reloadInput;
		bool switchInput;
		bool pivotInput;

		bool isInit;

		float delta;

		void Start()
		{
			InitInGame ();
		}

		public void InitInGame()
		{	
			
		}

		void FixedUpdate()
		{
			if (!isInit)
				return;

			delta = Time.fixedDeltaTime; 
		}

		void Update()
		{
			if (!isInit)
				return;

			delta = Time.deltaTime;
		}
	}
}
