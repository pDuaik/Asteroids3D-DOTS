# tsb_unity_test_persis_duaik
TSB Test

## Game Controls:
AWSD -> Move  
SPACE -> Fire  
LEFT SHIFT -> Acceleration  
White Balls Area -> Shooting Power UP

## Github
Main Branch -> Game running and working.  
Secondary Branch -> Code Review and Clean-Up. Game not necessarily working.

## Asteroids 3D
The main objective is to bring the classic Asteroids game to a 3D perspective.  
Using DOTS, I tried to maintain the original movement of the starship;
something that made the game quite hard.

## Hyperspace Jump
There is a full system that keeps all rocks in a canvas area.  
It gives the idea o infinity space, even though the amount of Asteroids is finite.

## Collision System
I haven't used the Physics library, thus making my collision system.  
It relies upon the distance from objects,
simplifying all objects colliders to spheres.  
My goal was to use mathematics as much as possible.

## Asteroid
When you hit an Asteroid, it will decrease in size.  
Shooting it a second time will destroy the asteroid.

## Power-Up
The shield power-up will turn on automatically
while the double shooting power-up must be collected.  
A full circle of spheres highlight the correct place.
