# Contributing to OrderFlowBot

Thank you for your interest in contributing to the OrderFlowBot NinjaTrader AddOn! This document outlines the process and guidelines for contributing.

## Table of Contents

- [Setting Up Your Environment](#setting-up-your-environment)
- [Workflow for Contributing](#workflow-for-contributing)
- [Code Formatting](#code-formatting)
- [Submitting Changes](#submitting-changes)
- [Additional Notes](#additional-notes)

## Setting Up Your Environment

To contribute to this project, you will need:

- [NinjaTrader 8](https://ninjatrader.com/) (you'll need the lifetime license or the OrderFlow+ package to work with the volumetric data)
- [Visual Studio](https://visualstudio.microsoft.com/) (the Community version works great)
- C#6 and NinjaScript knowledge

Ensure that NinjaTrader's AddOns folder is configured correctly to include `OrderFlowBot`.

### Folder Structure

The project structure is unconventional. The `OrderFlowBot` code resides in the following folder: `/AddOns/OrderFlowBot`

## Workflow for Contributing

1. **Fork the repository.**
   - Start by creating your own fork of the project.

2. **Clone your fork locally.**
   - Move the OrderFlowBot folder into your NinjaTrader AddOns folder normally located at: `C:\Users\<username>\Documents\NinjaTrader 8\bin\Custom\AddOns`.

3. Open the project in Visual Studio.
   - Open the solution in Visual Studio.
   - Make your changes in the OrderFlowBot folder.
   - Make sure NinjaTrader Editor is open. Saving the files in Visual Studio will trigger NinjaTrader to compile the changes.

4. **Auto-format your code.**
   - Before submitting your changes, ensure that your code is formatted using Visual Studio’s built-in auto-formatting tools:
   - Right-click the file in the Solution Explorer.
   - Select Format Document or use the shortcut (Ctrl + K, Ctrl + D).
   - This can also be set to auto-format on save.
5. **Test your changes in NinjaTrader.**
  - Ensure your changes work correctly by running NinjaTrader with your updated AddOn.

6. **Remove and replace the folder in NinjaTrader.**
  - After formatting, delete the existing OrderFlowBot folder in the AddOns directory for the repository. This will help show the recent changes in the source control.
  - Copy your updated folder to the AddOns folder.

## Code Formatting
  - Use Visual Studio's auto-format feature before committing.
  - Ensure there are no errors in the code. NinjaTrader will not compile if there are errors.

## Submitting Changes
  1. Commit your changes.
    - Make sure your commit messages are clear and descriptive.
  2. Push to your fork.
  3. Create a Pull Request (PR).
    - Submit a PR to the main or a primary feature branch such as dev or version if it exists with a detailed description of your changes.
  4. Respond to any feedback.
    - Be ready to make necessary revisions based on feedback from the maintainers.

## Additional Notes
  - This project follows an event-driven architecture. Be mindful of this when making changes or adding new features.
  - Try to keep services and components modular.
  - Contributions to documentation are also welcome!

Thank you for contributing!
