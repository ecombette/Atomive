using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{


	public class AtomBehavior : MonoBehaviour {

        public int type = 0;
		public GameObject DDController;
		public List<GameObject> bounds;         // Liste de liaisons SI Atome
		public List<GameObject> linkedAtoms;    // Liste des atomes liess si Liaison
        public int nbLiaisonMax;
        public Rigidbody rb;
        public const int maxDistance = 2;
        public const float Q = 10f; //refactor
        private float boundLength;
        private bool forcesActivated = false;


        void Start () 
		{
            DDController = GameObject.Find("DropDownController");
        }

        void Update () 
		{
            
            if (linkedAtoms.Count != 0)
            {
                if (bounds.Count == 0  && DDController.GetComponent<DropDownController>().isMoving() && DDController.GetComponent<DropDownController>().atomMoving() == linkedAtoms[0] || DDController.GetComponent<DropDownController>().atomMoving() == linkedAtoms[1])
                {

                    boundLength = Vector3.Distance(linkedAtoms[0].transform.position, linkedAtoms[1].transform.position) / 2;
                    
                    Vector3 newScale = transform.parent.GetComponent<Transform>().localScale;
                    
                    newScale.z = boundLength;
                    transform.parent.GetComponent<Transform>().localScale = newScale;
                    transform.parent.transform.position = linkedAtoms[0].transform.position;
                    transform.parent.transform.LookAt(linkedAtoms[1].transform);
                }
            }
		}

		public void OnHovered()
		{
			DDController.GetComponent<DropDownController> ().OnAtomHovered (gameObject);
		}

		public void OnStopHover()
		{
			DDController.GetComponent<DropDownController> ().OnAtomStopHover ();
		}

		public void OnHoverBound()
		{
			DDController.GetComponent<DropDownController> ().OnBoundHovered (gameObject);
		}

		public void OnStopHoverBound()
		{
			DDController.GetComponent<DropDownController> ().OnBoundStopHover ();
		}

		public void AddBound(GameObject bound)
		{
			bounds.Add (bound);
            if (nbLiaisonMax == this.bounds.Count) DDController.GetComponent<DropDownController>().AddAtomToMolecule(gameObject); 
        }

        public void DeleteBound(GameObject bound)
		{
            List<GameObject> theMolecule = DDController.GetComponent<DropDownController>().molecule;

            if (theMolecule.Contains(bound.GetComponent<AtomBehavior>().linkedAtoms[0]))
                DDController.GetComponent<DropDownController>().RemoveAtomFromMolecule(bound.GetComponent<AtomBehavior>().linkedAtoms[0]);

            if (theMolecule.Contains(bound.GetComponent<AtomBehavior>().linkedAtoms[1]))
                DDController.GetComponent<DropDownController>().RemoveAtomFromMolecule(bound.GetComponent<AtomBehavior>().linkedAtoms[1]);

            bounds.Remove (bound);
		}

		public void AddAtom(GameObject atom)
		{
			linkedAtoms.Add (atom);
		}

		public void RemoveAtom(GameObject atom)
		{
			linkedAtoms.Remove (atom);
            DDController.GetComponent<DropDownController>().RemoveAtomFromMolecule(atom);
        }


        #region MoleculeManager    
        private bool marked = false;

        public bool isMarked() { return marked; }
        public void Mark() { marked = true; }
        public void resetMark() { marked = false; }

        #endregion

        #region BoundsManager
        public bool isBondable()
        {
            return bounds.Count < nbLiaisonMax;
        }

        public GameObject getVoisin(GameObject atom)
        {
            if (linkedAtoms[0] == atom) return linkedAtoms[1];
            if (linkedAtoms[1] == atom) return linkedAtoms[0];
            return null;
        }
        #endregion

        /*
        #region Forces
        void FixedUpdate()
        {
            if (forcesActivated && isMarked() && this.tag == "Atom")
            {
                foreach (GameObject atomToRepulse in DDController.GetComponent<DropDownController>().molecule)
                    if (Vector3.Distance(atomToRepulse.rb.position, this.rb.position) < maxDistance && atomToRepulse != this)
                        ApplyForce(atomToRepulse, false, 1);

                foreach (GameObject bound in bounds)
                    ApplyForce(bound.getVoisin(this), true, 2);
            }
        }

        public void SetForceActivated(bool b)
        {
            this.forcesActivated = b;
        }



        void ApplyForce(GameObject atomToApplyForce, bool attract, int facteur)
        {
            Rigidbody rbToApplyForce = atomToApplyForce.rb;

            Vector3 direction;
            if (attract) direction = this.rb.position - rbToApplyForce.position;
            else direction = rbToApplyForce.position - this.rb.position;

            float distance = direction.magnitude;

            rbToApplyForce.AddForce(direction.normalized * nbLiaisonMax * Q / distance);
        }
    */
       



    }








}
