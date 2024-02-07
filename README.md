# GoDice Demo
This is a demo project on how you can communicate with GoDice. Use it as a reference or starting point for your own application.

Here you can find a tutorial video of how to build the project in Unity for Android:
https://youtu.be/SjlsfxbiFwc

TL;DR
- The project is fully workable and demonstrates some of features of GoDice, but you need to use a third party Bluetooth plugin
- Code is based on the GoDice companion app. Yet some parts are changed for demonstration sake and simplicity.
- You can also extract read-write protocols for bluetooth communication with GoDice. It contains all current events, but may be extended in future.
- Mapping for d20 and d24 shells also included. It's very easy to switch die's shell and parse die's output to a shell's value.
- Currently project is based on Unity 2021.2.4f1. If you open the project with a previous major version of Unity, there is a great chance that scripting define symbols will be missing and this will cause compilation errors. To fix it, add the following defines `USE_ODIN_MOCKUP;DICE_DEBUG;BLUETOOTH_DEBUG;BLUETOOTH_OPERATIONS_DEBUG`

# External dependencies

- Bluetooth plugin is not included in this repository due to legal issues. We strongly recommend using this plugin [Bluetooth LE for iOS, tvOS and Android](https://assetstore.unity.com/packages/tools/network/bluetooth-le-for-ios-tvos-and-android-26661). To add the Plugin to the project, do the following:
1. Make sure your current platform in Unity is either Android or iOS
2. Import the BLE plugin to the project.
3. Feel free to reorder plugin's content as you like, but make sure to create an Assembly Definition (BLE in our case) for these scripts:<img src="https://github.com/ParticulaCode/GoDiceUnityDemo/assets/50739566/9ef7dbbc-07d5-49b8-8819-44d9b0b6ab60" width="200" alignment="right">
4. Go to the folder `"Assets\Scripts\GoDice\App\Modules\Bluetooth"`, click the assembly refernece file `GoDice.App.Modules.Bluetooth` and the reference to `BLE` assembly definition (from the previous step) to the Assembly Definition Reference list (by dragging it to the end of the list), click Apply. Now everything supposed to compile.
5. Enter the "Project Setting" and add the `USE_BLE_PLUGIN` definition to the "Scripting Define Symbols" section. Click Apply.

If you want to use another plugin for bluetooth communication, you have to make your own implementation of `IBluetoothBridge` and disable `USE_BLE_PLUGIN` define.
- FrostLib is an in-house set of scripts.
- We encourage you to also add [Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041) to the project. It will provide you with a better inspector for simulated dice. And this is an awesome plugin in general. By default demo project has `OdinMockup` assembly to keep the code compiled, but you if you add Odin Inspector and disable `USE_ODIN_MOCKUP` 
- Whatever plugin you'll decide to use for Bluetooth - don't forget to reference its assembly definition in `GoDice.App.Modules.Bluetooth`

<img src="https://user-images.githubusercontent.com/50739566/143999517-9f664051-6967-4080-b336-4b05874585ef.png" width="500">

# Architecture
Project is based on an event-handler approach with a custom DI and ServiceLocator pattern. If you are looking for an entry point - look at `Bootstrapper` classes and `Bootstapper` game objects on the scene.` Demo.Bootstrapper` is an entry point to the project. Some code in the project is unused, but kept for demonstration sake. Some design decisions may seems redundant or overengineered, but keep in mind that all this code is transferred from GoDice companion project and part of much bigger and complex codebase.

# Modules
Project contains following modules:
- Demo module
- Bluetooth communication module
- Dice abstraction module 
- Dice simulation module for the editor
- Shared module
- Editor

Here is a quick look at all c# projects in the solution

<img src="https://user-images.githubusercontent.com/50739566/143999531-b00ce51e-0a1c-425c-838e-04e905a979a1.png" width="200">

## Demo module
Contains very basic UI classes, starts common services and initializes other modules. Demonstrates how to connect to multiple dice and exchange messages.

## Bluetooth module
Generally this is an abstraction and wrapper over bluetooth plugin. Module is responsible for:
- Providing implementation of `IBluetoothBridge`
- Device abstraction
- Scanning for GoDice devices
- Messaging via bluetooth

## Dice module
Module is responsible for:
- Dice abstraction over Bluetooth.IDevice
- Various dice related features. Like leds, shells, rotation to value parsing etc.
- Dispatching dice events with as little information as possible

## Simulation module
Module is responsible for:
- Hardware device substitution
- Editor based `IBluetoothBridge` implementation
- Presentation dice as game objects on scene with ability to simulate hardware’s events

Simulation module is not 100% correct to the firmware of GoDice. Yet they are reasonably similar and allow reliable playtest dice in editor.
In Editor simulated dice are always loaded by default in runtime. But you can have them instantiated on scene before entering play mode, if you want to have a persistent custom setup of dice.

## Shared

Contains classes that are supposed to be shared across different modules. Kept for compatibility sake with the GoDice companion application.

## Editor module
Contains useful commands and hotkeys to work with dice in editor.
