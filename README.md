Goal: hamster homebound should be a simple 2D platformer game with a title, next level, game over, level select screen, 3 levels The Levels should be referred to as Episodes

Episode 1: Breaking Bad - essentially a tutorial level that guides player through basic controls with a little speech block thing that provides exposition (text block object is shown in videos 79-80) and introduces them to the objects, obstacles, checkpoints and story (Scene > Testing2 in Unity)

Episode 2: Malcolm in the Middle - medium difficulty level to test some player skills (Scene > Testing in Unity) 

Episode 3: Arrested Development -  defeat big snake use golden keys to save Milton’s family, brothers Let and Burg (get it?) who look exactly like Milton but have a different secondary fur color, from cage 

Player character: Milton can move left/right with arrow keys, jump and double jump with the spacebar, and possibly animated. 

Enemy: snake that shimmies side to side, maybe a larger snake as the boss in ep 3

Collectibles/Pickup items: golden key for final cage unlock (in the tutorial this would be the gems; the amount of these you collect show up in the upper right hand corner of the screen as you collect them); pear for restoring a life or half life or whatever this tutorial has as its health system (tutorial version: cherries). IF WE HAVE TIME: stars to collect for the level score, red key for in level puzzle 

Obstacles: moving platform, spikes (can take straight from tutorial but pick a better pixel art because theirs don’t look like spikes)

Objects: boxes that can be climbed on to reach higher areas 

Things to note:
GitHub repo: https://github.com/blips-of-code/Hamster-Homebound
Game controls: left and right arrow keys to move left and right, spacebar to jump, double spacebar to double jump
Player, in our case, refers to MiltonPawsUpPixel
When following the animation tutorials: When they refer to the PlayerController script, don’t change PlayerController.cs, change SimplePlayerMovement instead
I put all the sprites for our game under the Art folder in Assets. The tutorial art is in different folders
You go into a specific level/scene by clicking the scenes folder. Main menu is the starting screen
When following the videos assigned to you, please add notes under the appropriate number below.
Before creating a new folder, make sure that a folder with that exact name doesn’t already exist
To make pixel art:
https://www.piskelapp.com/p/create/sprite/
If you find a pixellated image with a non-transparent bg that you’d like to add to the game, use this tool to remove the bg:
https://www.adobe.com/express/feature/image/remove-background
I update the repo using git bash; here are some commonly used commands for reference: https://dev.to/axlblaze/git-bash-commonly-used-commands-22f7

