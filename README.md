# Aottg2-Unity
[![Discord](https://img.shields.io/discord/681641241125060652.svg)](https://discord.gg/GhbNbvU)  

### About
Aottg2 is the sequel to the original Attack on Titan Tribute Game created by FengLee. This project is currently built in Unity 2023. For more details, join our discord server: https://discord.gg/GhbNbvU.

Contributing: join our discord server for more details on how to contribute. We accept applicants for a variety of work including programming, 3D modeling, texture and 2D art, sound effects, music, translation, and more.

### Required installs
1. Install git: https://git-scm.com/book/en/v2/Getting-Started-Installing-Git
2. Install git lfs: https://git-lfs.github.com/
3. Install Unity 2023.1.22f1 from Unity Hub

### Downloading the project
1. Open command prompt and [navigate](https://www.howtogeek.com/659411/how-to-change-directories-in-command-prompt-on-windows-10/) to your preferred installation folder
2. Enter `git clone https://github.com/AoTTG-2/Aottg2-Unity.git`
3. Open Unity Hub, click Project -> Add, and select your git folder

### Keeping project updated
1. Navigate your command prompt to the project folder (Aottg2-Unity folder)
2. Switch to your assigned branch by using `git checkout branch_name`
3. Enter `git pull` to update the project to the latest version

### Making and uploading changes
1. Navigate your command prompt to the project folder (Aottg2-Unity folder)
2. Switch to your assigned branch by using `git checkout branch_name`
3. Modify or add files to the Unity project
4. Make sure your project is updated to the latest version by using `git pull`
5. Enter `git status` to see which files have been changed, added, or removed by you
6. Add the files changes you wish to upload by entering `git add FILE`, or enter `git add .` to add all changes
7. Enter `git commit -m "Message"` to commit the changes, replace Message with your change description but include the quotation marks
8. Enter `git push` to finally upload the changes

### Building and testing
1. You can test the game by opening Scenes/Startup and using Unity play mode
2. You can build the game by clicking File -> Build Settings -> Build. Only build to the Aottg2-Unity/Aottg2 folder

> [!NOTE]
> If you need to test multiplayer changes, you can build a version of the game
> on the branch you want to test, and also run the game in the Unity play mode.
> Create a lobby and it'll show up only available to clients on that version.
> Connect both of them and you can test your changes.