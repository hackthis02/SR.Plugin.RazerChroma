Razer Chroma Mod for Satellite Reign.

Added Razer Chroma support

Features:
Seleted/Combat status for each agent [1-4 keys]
Shortcut key for selected agent as well as active/cooldown status
Ability shortcut keys reflect any custom mapping by the user.


How to Build:


First you'll need to build Colore found [here](https://github.com/chroma-sdk/Colore)

You will need MonoDevelop (or Visual Studio) & Unity 5.0+ installed to compile this mod.

Open the solution file "Razer.Chroma.sln" in MonoDevelop
Make sure the Solution pane is showing. (View -> Pads -> Solution)

Expand the "Razer.Chroma" solution and "Razer.Chroma" project in the "Solution" pane.

Right click "References" -> "Edit References"

Click ".Net Assembly" tab

Click "Browse" and add references to the following assemblies.

[SR install folder]\SatelliteReignWindows_Data\Managed\UnityEngine.dll
[SR install folder]\SatelliteReignWindows_Data\Managed\Assembly-CSharp.dll
[SR install folder]\SatelliteReignWindows_Data\Managed\Assembly-CSharp-firstpass.dll
[Colore project root]\src\Corale.Colore\bin\[Debug or Release]\Corale.Colore.dll
Click "OK"

Select menu "Build" -> "Rebuild All". 
Depending on selected configuration, "Razer.Chroma.dll" will be written out to either 

[ProjectRoot]Razer.Chroma\bin\Debug\Razer.Chroma.dll or 
[ProjectRoot]Razer.Chroma\bin\Release\Razer.Chroma.dll 

Copy Razer.Chroma.dll and Corale.Colore.dll to 
[SR install folder]\Mods

Start Satellite Reign

Once you start a game you should see the keyboard turn green.
