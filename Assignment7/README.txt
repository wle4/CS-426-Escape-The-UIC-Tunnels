The major changes we made were adding a Main Menu, UI elements such as an interact prompt for grabbing boxes/entering doors, several different sounds, and an interactable door that brings you to Level 2.
Screenshots of some of the major changes we made will be in our design document.

Assignment 7
UI Design:
    - Lighting on Level 1 and 2 -> Played around with environment lighting to fix most lighting issues.
    - HP bar and Flashlight battery bar not showing up -> Used Rect Transform to position them properly.
    - No interact prompt is shown when facing an interactable item -> Added a tag for these objects and telling the player script to prompt them with what to do with them (i.e., Door = "Exit [E]", "Grab [E]", etc.).
    - Main menu background was failing to cover full screen. -> Rect Transform to stretch background.
    - Main menu buttons weren't working. -> Added a scene manager script and UI manager script to piece them together.
    - HUD display for inventory not working. -> Added a HUD manager to take care of inventory and interactable prompts.
    - Transitions between levels are choppy. -> Used UI manager to make transitions
    - Camera not shaking when bridge falls. -> Added a Camera shake script to be reusable for any other instances of this in the game.

Sound Design:
    1) We used Lethal Company as a game to review sound design. Some of what we heard were:
    - Walking on grass
    - Walking on metal
    - Birds chirping
    - Ship noises
    - Monster thumping
    - Monster roaring
    The sounds weren't too loud or too quiet, and changed volumes based on the player's location. Some noises overpowered others, but it made sense since something
    like a monster roaring would be more important to hear than background noises like grass.

    2) Some of the noises we used were:
    - Walking/Sprinting on cement
    - Door open sound
    - Door slamming sound
    - Bridge collapsing sound
    - Grabbing sound (for objects)
    - Throwing sound
    - Loud "AHH" sound
    - Scary background music for main menu
    We chose these specific sounds because it sets the tone of the game, which is meant to be a horror game. The player traverses through hallways and doors, so walking/sprinting
    sounds and door open/closing sounds are important for general flow of the game. Grabbing and throwing sounds are important to show that the player has interacted with something
    in the game properly. The other sounds we listed above are situational, such as the bridge collapsing sound only plays when the bridge collapses in Level 1, or the background
    music only plays in the main menu, all to make the player feel more immersed into the game.
    There were a couple of sounds we rejected, such as other types of background music or other throwing sounds, and that's just because we thought the sounds we chose fit into our
    game better.
