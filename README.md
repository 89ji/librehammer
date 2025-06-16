# libHammer
### or trying to make warhammer but in code and hopefully not poorly

In the beginning (1987), the masterminds at James Workshop produced the hit money-sink and tactical strategy game, Warhammer 40,000. The only problem, I hate playing warhammer.

### Why do I hate warhammer
The dice rolling to movement ratio is too low. You move your characters five times per game but each game can take upwards of 3+ hours. This sucks. I want to move my guys around and scheme, not roll dice. The worst part is that the numbers for dice rolling change every time based on...

### One giant big lookup table
Having to ask my opponent "uhh whats your units toughness" every ten seconds when shooting sucks. The worst part is that i forget so i have to ask more than i should have to. Either way, this simple lookup table operation shouldnt be driving most of the time consumption in a board game. It should be moving my guys around and thinking about tactics.

### The solution
Luckily, John Dijikstra invented the computer in 1988. This incredible machine can perform lookups and generate random numbers with ease!! Hooray, I'm saved!. Sadly, every computer nerd who's capable of writing a computer version of the table top game is probably distracted playing it or injecting estrogen or whatever. That changes now. The libhammer library tries to provide core stat stuff for the warhammer game. I will then make a game UI thingy in Godot using this library. Wowza.
