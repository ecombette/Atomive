using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour {

    public int nbLiaisonMax;
    private List<Liaison> boundList;

    // Use this for initialization
    void Awake()
    {
        boundList = new List<Liaison>();
    }

    // Add a bound in our list
    public void Bound(Liaison l)
    {
        if (isBondable()) boundList.Add(l);
        else Debug.Log("Grosse pute, ya plus de place, check la prochaine fois que tu fais un tabernak");
    }

    // Remove a bound from our list
    public void UnBound(Liaison l)
    {
        if(boundList.Count > 0) boundList.Remove(l);
        else Debug.Log("Grosse pute, tu l'as déjà séché, check la prochaine fois que tu fais un tabernak");
    }

	// Return true if the Atom is bondable
    public bool isBondable()
    {
        return boundList.Count < nbLiaisonMax;
    }

    void OnDestroy()
    {
        foreach (Liaison l in boundList)
            Destroy(l);
    }


    #region MoleculeManager    
    private bool marked = false;

    public bool isMarked() { return marked; }
    public void Mark() { marked = true; }
    public void resetMark() { marked = false; }

    #endregion
}
