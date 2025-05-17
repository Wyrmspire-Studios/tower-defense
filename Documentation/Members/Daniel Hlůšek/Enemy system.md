# Creating basic slime enemy
To begin in preparation for creating an enemy using PathFollow2D I've added a Path2D to the test map for testing enemies ability to follow a set path.

as for creating an enemy I've created a scene with a PathFollow2D node with a CharacterBody2D child, who then had Sprite and CollisionBody2D to give enemies a way to be hit and seen.

next up came the scripting. The Slime.cs script includes movement called every Delta, for every Delta the script also checks if the enemy is at the end of the line, if it is at the end the script removes players health and deletes the enemy from the map.
For damage a method "TakeDamage" was created, this method damages the enemy by amount stated in the argument, the method then checks if the enemies health is 0 or bellow, if it is then gold is added to the player and the enemy is removed from the map.

In the GameData.cs script a method and events were created to notify when an enemy has been killed.