# Atomive

Atomive is a VR project developed with Unity for the HTC Vive, aiming at modelling, assembling and manipulating atoms and molecules in the context of physics and chemistry teaching. 
It currently consists of a sandbox, where you can create molecules and identify them using our molecule identification system.

The molecule identification system currently recognizes basic molecules, but also supports multiple bonds and non-linearity (i.e. forks in the molecular chain); if you want to complete the dictionary, simply add entries in the Awake() method of the MoleculeDictionary script, following the [SMILES specification](https://en.wikipedia.org/wiki/Simplified_molecular-input_line-entry_system) for the identification algorithm to work.

**N.B.** For now, our system cannot process SMILES containing cycles : it will simply not be recognized, even if you reproduce it correctly in the virtual environment. It is still under development!

*A project at UQAC (Québec), Canada, by Aurélien Durance, Ivan Alt and Elise Combette.*
