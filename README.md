# WWNL


## Game

The player is given a well-defined area consisting of square fields. They can build their own city in
this area and manage it as a mayor with wide powers. Overall, the city consists of different types of
zones (on which residents build automatically), service buildings to be built separately by the player,
and the roads connecting them. The player's goal is to develop a prosperous city where the citizens
are happy and the budget is balanced

## Additional Features
### Fire department
A fire can break out in buildings standing on zone fields. The chance of this should be lower for
residential and service zones, and higher for buildings in industrial zones. Fires can also break out in
service buildings, with the exception of the fire department.
The construction of a fire station as a service building has a dual role:

- It reduces the probability of fires in a given radius (because smaller fires are estinguished
quickly without signalling the player)
- It makes it possible to extinguish actual fires, regardless of whether they are within the
radius of the fire department. 

The player has the opportunity to send firefighters in a fire
truck from the nearest fire station to the scene, whose movement can be followed
throughout the playing field. A fire station has a single fire truck.
If a fire is not extinguished in time it will spread to neighboring buildings after a while.
If you wait any longer, the building will be destroyed.
### Pension 
Let's develop our city's pension system more thoroughly. All citizens should have an age (min. 18)
that increases annually and they retire at 65. After that, the retired citizen no longer works, but still
needs a place to live. He does not pay taxes. He receives a pension instead, which should be half of
the average annual tax paid in the 20 years before his retirement.
We should also calculate (with necessary modifications) a level of satisfaction for retired citizens.
Pensioners do not move out of the city even if they are very dissatisfied.
The age of new citizens coming to the city from outside must be between 18 and 60. The pensioner
dies with a probability that's increasing every year above the retirement age. Then a young, 18-year old citizen will automatically take his place (representing births within the city), but he will not
necessarily live in the same place.
### Forests
Forests can be planted on general fields, preferably near residential areas. Forests improve the
satisfaction of nearby residents who have a direct view of them, and also increase the desire to
move into such zones. Forests can be seen by citizens living at most 3 squares away from them if
there are no buildings between them. Forests should also reduce the negative effect of industrial
zones on residential zones if they are located between two such squares.
Forests grow for 10 years, then they reach their mature state. Accordingly, the bonus for forest fields
should increase continuously in the first 10 years after their planting. There is a one-time cost of
planting forests, and they also have to be looked after for 10 years afterwards (maintenance costs).
When starting a new game, there should already be forested areas on the playing field.
### Persistence
It should be possible to save and load the game, and to manage multiple saves.
### More advanced graphics
Rotatable 3-dimensional graphics.

## Camera controlling

- Use WASD to move your camera on the map
- Press left shift and move your mouse to rotate your camera
- For zooming, you need to press left shift and move the scrollweel on your mouse

## Other
Unity version 2021.3.22f1

## Developing Team
###### Atabek Mykyev 
###### Concalves Kalilo
###### Alibek Kalykov
###### Munkh-Aldar Munkhtenger