using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour {

    public int nbLiaisonMax;
    private int nbLiaisonRestante;

    /**
     * Est appellé dés qu'une liaison est accrochée à l'atome
     */
    void Lier()
    {
        if(nbLiaisonRestante > 0)
        {
            nbLiaisonRestante--;
        }
        else
        {
            //Pas possible les amis
        }
    }


    // Use this for initialization
    void Start () {
        nbLiaisonRestante = nbLiaisonMax;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
