# TemperatureMonitor
Test project demonstrating a simple temperature simulation system. Objects with a heat source componeent contribute to the temperature of 
objects with a temperature monitor component based on their relevant position and the ambient temperature of the scene. Heat source objects 
can be placed in the scene by left clicking on any surface.

Scripts included;

Temperature Control - static class that holds all active heat sources in the scene and a function to be referenced by temperature monitors 
                      which returns the temperature of an object at a given world position based on the distance from heat sources and the 
                      ambient temperature.

Heat Source - parent class for components that generate heat in the scene, functions to be overridden in child classes to allow for
 	      different types of heat sources to be created and managed.

Radius Heat Source - child of heat source, objects within a set radius receive the full temperature contribution, beyond radius
		     contribution falls off by 1/x^2 as object approaces falloff radius beyond which contribution is 0.

Temperature Monitor - checks the temperature control class for its current temperature

Temperature Colour Change - checks temperature monitor component and changes material colour (hot = red, cold = blue) and UI elements

Heat Source Placement - a class made to allow heat sources to be placed by clicking on a surface with settings from UI objects in the scene