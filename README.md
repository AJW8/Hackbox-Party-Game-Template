# Hackbox-Party-Game-Template
A template for creating online party games using ashbash1987's Hackbox package for Unity game engine: https://github.com/ashbash1987/hackbox-unity.

### Host
- Each game has a single host (the device from which the project is being accessed) whose screen must be publically displayed at all times.
- As the initial version of this project is just a template, it has no actual game functionality. You will need to implement this yourself.

### Player
- Once a game has been created, users can join as a player by entering the matching room code displayed on the host's screen.
- Players actively compete against each other to win either by ending up with the highest score (no score feature has been implemented) or by meeting some other condition.
- The player view is exclusive to the player and should not be shown to anyone else.
- During the game, the state of each player should update to allow the player to interact with the game.

### Audience
- If the maximum number of players have already joined or the game has started, any further users who try to join will be put in the audience.
- The audience generally does not compete but might be able to influence the outcome of the game in some way.
