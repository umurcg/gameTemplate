# Unity Mobile Game Template

## Overview
This Unity Mobile Game Template is designed for rapid prototyping of hypercasual and casual mobile games. Developed with a focus on flexibility and ease of use, it's an ideal starting point for both beginners and experienced developers.

## Features
- **Generic Architecture**: Optimized for hypercasual and casual game genres.
- **Data Tracking**: Automatically tracks Level, Game Money, and Level Money.
- **Level Management**: Utilizes a LevelManager to load levels from scriptable objects as needed.
- **Game State Flow**: Supports states like 'Start', 'End with Loss', and 'End with Win', with transitions triggered through actions and events.
- **Action-Based Architecture**: Key actions are integrated to facilitate the addition of new functionalities. Actions include:
    - `OnGameStarted`
    - `OnGameEnded`
    - `OnNewLevelLoaded`
    - `OnGameLost`
    - `OnGameWin`
    - `OnGameRestarted`
    - `OnLevelChanged`
    - `OnGameMoneyChanged`
    - `OnLevelMoneyChanged`
- **Helper Classes & Editor Tools**: Offers various utilities for streamlined mobile game development.

## Installation
You can easily add this template to any Unity project using the Package Manager:
1. Go to **Unity Package Manager**.
2. Select **Add package by name**.
3. Enter the following URL: `https://github.com/umurcg/gameTemplate.git`

## How to Use
After installing the template, you can start integrating it into your Unity project. Here's a quick guide to get you started:
1. **Initialize the Template**: Set up the template in your Unity project.
2. **Customize Levels**: Use the LevelManager to add or modify levels.
3. **Implement Actions**: Customize game logic by attaching your own methods to the provided actions.
4. **Utilize Helper Classes**: Explore and use the helper classes and editor tools to enhance your game development process.
5. **Core Demo**: Examine the Core Demo sample as a practical example of how to use the template in a project.

## Contributing
Feedback and contributions are welcome. If you'd like to contribute or suggest improvements, please open an issue or submit a pull request on the GitHub repository.

## License
This project is licensed under the MIT License - see the LICENSE file for details.

## Contact
For any inquiries or support requests, please contact umurcg@gmail.com
