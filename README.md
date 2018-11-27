# BusSimulator.ScheduleTool

A new tool to use with bus simulator 18 to bring in the aspect of schedules.

## How to use it?

1. Import the matching stops for your game version and language (see /Importable in repository)
2. Create your lines
3. The rest time is like a break you may or may not take at a stop
4. The traveling time states how long you travel to THE NEXT stop
5. Select route with destination and click drive

- You may select a different stop to start from
- You may choose a custom start time for the in-game clock

## Key bindings

- Locking: This locks to the next stop and deactivates the automatic continueing at the stop
- Skip: If you just passed a stop or leave to early you may want to display the correct next stop on your system
- Pause: Pauses the counter

## Some notes

- The fields regarding service time, fast bus and bendy busses have no effect for now
- The major flag for the stops influences there font weight and the line running text

## Planned features

- One key press for letting disabled passengers board/unboard the bus
- Skip bus stop on detection of door states
- Multi language (English & german)
- Configurable position and snapped draggable
- Door state in overlay
- Automatic repositioning when stopping (be able to see mirros)
- Choose bus at start to make door detection more precise (Problem: Non-working doors)
- Stop display (for emulating the terminals displays with your own schedule)
- Better switching of BS16/BS18 databases

## To-Do

- Rework code-behind in main window and line management
- ~Move to different ioc container~