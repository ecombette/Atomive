using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{


	public class AtomBehavior : MonoBehaviour {


        public AtomType Symbol;
        public GameObject DDController;
        public int boundCount = 0;
        public int atomCount = 0;
		public List<GameObject> bounds;
        public List<GameObject> boundsToPlace;
        public int boundNumber;         
		public List<GameObject> linkedAtoms;    
        public int maxBounds;
        public Rigidbody rb;

        private int toPlaceBoundCount = 0;
       
        private float boundLength;

        public Material AtomHighlight;

        public List<GameObject> newBounds;

        public bool marked = false; // Marqueur pour l'identification

        void Start () 
		{
            DDController = GameObject.Find("DropDownController");
        }


        public void placeBounds()
        {


            toPlaceBoundCount = 0;
            for (int i = 0; i < linkedAtoms[0].GetComponent<AtomBehavior>().boundCount; i++)
            {
                for (int j = 0; j < linkedAtoms[1].GetComponent<AtomBehavior>().boundCount; j++)
                {

                    
                    if (linkedAtoms[0].GetComponent<AtomBehavior>().bounds[i] == linkedAtoms[1].GetComponent<AtomBehavior>().bounds[j])
                    {
                        boundsToPlace.Add(linkedAtoms[0].GetComponent<AtomBehavior>().bounds[i]);
                        toPlaceBoundCount++;
                    }
                    /*if (bound == secondBound)
                    {
                        boundsToPlace.Add(bound);
                        toPlaceBoundCount++;
                    }*/
                }
            }

           

            /*
            foreach (GameObject bound in linkedAtoms[0].GetComponent<AtomBehavior>().bounds) //pour chaque liaison dans le premier atome linked
            {

                foreach (GameObject secondBound in linkedAtoms[1].GetComponent<AtomBehavior>().bounds) // pour chaque liaison dans le 2e atome linked
                {
                    if (bound == secondBound)
                    {
                        boundsToPlace.Add(bound);
                        toPlaceBoundCount++;
                    }
                    
                }
            }*/
            
            if (toPlaceBoundCount == 1)
            {
                foreach (GameObject toPlaceBound in boundsToPlace)
                {
                    boundLength = Vector3.Distance(linkedAtoms[0].transform.position, linkedAtoms[1].transform.position) / 2;
                    Vector3 newScale = transform.parent.GetComponent<Transform>().localScale;
                    newScale.z = boundLength;
                    transform.parent.GetComponent<Transform>().localScale = newScale;
                    transform.parent.transform.position = linkedAtoms[0].transform.position;
                    transform.parent.transform.rotation = Quaternion.LookRotation(linkedAtoms[1].transform.position - linkedAtoms[0].transform.position);
                    //transform.parent.transform.LookAt(linkedAtoms[1].transform);
                }
            }
            else if (toPlaceBoundCount == 2)
            {
                int side = 1;
                foreach (GameObject toPlaceBound in boundsToPlace)
                {
                    boundLength = Vector3.Distance(linkedAtoms[0].transform.position, linkedAtoms[1].transform.position) / 2;
                    Vector3 newScale = transform.parent.GetComponent<Transform>().localScale;
                    newScale.z = boundLength;
                    toPlaceBound.GetComponent<Transform>().localScale = newScale;
                    toPlaceBound.transform.position = linkedAtoms[0].transform.position + transform.parent.transform.up * (side) * 0.02f;
                    toPlaceBound.transform.rotation = Quaternion.LookRotation(linkedAtoms[1].transform.position - linkedAtoms[0].transform.position);
                    side = -1;
                    //transform.parent.transform.LookAt(linkedAtoms[1].transform);
                }
            }

            else if (toPlaceBoundCount == 3)
            {
                int side = 1;
                foreach (GameObject toPlaceBound in boundsToPlace)
                {
                    boundLength = Vector3.Distance(linkedAtoms[0].transform.position, linkedAtoms[1].transform.position) / 2;
                    Vector3 newScale = transform.parent.GetComponent<Transform>().localScale;
                    newScale.z = boundLength;
                    toPlaceBound.GetComponent<Transform>().localScale = newScale;
                    toPlaceBound.transform.position = linkedAtoms[0].transform.position + transform.parent.transform.up * (side) * 0.02f;
                    toPlaceBound.transform.rotation = Quaternion.LookRotation(linkedAtoms[1].transform.position - linkedAtoms[0].transform.position);
                    side = side - 1;
                    //transform.parent.transform.LookAt(linkedAtoms[1].transform);
                }
            }
            




            boundsToPlace = new List<GameObject>();
            toPlaceBoundCount = 0;


        }
            void Update () 
		{
            if (boundCount < 0) boundCount = 0;

            if (atomCount != 0) //Si jamais on est dans une liaison dont les atomes sont en train de bouger
            {
                if (boundCount == 0 && DDController.GetComponent<DropDownController>().isMoving() && DDController.GetComponent<DropDownController>().atomMoving() == linkedAtoms[0] || DDController.GetComponent<DropDownController>().atomMoving() == linkedAtoms[1])
                {
                    placeBounds();
                    /*
                    toPlaceBoundCount = 0;
                    foreach (GameObject bound in linkedAtoms[0].GetComponent<AtomBehavior>().bounds) //pour chaque liaison dans le premier atome linked
                    {

                        foreach (GameObject secondBound in linkedAtoms[1].GetComponent<AtomBehavior>().bounds) // pour chaque liaison dans le 2e atome linked
                        {
                            if (bound == secondBound)
                            {
                                boundsToPlace.Add(bound);
                                toPlaceBoundCount++;
                            }
                            
                        }
                    }

                            if (toPlaceBoundCount == 1)
                            {
                        Debug.Log("ONE BOUND TO PLACE");
                                foreach (GameObject toPlaceBound in boundsToPlace)
                                {
                                    boundLength = Vector3.Distance(linkedAtoms[0].transform.position, linkedAtoms[1].transform.position) / 2;
                                    Vector3 newScale = transform.parent.GetComponent<Transform>().localScale;
                                    newScale.z = boundLength;
                                    transform.parent.GetComponent<Transform>().localScale = newScale;
                                    transform.parent.transform.position = linkedAtoms[0].transform.position;
                                    transform.parent.transform.rotation = Quaternion.LookRotation(linkedAtoms[1].transform.position - linkedAtoms[0].transform.position);
                                    //transform.parent.transform.LookAt(linkedAtoms[1].transform);
                                }
                            }
                            else if (toPlaceBoundCount == 2)
                            {
                        Debug.Log("TWO BOUND TO PLACE");
                        int side = 1;
                                foreach (GameObject toPlaceBound in boundsToPlace)
                                {
                                    boundLength = Vector3.Distance(linkedAtoms[0].transform.position, linkedAtoms[1].transform.position) / 2;
                                    Vector3 newScale = transform.parent.GetComponent<Transform>().localScale;
                                    newScale.z = boundLength;
                                    toPlaceBound.GetComponent<Transform>().localScale = newScale;
                                    toPlaceBound.transform.position = linkedAtoms[0].transform.position + transform.parent.transform.up * (side) * 0.02f;
                                    toPlaceBound.transform.rotation = Quaternion.LookRotation(linkedAtoms[1].transform.position - linkedAtoms[0].transform.position);
                                    side = -1;
                                    //transform.parent.transform.LookAt(linkedAtoms[1].transform);
                                }
                            }

                    else if (toPlaceBoundCount == 3)
                    {
                        Debug.Log("THREE BOUND TO PLACE");
                        int side = 1;
                        foreach (GameObject toPlaceBound in boundsToPlace)
                        {
                            boundLength = Vector3.Distance(linkedAtoms[0].transform.position, linkedAtoms[1].transform.position) / 2;
                            Vector3 newScale = transform.parent.GetComponent<Transform>().localScale;
                            newScale.z = boundLength;
                            toPlaceBound.GetComponent<Transform>().localScale = newScale;
                            toPlaceBound.transform.position = linkedAtoms[0].transform.position + transform.parent.transform.up * (side) * 0.02f;
                            toPlaceBound.transform.rotation = Quaternion.LookRotation(linkedAtoms[1].transform.position - linkedAtoms[0].transform.position);
                            side = side - 1;
                            //transform.parent.transform.LookAt(linkedAtoms[1].transform);
                        }
                    }
                           

                            
                        
                    
                    Debug.Log("Bounds between the 2 atoms " + toPlaceBoundCount);
                    boundsToPlace = new List<GameObject>();
                    toPlaceBoundCount = 0;

    */
                }
            }
            else if (!DDController.GetComponent<DropDownController>().isDraggingBound())
            {
                if (maxBounds != boundCount)
                {
                    gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("g_vOutlineColor", Color.red);
                }

                else
                {
                    gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("g_vOutlineColor", Color.green);
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
            boundCount++;
            bounds.Add (bound);
        }

        public void DeleteBound(GameObject bound)
		{
            Debug.Log("BOUND DELETED");
            newBounds.Clear();
            boundCount--;
            if (boundCount == 0)
            {
                Debug.Log("LIST CLEARED");
                bounds.Clear();

            }
            else
            {
                foreach (GameObject b in bounds)
                {
                    Debug.Log(b);
                    Debug.Log(bound);
                    
                    //Debug.Log(bound.transform.parent.gameObject);
                    if (b == bound)//.transform.parent.gameObject)
                    {
                        Debug.Log("Bound deleted");
                        continue;
                    }
                    newBounds.Add(b);
                }

                bounds.Clear();
                foreach (GameObject b in newBounds)
                {
                    bounds.Add(b);

                }
                //bounds = newBounds;
                newBounds.Clear();
                //placeBounds();
                
                //bounds.Remove (bound);
            }
        }

		public void AddAtom(GameObject atom)
		{
            atomCount++;

            linkedAtoms.Add (atom);
		}

		public void RemoveAtom(GameObject atom)
		{

            atomCount--;

            linkedAtoms.Remove (atom);
           
        }

        //------------------------------------------------------------------------------
        public List<AtomBehavior> GetNonMarkedNeighboursOfInterest()
        {
            List<AtomBehavior> nonMarkedNeighboursOfInterest = new List<AtomBehavior>();

            List<AtomBehavior> neighbours = GetAllNeighbours();
            Debug.Log("GetNeighbours size : " + neighbours.Count);
            foreach (AtomBehavior atom in neighbours)
            {
                Debug.Log("Atom marked : " + atom.isMarked());
                if (atom.Symbol != AtomType.H && atom.Symbol != AtomType.Void && !atom.isMarked())
                {
                    Debug.Log("Atom added to NoI");
                    nonMarkedNeighboursOfInterest.Add(atom);
                }
            }
            Debug.Log("GetNonMarkedNeighboursOfInterest size : " + nonMarkedNeighboursOfInterest.Count);
            return nonMarkedNeighboursOfInterest;
        }

        public List<AtomBehavior> GetNonMarkedNeighboursByBond(int bondType)
        {
            List<AtomBehavior> nonMarkedNeighboursOfInterest = new List<AtomBehavior>();

            List<AtomBehavior> neighbours = GetAllNeighbours();
            foreach (AtomBehavior atom in neighbours)
            {
                // TODO: CHECK BOND TYPE

                if (atom.Symbol != AtomType.H && atom.Symbol != AtomType.Void && !atom.isMarked())
                {
                    nonMarkedNeighboursOfInterest.Add(atom);
                }
            }

            return nonMarkedNeighboursOfInterest;
        }

        private List<AtomBehavior> GetAllNeighbours()
        {
            List<AtomBehavior> neighbours = new List<AtomBehavior>();

            foreach(GameObject b in bounds)
            {
                Debug.Log("GetAllNeighbours : " + b);
                if (b.transform.Find("Cylinder").GetComponent<AtomBehavior>().linkedAtoms[0] != gameObject)
                {
                    neighbours.Add(b.transform.Find("Cylinder").GetComponent<AtomBehavior>().linkedAtoms[0].GetComponent<AtomBehavior>());
                }
                else
                {
                    neighbours.Add(b.transform.Find("Cylinder").GetComponent<AtomBehavior>().linkedAtoms[1].GetComponent<AtomBehavior>());
                }
            }

            Debug.Log("GetAllNeighbours size : " + neighbours.Count);
            return neighbours;
        }

        public bool isMarked() { return marked; }
        public void Mark() { marked = true; }
        public void resetMark() { marked = false; }
    }








}
