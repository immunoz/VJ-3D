# FIB's VJ project - Overcooked
This repository contains the code of a game based on the Team 17 cooking cooperative game, Overcooked (I and II).


## Statement
This project is based on developing a game with similar characteristics to 
Overcooked game. In this game, player has to control a cooker in order to 
prepare the maximum number of deliveries in a limited amount time, all of 
this while avoiding obstacles and dangers.

The resulting game has to count with the following requirements:
* Player has to move a cooker who can pick-up, chop, cook and put-in a plate 
  a series of ingredients.

* Deliveries have to be shown in the screen.

* A minimum of 5 levels can be played.

* A minimum of 7 recipes have to be available, which can be prepared with least 7 different ingredients.

* A chopper to chop ingredients is needed and also two more cooking tools as pots or pans.

* Any process like cooking or chopping requires a time to be completed. In the case 
  of cooking, if that time is exceeded, the game has to warn us and, if it is not picked-up,
  the food has to get burned and the kitchen is set on fire.
    
* An extinguisher is needed to extinguish any fire on the kitchen.

* Ingredients, cooking tools, plates and others can be put over the table

* SoundFX and music is required for the game.

* God mode Keys to test the game.

* Animations for each action in the game.

A more detailed version of the statement can be found in the files of this
repository (statement is in Catalan).


## Prerequisites
To test and modify the project first clone this repository. If you only want to
check the final result just open the binary in the X folder. This binary can be executed
in Windows OS with 64 bit architecture. Otherwise, if you want to modify the game, you will need the following tools:
* Unity version X
* Visual Studio Community (or other code editor)
* Blender 3D (if you want to modify the 3D models)

Just make sure to open the project directory with unity editor.

**Note:** if you test any playable scene in the game using Unity, you will notice
that an error is thrown in the console. This is because the GameManager need the 
SoundManager, just drop it in the hierarchy of the scene from the prefabs folder
and that should fix the error. By default the Sound manager is only loaded at the main menu
scene, therefore, all other scenes will throw the error.


## Built With

* [Unity](https://unity3d.com/es/get-unity/download) - Game engine used to develop this game
* [Visual Studio Community 2019](https://visualstudio.microsoft.com/es/vs/) - Used to edit and create the code
* [Blender](https://www.blender.org/) - Modeling 3D software used to create 3D models
* [Photoshop](https://www.adobe.com/es/products/photoshop/free-trial-download.html) - Photo editor used to create and edit sprites

## Authors & Contact
* **Isaac Marcelo Muñoz Cruz** - [Github](https://github.com/immunoz) - [LinkedIn](https://www.linkedin.com/in/isaac-marcelo-mu%C3%B1oz-cruz-409a811a2/)
* **Anas Infad** - [Github](https://github.com/ANASinfad) - [LinkedIn](https://www.linkedin.com/in/anas-infad-04526a1bb/)

Project Link: [https://github.com/immunoz/VJ-3D.git](https://github.com/immunoz/VJ-3D.git)

Feel free to use our code for any purpose, but always remember to attribute the authors 😉.  


## Acknowledgments
Special thanks to the [Brackeys](https://www.youtube.com/user/Brackeys) YT channel.
