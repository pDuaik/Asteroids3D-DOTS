# Asteroids3D
Using DOTS to create an Asteroids game in 3D.  
The purpose of the project is to dive deep into DOTS,  
and find best practices for a Data-Oriented project.
I am also doing my best to avoid libraries, relying, in most cases,  
on mathematical responses to complex problems.

## Game Controls
- AWSD -> Move  
- SPACE -> Fire  
- LEFT SHIFT -> Acceleration
- LEFT CTRL -> Brakes **(horrible workaround that I will delete soon)**

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

## TODO List
- [x] New Collision System
- [x] New Shooting System
- [x] New Camera Movement System
- [x] Killed Game Data Manager
- [x] Change Missile Collision query to buffer
- [ ] Change Power-Up System
- [ ] Change Shield System
- [ ] Improve player movement
- [ ] Shatter Asteroid when hit by a missile.
- [ ] Hyperspace jump function was not implemented, I thought it was world wrapping.
- [ ] UFOs are missing.
- [ ] Asteroids are not broken apart in smaller ones when hit/shoot.
- [ ] Asteroids speed doesn't change with size (in original design smaller asteroids move faster than bigger ones).
- [ ] Asteroids don't re-spawn after you destroy all of them.
- [ ] Use EntityManager to batch Entities allocation.

