# Procedural-Generation
Procedural Terrain Generation
Generates Mesh Terrain using vertex displacement

Consists of 2 different generators which are accessed by attaching appropriate manager class to Empty GameObject.

TerrainManager
  Terrain generator Using Perlin Noise, Voronoi and other simple methods to create different terrain features.
  1. Set terrain Size and other options in TerrainManager inspector then press "Make default map" to generate empty mesh terrain.
  2. Now you can use Generate Map buttons to update mesh data to Unity and display it.
  3. Using Perlin Noise button you can generate Perlin noise with settings in perlin foldout and using Generate Map will visualize it.
  
Tectonics (in progress)
Terrain generator using tectonic plates movement.

Tectonics Manager drives all other parts of this projects and should be again attached to empty GameObject.
1. Use generate Map to create empty mesh.
2. Use Make PressureMap to generate Tectonics Plates.
3. Now you can use ColorMap to visualize different regions as different colors on Map (Using custom simple terrainShader).

General Idea of Tectonic Plate movement Generator.

1. Create Tectonics Plates on Map
2. each plate have own Movement direction, movement magnitude, elevation.
3. Calculate how plates interact with each other by measuring their change in their displacement to each other d.
  d<-0.25 : hills, Mountains.
  -0.25< d < 0.25 : flatlands, lakes .
  d>0.25 : lakes, canyons, beaches.
 4. Now calculate Moisture Map (In progress)
 5. assign default minimum moisture map to all tiles:
  -Grab ALL water at equator regions
	- Move along wind north and South, while dumping a bit overtime and increasing moisture of given regions.
	-Mountains create big dump zones with possible shadowing anything beyond (create plains bushlands beyond)
	-move until Poles where hurricanes are created and move towards beaches
 
 
