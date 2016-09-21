# TODO

## Task queue
- CircularBuffer
    - `git diff` modified and original versions
    - Write tests for new methods
    - Implement automatic capacity increase
- Refactor snake classes

## SUMMARY (TODO: remove this)
- **[CURRENT]** Tests
- Refactor
    - Properties: use read-only, in-place initialized properties
    - Clean up:
        - MeshUtils
        - See comment to MeshUtils.ConvertQuadToTriangles
    - Organize namespace (+ folder) hierarchy
    - Assign appropriate class access modifiers
    - Extract all non-nested interfaces and classes into separate files
    - Document all non-optional dependencies
    - Replace loops with queries where appropriate
    - Search for "TODO" comments
- Framerate-independent snake growth
- Skin gallery
- Correct spawn
- Level gallery

## Planning
- **[CURRENT]** Clean up docs
    - Разделить на требования, дизайн и расписание
- Gantt chart

## Development tools
- Setup and test automated backup
    - Синхронизировать c облаком не только Git, но и
    - Использовать проекцию в ZipFS (7z?)
    - Также синхронизировать билды и файлы, исключенные с пом. `.gitignore`
    - Удалённый воркер по раскидыванию бэкапов в разные источники
- Build automation
    - https://www.google.ru/search?q=unity+cache+server
        - http://forum.unity3d.com/threads/automating-builds-with-jenkins.319169/
        - http://answers.unity3d.com/questions/47183/continuous-integration-with-unity.html
- Cache server
    - https://unity3d.com/unity/team-license
- Version control
    - Split assets and code into Git submodules

## Code
- Snake
    - Body mesh
        - Mesh partitioning
        - Collision detection
    - Wavy walker decorator
    - Correct respawn

- Gameplay
    - Correct food spawn
    - Collisions with obstacles
    - Increase speed when eating (i.e. up to 14/240)

- Establish more modular, manageable workflow
- Algorithms visualization
- Separate development and production assets

## Art
- Skins
- Decorations
    - Spruce
    - Flowers
    - Mushrooms
- Logo
