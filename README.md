# Rimworld ContentFinder Optimizer

This is an experimental patch for the ContentFinder\<Texture2d>.Get() method in Rimworld.

## Why?

In my modlist (over 1000 mods), anything that calls ContentFinder\<Texture2d>.Get() repeatedly tends to use a lot of resources due to this method being fairly expensive when there are a lot of textures to search through. So I have introduced a cache to optimize these calls.

## Should I use this?

I don't expect many issues with using this mod, but I do not have a comprehensive understanding of the game's code so I will not make gurantees. In testing, I have found no issues.

## Installation

### Manual

Clone this repository into the `Mods` folder in your Rimworld install directory.

(Or you can click the green `Code` button above and press `Download Zip` and extract the zip to the folder instead.)

Default folder locations (Info taken from <https://rimworldwiki.com/wiki/Modding_Tutorials/Mod_Folder_Structure>):

1. Windows: `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods`
2. Mac: `~/Library/Application Support/Steam/steamapps/common/RimWorld/RimWorldMac.app/Mods`
3. Linux (Steam): `~/.steam/steam/steamapps/common/RimWorld/Mods`
4. Linux (GOG): `/home/<user>/GOG/Games/RimWorld/game/Mods/`

Note that these locations may be different depending on whether you used a Steam library on another drive, used GOG or changed the GOG install directory.

### Rimsort (Recommended!)

In Rimsort's top bar, select `Download -> Add Git Mod` and put the URL of this repository in the text prompt that comes up.

Once it downloads, press `Refresh` near the bottom to make the mod show in the left pane and drag it to the right pane.

You can update the mod by right clicking it and selected `Miscellaneous options -> Update mod with git`.

You may get a warning that this mod lacks a publish field ID. If this happens, select the mod and press add to ignore list.

## Settings

This mod has a configuration menu in-game. Go to `Options -> Mod settings` and search for this mod.

You can enable/disable the functionality of this mod and change how long the cache keeps items without restarting or reloading a save.

## Does it work?

The test results below will show that this mod succeeds in its goal of improving FPS. Data was collecting using MSI Afterburner's benchmark feature.

The tests below were run with my modlist of over 1000 mods and over 54,000 textures added from the mods.

### Test 1: No patch, My mid-game save, Almost whole map in view and unpaused. 8 Pawns. 1 minute of data collection

```txt
05-01-2026, 01:05:57 RimWorldWin64.exe benchmark completed, 2375 frames rendered in 62.625 s
                     Average framerate  :   37.9 FPS
                     Minimum framerate  :   20.3 FPS
                     Maximum framerate  :   58.8 FPS
                     1% low framerate   :    8.2 FPS
                     0.1% low framerate :    6.6 FPS
```

### Test 2: Patch enabled, My mid-game save, Almost whole map in view and unpaused. 8 Pawns. 1 minute of data collection

```txt
05-01-2026, 01:04:28 RimWorldWin64.exe benchmark completed, 4316 frames rendered in 62.265 s
                     Average framerate  :   69.3 FPS
                     Minimum framerate  :   49.8 FPS
                     Maximum framerate  :   75.1 FPS
                     1% low framerate   :   28.9 FPS
                     0.1% low framerate :    8.0 FPS
```

This test shows a performance improvement of 80%!

### Conclusion

This patch will give significant improvement to heavy mod lists that have a huge amount of textures.

While I have not specifically tested this case, I expect that light mod lists will see little improvement.
