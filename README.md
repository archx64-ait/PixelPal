# PixelPal

PixelPal is developed using Unity3D which supports building for multiple platforms.
For this HCI project, it's built for Windows to get native run-time performance.
The main character, avatar, is imported from ReadyPlayerMe.
Oculus Lip Sync technology is used to animate speaking motion of the character.
Other animation, idle, is imported from Adobe Mixamo. OpenAI is used for defining the avatar behaviour and generate responses to user input.
The responses are then sent to Elevenlabs to create audio clips which are spoken by the avatar.  

To run the application,
create two folders ```.openai``` and ```.elevenlabs``` in your ```C:\Users\UserName\``` .
Create auth.json in each folder.

The content in your JSON files should be:

```json
{
    "api_key": "your API key for OpenAI or Elevenlabs"
}
```

Clone the repository and switch to ```build-windows``` branch.
Execute the executable.  
