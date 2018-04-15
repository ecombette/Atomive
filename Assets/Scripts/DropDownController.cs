using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Valve.VR.InteractionSystem
{
	public class DropDownController : MonoBehaviour {

        public List<GameObject> molecule;

        public GameObject canvas;
		public GameObject oxygen;
		public GameObject carbon;
		public GameObject hydrogen;
		public GameObject bound;

		public Hand actingHand;
		private bool available;
		private bool selected;
		private bool hovered;
		private bool creatingBound;
		private bool dragging;
		private bool draggingBound;
		private SteamVR_TrackedController device;
		private GameObject currentBound;
		private float boundLength;
		private GameObject atomHovered;
		private GameObject lastAtomHovered;
		private GameObject boundHovered;
		private bool connected;
		private int atomCode;

		void Start () 
		{
			atomCode = -1;
			device = GetComponent<SteamVR_TrackedController> ();
			available = selected = hovered = false;
			atomHovered  = currentBound = lastAtomHovered = null;
			dragging = false;
			creatingBound = draggingBound = false;
			boundLength = 0;
			connected = false;

        }
	
		void Update () 
		{
			if (draggingBound) 
			{
				
				if (lastAtomHovered != null) {
					boundLength = Vector3.Distance(lastAtomHovered.transform.position, atomHovered.transform.position)/2;
					currentBound.transform.LookAt (lastAtomHovered.transform);
					connected = true;
				} else {
					boundLength = Vector3.Distance(actingHand.transform.position, atomHovered.transform.position)/2;
					currentBound.transform.LookAt(actingHand.transform);
					connected = false;
					TriggerVibration (1000);
				}
				Vector3 newScale = currentBound.transform.localScale;
				newScale.z = boundLength;
				currentBound.transform.localScale = newScale ;


			}
			else if (creatingBound) 
			{
				if (atomHovered)
				{
					currentBound = null;
					currentBound = Instantiate (bound, atomHovered.GetComponent<Transform>().position, atomHovered.GetComponent<Transform>().rotation);
					draggingBound = true;
				}
			}
			else if (dragging) 
			{
				if (atomHovered != null) 
				{
					
					atomHovered.transform.position = actingHand.GetComponent<Transform> ().position;
					TriggerVibration (1000);

				}
			}
			else if ((Input.GetKeyDown (KeyCode.JoystickButton14) || Input.GetKeyDown (KeyCode.JoystickButton15)) && available) {
				switch (atomCode) {

				case -2:
					if (hovered) 
					{
						dragging = true;
					}
					break;
				case -1:
					if (hovered && atomHovered != null) {
						if (atomHovered.GetComponent<AtomBehavior> ().bounds.Count != 0) 
						{
							foreach(GameObject b in atomHovered.GetComponent<AtomBehavior> ().bounds)
							{
								Destroy (b);
							}

						}
						Destroy (atomHovered);
					}
					if (hovered && boundHovered != null && !draggingBound) {

						Destroy (boundHovered);
					}
					break;
				case 0:
					if (hovered) {
						creatingBound = true;
					} else {
						creatingBound = false;
					}
					break;
				case 1:
					Instantiate (hydrogen, actingHand.GetComponent<Transform> ().position, actingHand.GetComponent<Transform> ().rotation);
					TriggerVibration (1000);
					break;
				case 2:
					Instantiate (oxygen, actingHand.GetComponent<Transform> ().position, actingHand.GetComponent<Transform> ().rotation);
					TriggerVibration (1000);
					break;
				case 3:
					Instantiate (carbon, actingHand.GetComponent<Transform> ().position, actingHand.GetComponent<Transform> ().rotation);
					TriggerVibration (1000);
					break;
				default:
					
					break;
				}
			}
			if ((Input.GetKeyUp (KeyCode.JoystickButton14) || Input.GetKeyUp (KeyCode.JoystickButton15)) && selected) 
			{
				available = true;
				creatingBound = false;
				dragging = false;
                if (draggingBound && connected && currentBound != null && lastAtomHovered.GetComponent<AtomBehavior>().isBondable() ) {
					atomHovered.GetComponent<AtomBehavior> ().AddBound (currentBound);
					lastAtomHovered.GetComponent<AtomBehavior> ().AddBound (currentBound);
					currentBound.transform.Find("Cylinder").GetComponent<AtomBehavior> ().AddAtom (atomHovered);
					currentBound.transform.Find("Cylinder").GetComponent<AtomBehavior> ().AddAtom (lastAtomHovered);
					currentBound = null;
					draggingBound = false;
                }
                else 
				{
					Destroy(currentBound);
					draggingBound = false;	

				}
				if (lastAtomHovered != null) {
					atomHovered = lastAtomHovered;
				} 

					lastAtomHovered = null;


			}


		
		}




		public void OnOxygen()
		{
			atomCode = 2;
			selected = true;
			available = false;
			TriggerVibration (1000);
		}

		public void OnHydrogen()
		{
			atomCode = 1;
			selected = true;
			available = false;
			TriggerVibration (1000);

		}
		public void OnCarbon()
		{
			atomCode = 3;
			selected = true;
			available = false;
			TriggerVibration (1000);


		}
		public void OnBound()
		{
			atomCode = 0;
			selected = true;
			available = false;
			TriggerVibration (1000);

		}

		public void OnDel()
		{
			atomCode = -1;
			selected = true;
			available = false;
			TriggerVibration (1000);
		}

		public void OnMove()
		{
			atomCode = -2;
			selected = true;
			available = false;
			TriggerVibration (1000);
		}

		private void TriggerVibration(ushort power)
		{
			actingHand.controller.TriggerHapticPulse (power);
		}

		public void OnAtomHovered(GameObject atom)
		{
			if (!dragging && !draggingBound) {
				atomHovered = atom;
				lastAtomHovered = null;
			} else {
				lastAtomHovered = atom;
			}
			hovered = true;
		}

		public void OnAtomStopHover()
		{
			if (!draggingBound && !dragging) {
				
				atomHovered = null;
			}
			lastAtomHovered = null;


			
			hovered = false;
		}

		public void OnBoundHovered(GameObject bound)
		{
			boundHovered = bound;
			hovered = true;
		}

		public void OnBoundStopHover()
		{
			boundHovered = null;
			hovered = false;
		}

		public bool isMoving()
		{
			return dragging;
		}

		public GameObject atomMoving()
		{
			return atomHovered;
		}

        public void ActivateElectroMagnetique()
        {/*
            foreach (GameObject a in this.molecule)
                a.GetComponent<AtomBehavior>().SetForceActivated(true);

            GameObject fixedAtom = this.molecule[0];
            this.molecule.Remove(fixedAtom);

            Sleep();

           this.molecule.Add(fixedAtom);
           foreach (GameObject a in this.molecule)
                a.GetComponent<AtomBehavior>().SetForceActivated(false);  */
        }

        static IEnumerator Sleep()
        {
            print(Time.time);
            yield return new WaitForSeconds(5);
            print(Time.time);
        }

        public void AddAtomToMolecule(GameObject a)
        {
            molecule.Add(a);
        }
        public void RemoveAtomFromMolecule(GameObject a)
        {
            if (molecule.Contains(a))
                molecule.Remove(a);
        }
    }
}