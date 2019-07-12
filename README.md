# RIPA
**Raph's IPA is Pretty Awesome**, a custom frok of Eusth's Illusion Plugin Architecture

## How To Install

1. Download a release (https://github.com/raftario/RIPA/releases)
2. Extract the contents into the game folder
3. Drag & drop the game exe onto **IPA.exe**

## How To Uninstall

1. Drag & drop the game exe onto **IPA.exe** while holding <kbd>Alt</kbd>

## How To Develop

1. Create a new **Class Library** C# project (.NET 3.5)
2. Download a release and add **IllusionPlugin.dll** to your references
3. Implement `IPlugin` or `IEnhancedPlugin`
4. Build the project and copy the DLL into the Plugins folder of the game

## How To Keep The Game Patched

After each update, you will need to patch the game again. This is a design decision that difers from the original IPA.

## Arguments

`IPA.exe file-to-patch [arguments]` 

- `--launch`: Launch the game after patching
- `--revert`: Revert changes made by IPA (= unpatch the game)
- `--nowait`: Never keep the console open

Unconsumed arguments will be passed on to the game in case of `--launch`.
