# Factory Factory

Factory Factory is a chaotic, infinite, AI generated factory!

This project runs entirely in Unity. It is essentially a game that plays itself.

# Status

This project was not designed to be open-source. It was built quickly as an experiment to showcase the absurdity of ChatGPT generated products on a procedurally generated assembly line. It was originally designed to run 24/7 on Twitch before becoming the interactive live-stream it is today.

As a result there is quite a bit of technical debt.

# Setup

I have only tested on Windows 10. Other platforms may work but I would be cautious.

1. Install **Unity 2021.3.24f1**
1. Install **git LFS**
1. Clone this project and include submodules: `git clone --recursive git@github.com:megalon/factory-factory.git`
1. Init git lfs and download files: `git lfs install` then `git lfs checkout`
1. Remove the extra Newtonsoft dll that comes with Twitchmata in `Assets\Twitchmata\Plugins`
1. Open the project in Unity
1. Add your API keys as shown below
1. Open the "Main" scene. It can be found in `Assets\Scenes\`
1. Run the project

### API Keys
Create a new file called "Auth.cs" at `Assets\Scripts\Utils\Auth.cs`

```csharp
public static class Auth
{
    public readonly static string UberduckPubKey = "YOUR-UBERDUCK-PUBLIC-KEY";
    public readonly static string UberduckPrivKey = "YOUR-UBERDUCK-PRIVATE-KEY";
    public readonly static string OpenAIAPIKey = "YOUR-OPEN-AI-KEY";
    public readonly static string PolyPizzaAPIKey = "YOUR-POLY-PIZZA-KEY";   
    
    // Twitch? See the instructions below
}
```

### Twitchmata auth (optional)
If you want to enable the Twitch integration, you must set up the Twitchmata object. If you want to use the Bits, Subs, and other Affiliate only features of the Twitch API you will need to use a Twitch account that has affiliate.

The Twitchmata object is in the Main scene under `Management/TwitchmataManager`

Follow the [instructions to set up the Twitchmata object on the Twitchmata repo.](https://github.com/pilky/twitchmata/blob/main/Documentation~/Setup.md#2-setting-up-the-game-object)

There is a 2nd object called `DISABLE THIS TwitchmataManagerTest` which you can use to set up a 2nd test instance of Twitchmata for testing. You should only have one enabled at a time.

## Next steps

### Adjust the prompts

The prompts are stored as ScriptableObjects at `Assets\Scripts\OpenAI\PromptScriptableObjects\`

Click on one in the project, and edit the settings in the Inspector window.

# FAQ

## Do you think AI should replace writers and TV shows should be AI generated?

No, of course not. This project was designed to showcase the absurdity of AI, not replace TV shows made by real people.

## I get a ton of errors about Twitchmata when I try to launch the project

You probably didn't include submodules when cloning the project, so Twitchmata didn't download. Even if you aren't using the Twitch integration, Unity is still looking for the Twitchmata files in the project. Try `git submodule update --init --recursive`

## All of the textures, models, and other media files are only 1KB! What gives?

You did not download the git LFS files. Make sure you have git LFS installed and run the commands in the Setup section above.

## I want to make my own ChatGPT powered TV show. Can I fork you project and make my own?

I wouldn't recommend it. This project was not designed as a generic platform to build on.

I would recommend using this as a reference while making your own project.

# Credits
megalon - C# programming, Unity development, machine 3D models, machine animations

ivanmzart - Logo design and animation. Factory worker design, model, and animations

[Kenny Conveyor Kit](https://kenney.nl/assets/conveyor-kit), [Kenny Car Kit](https://kenney.nl/assets/car-kit)

### Special thanks
HyperVirtualExtreme, BobbieVR, StevenTheCat

### Libraries
[OpenAI-API-dotnet](https://github.com/OkGoDoIt/OpenAI-API-dotnet), [Twitchmata](https://github.com/pilky/twitchmata), [Uberduck.NET](https://github.com/EnzoBarizza/Uberduck.NET)

# License

### Code
MIT

### Assets
Original textures and 3D models are licensed under a [Creative Commons Attribution-NonCommercial 4.0 International License](https://creativecommons.org/licenses/by-nc/4.0/).

Contents of the [Kenny Conveyor Kit](https://kenney.nl/assets/conveyor-kit) and [Kenny Car Kit](https://kenney.nl/assets/car-kit) are released under a [CC0](https://creativecommons.org/choose/zero/) license.

Models downloaded though the PolyPizza API may have different licenses. Refer to the [Poly Pizza website](https://poly.pizza/) for more information.

# DISCLAIMER

ChatGPT, GPT-3, and all API's available by OpenAI are trained on data obtained on the internet. The thoughts, and opinions of the generated text and voice-over do not reflect the thoughts and opinions of the developers.
