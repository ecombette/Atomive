# Atomive

VR Unity project (with HTC Vive) for modelling, assembling and manipulating atoms and molecules in the context of physics and chemistry teaching.

The system currently supports multiple bonds and branches, you only have to add entries to the dictionary if you want it to be more complete : add them in the Awake() method of MoleculeDictionary, following the [SMILES specification](https://en.wikipedia.org/wiki/Simplified_molecular-input_line-entry_system) for the recognition algorithm to work.

**N.B.** For now, our system cannot process SMILES containing cycles : it will simply not be recognized even if you create it correctly in the virtual environment. It is under development!

A project at UQAC, Québec (Canada), by Aurélien Durance, Ivan Alt and Elise Combette.
