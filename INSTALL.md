# Installation Instructions

## Useful Links

The GitHub and Thunderstore links below install the Iceskull Temporary fork.

* [Latest Iceskull Release on GitHub](https://github.com/Iceskull7/ValheimPlus/releases/latest)
* [Iceskull-ValheimPlus_Temporary on Thunderstore](https://thunderstore.io/c/valheim/p/Iceskull/ValheimPlus_Temporary/)
* [Grantapher's upstream version on Nexus](https://www.nexusmods.com/valheim/mods/2323)

Keep only one ValheimPlus plugin installed. This fork uses the existing plugin GUID `org.bepinex.plugins.valheim_plus` and configuration file, so you can retain your settings when replacing another ValheimPlus version.

## Client - Automatic Installation

If you want to avoid the headache of manual installation at the cost of downloading a mod manager, I recommend [r2modman](https://valheim.thunderstore.io/package/ebkr/r2modman/), since it will automatically bundle the mod dependency for this mod.

1. Install [r2modman](https://valheim.thunderstore.io/package/ebkr/r2modman/).
2. Select Valheim and a profile. Disable or remove any other ValheimPlus package in that profile, keeping your configuration file.
3. Go to the "online" mods tab and search for `ValheimPlus_Temporary` by **Iceskull** (`Iceskull-ValheimPlus_Temporary`). Click "Download", select the latest version, start the game, [verify the title screen](#verify-the-installation), and then quit the game.
    * This will generate a configuration file for you.
4. Configure the mod options, see the [configuring](#configuration) section below.
5. Start the game again, this time for real!

## Manual Installation

### Important exceptions
If any of these sections don't apply to you, skip them.

#### Hosted Server
If your server host automatically installs ValheimPlus, select its manual or custom-mod installation option and install this fork's `WindowsServer.zip` or `UnixServer.zip`. Ask the host to prevent its automatic installer from replacing your chosen version.

The published fork archives use `ValheimPlus.dll`. Remove other ValheimPlus DLLs, including renamed copies, before installing it. Keep only one ValheimPlus plugin and use the same fork version on the server and clients.

#### Unix
If you are running the game on Unix, your installation comes with two extra files compared to its Windows counterparts. For details on the additional steps you need to take, search for the text "`Configuration (Linux`" on the [BepInExPack_Valheim page](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).

### General Instructions
1. Download the appropriate archive from [the latest Iceskull GitHub release](https://github.com/Iceskull7/ValheimPlus/releases/latest). Choose `WindowsClient.zip`, `WindowsServer.zip`, `UnixClient.zip`, or `UnixServer.zip` for a BepInEx bundle. If BepInExPack_Valheim `5.4.2360` is already installed, the plugin-only archive is `ValheimPlus-1.3.4.zip`.
2. Locate your game folder manually or start your Steam client and right-click the Valheim game. You can then right click the game in your library and select `Manage -> browse local files` for Steam to open your game folder.

    ![Browse Local Files Image](./resources/images/nav_steam_local_files.png)
3. Remove other ValheimPlus DLLs from `BepInEx/plugins`, retaining `BepInEx/config/org.bepinex.plugins.valheim_plus.cfg`. Copy the contents of the chosen ZIP into the game folder, overwriting the matching files.
4. Start the game, [verify the title screen](#verify-the-installation), then exit it.
    * This will generate a configuration file for you.
5. Configure the mod options, see the [configuring](#configuration) section below.
6. Start the game again, this time for real!

## Verify the Installation
The title screen and logs will *mostly* verify that the mod is working before you load into the game.

### Current Version Information

In the current mod release, you should have the following versions:
* Valheim version `1.0.21 (n-41)`
* BepInEx version `5.4.23.6`
  * `BepInExPack_Valheim` version `5.4.2360`, including UnityDoorstop `4.6.0`
    * This can be verified via logs only.
    * The four platform bundles on GitHub include this version; the plugin-only archive requires an existing BepInEx installation.
    * Thunderstore supports having this dependency and installing it for you via a mod manager.
* ValheimPlus version `1.3.4`
  * The title screen displays `ValheimPlus 1.3.4 (Iceskull Temporary)`.
  * The plugin, GitHub release, and `Iceskull-ValheimPlus_Temporary` Thunderstore package all use `1.3.4`.

For more info on compatibility of various V+ and Valheim versions, check out [COMPATIBILITY.md](COMPATIBILITY.md).

### Title Screen
The top left and bottom right corners of the title screen show BepInEx, Valheim, and ValheimPlus version information. Make sure your version numbers match the [current version information](#current-version-information) above. The older screenshots below illustrate where those labels appear.

Top Left:

![Top Left](resources/images/top-left.png)

Bottom right:

![Bottom right](resources/images/bottom-right.png)

### Logs

When running Valheim with `BepInExPack_Valheim` installed, a log should automatically be opened when you start the game. If not, it can be found at `%USERPROFILE%\AppData\LocalLow\IronGate\Valheim\Player.log` on Windows.

The following historical examples illustrate the log format. Use the [current version information](#current-version-information) above when checking your installation.

The BepInEx version is generally the first line of the `player.log`
```
[Message:   BepInEx] BepInEx 5.4.21.0 - valheim (3/28/2023 9:32:11 PM)
```

Followed by the BepInExPack_Valheim version if it is greater than version `5.4.2103`.

```
[Message:   BepInEx] User is running BepInExPack Valheim version 5.4.2105 from Thunderstore
```

After that, the ValheimPlus version can be found below it.
```
[Info   :   BepInEx] Loading [Valheim Plus 0.9.9.13]
```

```
[Info   :Valheim Plus] ValheimPlus [0.9.9.13] is up to date.
```

Finally, after ValheimPlus has loaded, at some point Valheim itself will log its version.
```
[Info   : Unity Log] 03/28/2023 23:14:38: Valheim version:0.215.2
```




## Configuration

With [Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager) installed, press F1 at the main menu and edit the settings there. Settings are read-only while a world is loaded, because most of them only take effect when the game starts.

Without it, locate the configuration file and edit it with a text editor. It is created the first time you run the game.

### File Location
| Installation Method | Location                                                                                                                                                                 |
|---------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Manual Installation | `<game folder>/BepInEx/config/org.bepinex.plugins.valheim_plus.cfg`                                                                                                                          |
| R2ModMan            | `%APPDATA%/r2modmanPlus-local/Valheim/profiles/<profile_name>/BepInEx/config/org.bepinex.plugins.valheim_plus.cfg` (replace `<profile_name>` with the name of your profile)                  |
| Thunderstore        | `%APPDATA%/Thunderstore Mod Manager/DataFolder/Valheim/profiles/<profile_name>/BepInEx/config/org.bepinex.plugins.valheim_plus.cfg` (replace `<profile_name>` with the name of your profile) |
