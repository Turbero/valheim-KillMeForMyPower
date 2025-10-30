### CHANGELOG

## 2.0.1

* Fix for .cfg file (temporary) while solving sync-issues

## 2.0.0

I gave up on setting up the special key when killing boss mechanism. It worked for some people, for others not. But fear not!!!
New implementation done!
  * Now the names of the people that can use a boss power will be added to a list in the configuration file (server-synced ofc)
  * The list is automatically updated with the names of the people nearby, no need to send RPC messages or other s**t
  * For people that installs or updates the mod in an existing world, remember you must activate "activateMidPlayDetection" to allow players who already selected the boss power anytime before the mod installation to have the grant. I'll toggle it ON by default this time.

Other restriction mechanisms remain untouched and still operational: special items, vendors, minimum days and max skill levels

---

<details><summary><b>Old releases notes (click to expand)</b></summary>

## 1.3.10

* Fixed mod, sorry I broke it by accident. Now it works!
* "Now the boss kill should also be granted to players nearby as intended." --> tested and confirmed it works.

## 1.3.9

* Now the boss kill should also be granted to players nearby as intended.


* Added command "/bosses_permissions" to check which bosses the player has killed in the world.


* For worlds already started after the mod is installed, if players are already beyond the boss cap level, it will automatically be set at the max level allowed for it when using the skill next time.


## 1.3.8

* Added that kill counts also for players participating in the boss battle, not only for the player giving the last hit.
* This can be activated or deactivated in the config file (active by default)

## 1.3.7

* Hildir's map table also locked by the same mandatory monster to kill to talk to her

## 1.3.6

* Added fix for compatibility with other mods which adds new abilities such as NorseDemigod (thanks, bytorphoto)

## 1.3.5

* Added usage restrictions for MonstrumDeepAndNorth mod monsters from Therzie (https://thunderstore.io/c/valheim/p/Therzie/MonstrumDeepNorth/):
  * Gorr, Brutalis, StormHerald and Sythrak can no longer be used/activated before killing them
  * not necessary to install MonstrumDeepAndNorth to keep using this mod. This is just in case you use that other mod.

## 1.3.4

* Added red color to the skill level in the skills dialog if it reaches its limit and the player has to kill the next boss to continue to skill up.
* Added fix for compatibility with modded bosses to avoid exceptions in the log (thanks, Sunkalsna)

## 1.3.3

* Fixed exception when player tried to activate power without having learnt any before (thanks, bytorphoto)

## 1.3.2

* Fixed exception when defeating certain bosses with different names after being instantiated in battle (thanks, bytorphoto)

## 1.3.1

* Added new configuration option for server worlds that already started if this mod is installed mid-play. Please read carefully:
  * Players that have already SELECTED at the spawn point (not killed) some power boss will not have to repeat that fight after installing the mod.
  * Unfortunately the game only leaves player personal keys when you select the boss power.
  * I'll put this configuration FALSE by default under "1 - General" category, change manually if you need for your mid-play game.
  * it's all it can be done in this matter until the game code changes to better

## 1.3.0

* Added new feature to restrict leveling up more than the indicated level in the configuration before killing each boss. 
  * Examples: 
    * set up that before killing Eikthyr you can only level up skills until 20
    * set up that before killing The Elder you can only level up skills until 35
    * and so on. It's configurable for each existing boss
  * If any issues or not interested, just leave them with the default value 100 and restrictions won't be applied
* Added small message in tooltip of progression items (crypt key, wishbone and demister) to indicate in green or red if you can already use them
* Added a small lightning effect when trying to use one of those progression item when you should not yet
* Changed configuration names under category "3 - Days". 
* Recommended to regenerate the config file if you're going to customize the new options!

## 1.2.1

* Now the boss restrictions to access the NPC vendors can be removed in the configuration file using 'None' value
* Code refactor to unify bosses information

## 1.2.0

* Necessary boss to access NPC vendors can be configured in the config file now (easier to use with any other configuration manager mod this way). By default:
  * you have to kill Eikthyr for Haldor and Hildir
  * you have to kill The Elder for the Bog Witch
* Mandatory boss name to access NPC vendors is now mentioned in the yellow alert when talking to them (keep the wildcard in the message if you customize it)
* Removed hardcoded blocks for Thunderstone, Ymir meat and chicken egg items and moved to default configuration (customize/remove them if you want)
* Recompiled with latest game version
* Tip: Deleting old config file and let it generate again will be useful and cleaner but not mandatory

## 1.1.2

* Restricting crypt key, wishbone and demister items from being used or equipped until the corresponding boss is killed by the player, even if the item is traded by another player.

## 1.1.1

* Added options in .cfg to setup more items to allow buying in vendors or not after killing the indicated boss. Example: BarrelRings,Bonemass;BeltStrength,Elder

## 1.1.0

* Added local progression to players for NPC vendors. This is translated into the next:
  * Haldor: need to defeat Eikthyr before buying anything
    * Thunderstone: need to defeat The Elder before showing up in the vending list
    * Ymir meat: need to defeat The Elder before showing up in the vending list
    * Chicken egg: need to defeat Yagluth before showing up in the vending list
* NPC restriction additions:
  * Hildir: need to defeat Eikthyr before buying anything
  * Bog Witch: need to defeat The Elder before buying anything
* When it is not possible to buy anything to a vendor a yellow message will pop up on screen, meaning it is still locked.
* Can be disabled in the configuration file, active by default.

## 1.0.4

* Fixed serverSync issue
* Fixed minimum days calculation

## 1.0.3

* ServerSync integration
* Added configurable minimum necessary number of days for each boss to obtain power without killing him

## 1.0.2

* Small issue in version number

## 1.0.1

* Fixed issue when being poisoned for infinite time when learning bonemass or queen power without killing them

## 1.0.0

Initial version

</details>