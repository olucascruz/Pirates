# Pirates
 
# About game:
  A Top-Down Shooter about pirates 
  
  # Gameplay:
  The player will survive and destroy other ships until the game session ends
  
  # Project Organization:

 - Assets
 
 - - Art
 
 - - - Animations
 - - - - BulletAnimation
 - - - - ExplosionAnimation
 - - - - ShipChaser
 - - - - ShipPlayableAnimation
 - - - - ShipShooter
 
 - - - Sprites
 - - - - kenney_piratepack
 - - - - Spritesheet
 - - - - Tilesheet
 - - - - Basic
 - - - - TilePallete

 - - - Resourses
 - - - - Prefabs
 - - - - ScriptableObjects

 
 - - - _Scripts
 - - - - GameRules
 - - - - Scenario
 - - - - ScriptableObjects
 - - - - Ship
 - - - - States
 - - - - UI
 - - - - Utils
 - - - - Weapons

- - - Scenes
- - - Settings
- - - TextMesh Pro


# Game mechanics:
## Player
-Move forward
-Rotate
-Shoot
-Triple Shoot

## Enemies
"Shooter” type will shoot the player when they are close to him. <br>
“Chaser” type will chase the player and hit him with their own ship. They will explode when they hit the player.
