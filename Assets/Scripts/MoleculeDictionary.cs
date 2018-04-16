using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeDictionary : MonoBehaviour {

    public Dictionary<string, string> molecules;

    /*** TEST ***/
    public void Test()
    {
        foreach (KeyValuePair<string, string> pair in molecules)
        {
            // Test molécule
            Debug.Log(pair.Key + " : " + pair.Value);
        }
    }


	void Awake ()
    {
        molecules = new Dictionary<string, string>();

        // Eau
        molecules.Add("Eau", "O");

        // Dioxygène
        molecules.Add("Dioxygène", "O=O");

        // Dioxyde de carbone
        molecules.Add("Dioxyde de carbone", "O=C=O");

        // Méthane
        molecules.Add("Méthane", "C");

        // Éthane
        molecules.Add("Éthane", "CC");

        // Méthanol
        molecules.Add("Méthanol", "CO");

        // Éthanol
        molecules.Add("Éthanol", "CCO");

        /*** TEST ***/
        Test();
	}
}
