# QuickSelect
A Unity Editor extension that allows you to quickly select objects in the hierarchy.

[日本語のREADMEはこちら](README.ja.md)

## Installation
1. Place `QuickSelectWindow.cs` in the `Editor` folder of your Unity project.
2. Open the QuickSelect window from **Tools > QuickSelect** in the Unity menu bar.

## Usage
1. Select the object you want to register (click it in the Hierarchy).
2. In the QuickSelect window, click the Register Selected Objects button. A button with the selected object's name will appear in the window.
3. Click this button to instantly select the corresponding object in the Hierarchy.
4. Even if you switch scenes, the registered objects will remain selectable when you return to the original scene.

## Note
By default, the selected objects are saved in`QuickSelect/SelectedGameObjects.json` at the root of your project.
If needed, you can change the save location or exclude it from version control (e.g., by adding it to `.gitignore`).
