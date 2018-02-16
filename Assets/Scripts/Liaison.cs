using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liaison : MonoBehaviour {

    private Atom atome1;
    private Atom atome2;

    // Should be called by the Controller to make a Bound between 2 atoms
    public void BoundAtoms(Atom a, Atom b)
    {
        atome1 = a;
        atome2 = b;

        atome1.Bound(this);
        atome2.Bound(this);
    }

    //Ensure to free lhe list of Bounds of each atom
    void OnDestroy()
    {
        if (atome1 != null) atome1.UnBound(this);
        if (atome2 != null) atome2.UnBound(this);
    }

    #region MoleculeManager    
    private bool marked = false;

    public bool isMarked() { return marked; }
    public void Mark() { marked = true; }
    public void resetMark() { marked = false; }

    #endregion
}
