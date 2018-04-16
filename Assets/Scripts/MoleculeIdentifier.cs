using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: to be amended when extending the dictionary
public enum AtomType
{
    H, C, O, Void
};

namespace Valve.VR.InteractionSystem
{
    public class MoleculeIdentifier : MonoBehaviour
    {
        private Dictionary<string, string> moleculeDictionary;
        private string moleculeIdentified;


        void Start()
        {
            moleculeDictionary = gameObject.GetComponent<MoleculeDictionary>().molecules;
            Debug.Log("Identification dictionary size : " + moleculeDictionary.Count);
        }

        public string IdentifyMolecule()
        {
            foreach (KeyValuePair<string, string> pair in moleculeDictionary)
            {
                if (SameMoleculeAs(pair.Key))
                {
                    moleculeIdentified = pair.Key;
                    return pair.Key;
                }
            }

            Debug.Log("This molecule does not exist in our database");
            moleculeIdentified = string.Empty;
            return "Inconnue";
        }

        public string GetMoleculeIdentified()
        {
            return moleculeIdentified;
        }

        public void OnMoleculeId()
        {
            IdentifyMolecule();
        }

        private bool SameMoleculeAs(string index)
        {
            Debug.Log("SameMolecule : " + moleculeDictionary[index]);
            // We retrieve a copy of the SMILES from the dictionary
            string smiles = string.Copy(moleculeDictionary[index]);
            // We retrieve the atoms in the molecule GameObject
            GameObject[] atomsInMolecule = GameObject.FindGameObjectsWithTag("Atom");
            Debug.Log("Atoms retrieved from molecule object : " + atomsInMolecule.Length);

            // We retrieve the starting points of the recursion...
            while(smiles[0] == '(')
            {
                smiles = smiles.Substring(1);
            }
            AtomType smilesStartingSymbol = GetSmilesStartingSymbol(ref smiles);
            Debug.Log("SameMolecule starting symbol : " + smilesStartingSymbol);
            List<AtomBehavior> startingPoints = GetStartingPointsByType(atomsInMolecule, smilesStartingSymbol);
            // ...and start it from there
            bool match = false;
            foreach(AtomBehavior atom in startingPoints)
            {
                ResetAllMarks(atomsInMolecule);
                if (CompareBranchFrom(ref smiles, atom))
                {
                    match = true;
                    break;
                }
            }

            // we clean all our atoms
            ResetAllMarks(atomsInMolecule);

            if (match)
            {
                Debug.Log("This molecule is " + index);
                return true;
            }
            else
            {
                Debug.Log("This molecule is not " + index);
                return false;
            }
        }

        private AtomType GetSmilesStartingSymbol(ref string smiles)
        {
            if(System.Char.IsUpper(smiles[0]))
            {
                    if (smiles.Length > 1 && System.Char.IsLower(smiles[1]))
                    {
                        // TODO: to be amended when extending the dictionary (adapt else block below)
                        Debug.LogError("The dictionary only contains molecules with C, O and H atoms.");
                        return AtomType.Void;
                    }
                    else
                    {
                        char symbol = smiles[0];
                        if (smiles.Length > 1 && smiles[1] == ']')
                        {
                            smiles = smiles.Substring(2);
                        }
                        else
                        {
                            smiles = smiles.Substring(1);
                        }

                        // TODO: to be amended when extending the dictionary
                        switch (symbol)
                        {
                            case 'C':
                                return AtomType.C;
                            case 'O':
                                return AtomType.O;
                            case 'H':
                                return AtomType.H;
                            default:
                                return AtomType.Void;
                        }
                    }
            }
            else if(smiles[0] == '[')
            {
                smiles = smiles.Substring(1);
                return GetSmilesStartingSymbol(ref smiles);
            }
            else
            {
                Debug.LogError("MoleculeIdentifier[GetSmilesStartingPoint] : Something went wrong with the smiles " + smiles);
                return AtomType.Void;
            }
        }

        private List<AtomBehavior> GetStartingPointsByType(GameObject[] molecule, AtomType symbol)
        {
            List<AtomBehavior> atomsFound = new List<AtomBehavior>();

            foreach(GameObject atom in molecule)
            {
                AtomBehavior atomScript = atom.GetComponent<AtomBehavior>();
                if(atomScript.Symbol == symbol)
                {
                    atomsFound.Add(atomScript);
                }
            }

            Debug.Log("GetStartingPointsByType size : " + atomsFound.Count);

            return atomsFound;
        }

        private bool CompareBranchFrom(ref string smiles, AtomBehavior startingPoint)
        {
            Debug.Log("CompareBranchFrom current smiles : " + smiles);
            
            // First, we record the SMILES' last state to restore it if this branch does not match
            string smilesLastState = string.Copy(smiles);

            // We also keep track of the processed atoms in this branch to clear them at the end if it does not match
            List<AtomBehavior> atomsTrack = new List<AtomBehavior>();
            startingPoint.Mark();
            atomsTrack.Add(startingPoint);

            List<AtomBehavior> nonMarkedNeighboursOfInterest;
            AtomType nextSymbol = AtomType.Void;
            while (!smiles.Equals("") && smiles[0] != ')')
            {
                Debug.Log("In while loop : smiles is ." + smiles + ".");
                Debug.Log("CompareBranchFrom : the smiles is not empty, entering the while loop");
                // an opening parenthesis means a new branch
                if (smiles[0] == '(')
                {
                    smiles = smiles.Substring(1);
                    // the method is called recursively for each new branch
                    if (!CompareBranchFrom(ref smiles, startingPoint))
                    {
                        smiles = string.Copy(smilesLastState);
                        ResetAllMarks(atomsTrack);
                        return false;
                    }
                }
                else
                {
                    if (smiles[0] == '=')
                    {
                        smiles = smiles.Substring(1);
                        nextSymbol = GetSmilesStartingSymbol(ref smiles);
                        nonMarkedNeighboursOfInterest = startingPoint.GetNonMarkedNeighboursByBondType(2);
                    }
                    else if (smiles[0] == '#')
                    {
                        smiles = smiles.Substring(1);
                        nextSymbol = GetSmilesStartingSymbol(ref smiles);
                        nonMarkedNeighboursOfInterest = startingPoint.GetNonMarkedNeighboursByBondType(3);
                    }
                    else
                    {
                        nextSymbol = GetSmilesStartingSymbol(ref smiles);
                        nonMarkedNeighboursOfInterest = startingPoint.GetNonMarkedNeighboursOfInterest();
                    }

                        Debug.Log("In while loop : nb of NoI is " + nonMarkedNeighboursOfInterest.Count);
                        if (nonMarkedNeighboursOfInterest.Count == 1 && nonMarkedNeighboursOfInterest[0].Symbol == nextSymbol)
                        {
                            startingPoint = nonMarkedNeighboursOfInterest[0];
                            startingPoint.Mark();
                            atomsTrack.Add(startingPoint);
                        }
                        else
                        {
                            smiles = string.Copy(smilesLastState);
                            ResetAllMarks(atomsTrack);
                            return false;
                        }
                }
            }

            // if we are at the end of the branch and there are more neighbours of interest to be processed (i.e. unmarked)...
            nonMarkedNeighboursOfInterest = startingPoint.GetNonMarkedNeighboursOfInterest();
            Debug.Log("CompareBranchFrom non marked NoI size : " + nonMarkedNeighboursOfInterest.Count);
            if (nonMarkedNeighboursOfInterest.Count > 0)
            {
                Debug.Log("CompareBranchFrom : there are non marked NoI");
                // ...the SMILES does not match the current branch
                smiles = string.Copy(smilesLastState);
                ResetAllMarks(atomsTrack);
                foreach(AtomBehavior atom in atomsTrack)
                {
                    Debug.Log("Atom marked : " + atom.isMarked());
                }
                return false;
            }
            if (!smiles.Equals(""))
            {
                smiles = smiles.Substring(1);
            }
            // else, the SMILES does match the current branch
            return true;
        }


        private void ResetAllMarks(GameObject[] atoms)
        {
            foreach (GameObject atom in atoms)
            {
                atom.GetComponent<AtomBehavior>().resetMark();
            }
        }

        private void ResetAllMarks(List<AtomBehavior> atoms)
        {
            foreach (AtomBehavior atom in atoms)
            {
                atom.resetMark();
            }
        }
    }
}
