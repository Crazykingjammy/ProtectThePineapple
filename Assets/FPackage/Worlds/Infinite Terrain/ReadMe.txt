Infinite Terrain - by Looney Lizard
===================================

Description
-----------

This is a simple script that can be used to create an "infinite" terrain
without the need to have huge, resource intensive terrains.

When linked to a terrain, the script will create a three by three grid from
copies of the terrain. It will then ensure that the player is always located
on the centre terrain (of the grid), moving Terrains to the opposite side of
the grid like a "treadmill" as the player moves.

I originally created this script for a boss battle that takes place in an
"endless cavern" where I needed to ensure that the player never reached the
edge of the arena.

Important Notes
---------------

1) The script will make no attempt to reshape terrains where they are stitched
   together. It is therefore essential that the selected terrain is "tileable",
   i.e. the opposite sides of the terrain should match up perfectly.

2) As the terrain grid "scrolls", terrains are physically moved and any objects
   located on the terrain will not move with it unless the objects are added as
   children of the terrain.

3) Only applicable if CloneTerrainChildren is TRUE:
   The script assumes that all copies of the original terrain will have a name
   ending with "(Clone)" (Unity default naming), so ensure that the name of the
   original terrain does not follow this pattern.

Usage
-----

1) Create a Terrain that can be tiled, i.e. the opposite sides of the Terrain
   must match up perfectly.

2) Link the InfiniteTerrain.cs script to the Terrain.

3) Set CloneTerrainChildren to indicate whether or not child objects of the
   Terrain will be copied when constructing the Terrain grid.

4) Link the GameObject that represents the player to the PlayerObject property.

That's it!

Version History
---------------

1.0 - Published!

Future Enhancements
-------------------

I would love to hear any ideas. Please leave comments on the Asset Store page.
