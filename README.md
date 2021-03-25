# TemperatureMonitor
 Test project demonstrating a simple temperature simulation system. Objects with a HeatSource componeent contribute to the temperature of 
objects with a TemperatureMonitor component based on their relevant position and the ambient temperature of the scene. Heat source objects can be placed in the scene by left clicking on any surface 

Scripts included;


Temperature Control - static class that holds all active HeatSources in the scene and a function to be referenced by HeatMonitors which 
		      returns the temperature of an object ata  given world position based on the diatnce from heat sources and the ambient 
		      temperature.

Heat Source - component to generate heat in the scene, contains data for the temperature of the object and the ranges at which the temperature 
	      of objects will receive full, partial or no contribution

Temperature Monitor - checks the temperature Control class for its current temperature and sets it's surface colour accordingly

Heat Source Placement - a class made to allow heat sources to be placed by clicking on a surface with settings from UI objects in the scene