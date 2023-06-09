using UnityEngine;
using System.Linq;
using System;
using UnityEngine.EventSystems;

namespace Pathfinding {
	/// <summary>
	/// Moves the target in example scenes.
	/// This is a simple script which has the sole purpose
	/// of moving the target point of agents in the example
	/// scenes for the A* Pathfinding Project.
	///
	/// It is not meant to be pretty, but it does the job.
	/// </summary>
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_target_mover.php")]
	public class TargetMover : MonoBehaviour {
		/// <summary>Mask for the raycast placement</summary>
		private bool boolOffSet = false;
		public bool AllowMove = true;
		public GameObject hitGameObject = null;
		public Vector3 OffSetVector =  new Vector3 (0,2f,0);
		public LayerMask mask;

		public Transform target;
		IAstarAI[] ais;

		/// <summary>Determines if the target position should be updated every frame or only on double-click</summary>
		public bool onlyOnClick;
		public bool use2D;

		public LayerMask layerMask;

		Camera cam;


		public void Start () {
			//Move target to start on player.
			
			//Cache the Main Camera
			cam = Camera.main;
			// Slightly inefficient way of finding all AIs, but this is just an example script, so it doesn't matter much.
			// FindObjectsOfType does not support interfaces unfortunately.

			ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
			useGUILayout = false;
		}

		public void OnGUI () {
			if(AllowMove){
				if (onlyOnClick && cam != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 1) 
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
					if (EventSystem.current.IsPointerOverGameObject() && hit.collider == null) return;
					if (hit.collider != null)
					{
						Debug.Log(hit.collider.name);
						boolOffSet = true;
						hitGameObject = hit.collider.gameObject;
					} else
					{
                        hitGameObject = null;
					}
					UpdateTargetPosition();
				}
			}
		}

		/// <summary>Update is called once per frame</summary>
		void Update () {
			if (!onlyOnClick && cam != null) {
				UpdateTargetPosition();
			}
		}

		public void UpdateTargetPosition () {
			Vector3 newPosition = Vector3.zero;
			bool positionFound = false;

			if (use2D) {
				if(boolOffSet)
					newPosition = cam.ScreenToWorldPoint(Input.mousePosition)- OffSetVector;
				else
					newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
				newPosition.z = 0;
				boolOffSet = false;
				positionFound = true;
			} else {
				// Fire a ray through the scene at the mouse position and place the target where it hits
				RaycastHit hit;
				if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask)) {
					newPosition = hit.point;
					positionFound = true;
				}
			}

			if (positionFound && newPosition != target.position) {
				target.position = newPosition;

				if (onlyOnClick) {
					for (int i = 0; i < ais.Length; i++) {
						if (ais[i] != null) ais[i].SearchPath();
					}
				}
			}
		}
	}
}
