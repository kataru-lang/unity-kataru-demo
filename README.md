# Unity Kataru Demo

## About
Pretty big Unity starter project with Kataru submodule. Tested on 2021.1.9f1. 

Topdown 2D but I imagine it's pretty easy to convert it to another style.

Kataru yml files located in Assets/Kataru/Editor.

For syntax highlighting, download the [Kataru Extension](https://marketplace.visualstudio.com/items?itemName=Kataru.vscode-kataru).

Contains:
- Scene loading framework
- Dialogue UI framework
- Basic AudioMixer setup
- Camera stacking
- Basic player control
- Settings/rebinding controls
- A ton of Kataru commands
- Saving and loading via Kataru

As well as the following third party packages:
- Input System
- URP 2D Renderer
- Post Processing
- Cinemachine
- TextMeshPro
- NaughtyAttributes
- Hierarchy2
- Rotary Heart Serialized Dictionary Lite
- SuperUnityBuild
- DOTween
- Newtonsoft.Json-for-Unity

Cool misc features:
- Wiggly dialogue text
- Bubble stays on screen when character goes off screen
- Voice blips
- Customizable bubble depending on character 


## Implementation notes

Note that the Kataru.Constants class is used in collaboration with NaughtyAttributes to provide UI for selecting namespaces, characters, or passages, and is not necessary; if you choose to keep it then be sure to add to the classes in that file as you write your story.


Sometimes in a CommandHandler, a character is referred to as a 'reference.' This indicates that the character is mainly a character to deliver commands, not dialogue lines.

For more info on working with Unity and Kataru, see [the docs](http://kataru-lang.github.io/).
