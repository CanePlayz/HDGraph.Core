# Changelog

## Version 2.0 - Relaunch as

...

## Version 1.5.1

New:

- .NET Framework v4.5 now required to start HDGraph (it is installed by default on Windows 8+). If you're still on Windows 7, you can either continue to use HDGraph v1.4.2 or install .NET Framework 4.5.x.

Fixed:

- Right click on a drive from the Windows explorer didn't open the right folder
- Fix exports on Windows 8

## Version 1.4.2

Added:

- New "ignore reparse points" option to avoid duplicates due to symbolic links, junction points, or volume mount points
- Excluded folders appear in the report list (advanced tab)

Fixed:

- Bug 3442099: Crash when running on Windows 8 x64 (thanks to AdamNCasey)
- Minor i18n bug

## Version 1.4.1

Fixed:

- Bug 3153690: Error while scanning path with a "," or ";"
- Fixed UI for empty folder

## Version 1.4.0

Added:

- Completely new draw engine (WPF). Faster and more powerful. Defined as a plugin, which requires .NET Framework v3.5 or higher. Original draw engine ("basic engine") is still available.
- WPF draw engine: Zoom feature
- WPF draw engine: Full fluid rotation, using slider or mouse directly
- WPF draw engine: auto hide too small folders, in order to speed up the build of the graph
- WPF draw engine: New print feature (in file menu)
- New native scan engine on Windows: faster and more accurate (up to 6 times faster)
- Drag & drop capability (from Windows Explorer)
- Auto check (after startup) if a new version is available. Check can be done manually via the Help menu. Auto check can be disabled in the Options menu.
- Partially compatible with Linux and Mac (basic draw engine only), thanks to [Mono](http://www.mono-project.com)

Improved:

- Refactored design, mostly with the WPF draw engine

Fixed:

- Bug on text density setting: value loaded from a previous saved state was wrong
- Wrong message when folder deletion failed
- Some minor bugs

## Version 1.3.0

Added:

- New warning messages when scan errors occur
- New image rotation capability (you can rotate the graph as you want)
- New text density setting (you can change the text density in real time)
- New tooltip available when the mouse is hovered a directory
- Count the number of files in a directory (see the "directory details" form)
- New command line argument to set output image size

Improved:

- Better resize mode (preview during resize, real redraw when resize finished)
- Asynchronous draw for complicated graphs (which take a long time to draw) to avoid application freeze
- Main window state is memorized on exit (or the size if the window is not maximized)
- Performance improvements

Fixed:

- Bug 2505755 (Wrong Image saved when run from CLI)
- Some minor bugs

## Version 1.2.0

Added:

- Root folder can include disk free spaces
- Root folder always contains an "unknown files" parts, which represents the files that can not be scanned (due to acces restrictions for exemple) and lost space
- Multiple color styles (random or linear colors)
- New Details window, including the full list of current subfolders
- Command line support (see help for details) for scripting use
- New toolbar containing analyse shortcuts with computer's drives
- New wait form when you delete a folder
- Cursor appearance change on hovered folders

Improved:

- Graph export available in PNG format
- Full support of Windows Vista (UAC support for explorer integration)
- Main window size is memorized on exit
- Scan button is now the default action (equivalent to Enter key)

Fixed:

- Some minor bugs

Other:

- Licence replacement: HDGraph is now shipped under the CeCILL v2 licence.

## Version 1.0.1

Added:

- Manual directory refresh (on right click)
- Open log file menu item
- Delete folder menu item
- Icons in right click menu

Improved:

- Installer automatically removes previous version

Fixed:

- Tab order (use the tab key to navigate from a control to another)
- Minor translation mistakes
- Bug on directory scan if a filename introduce a too long file path (these files are now ignored)
- Other minor bugs

## Version 0.9.3

Improved:

- Documentation
- AboutBox

Fixed:

- Load from Explorer: directory name displayed in 8.3 format (#1579159)
- Incorrect window minimum size (#1580362)
- Some other minor bugs

## Version 0.9.2

Added:

- New Auto complete graph option: auto complete missing chart parts when you navigate in-depth directories
- First lines in documentation file (FR only; EN coming soon)
- Legend link (FR only; EN coming soon)

Fixed:

- Image bug in the about form in EN language (#1577157)
- Invalid controls locations after changing language if the main form has been resized before. (#1577159)

## Version 0.9.1

Added:

- Directory scan can be cancelled
- Short "Getting started" text shown at startup

Improved:

- Directory scan window is now modal (mother window not accessible until operation is finished)
- Changing language doesn't require the application to restart anymore
- Improved error management and logs
- Some minor improvements

Fixed:

- Bug opening HDGraph with a right click on a drive in the explorer

## Version 0.9.0

Initial public release
