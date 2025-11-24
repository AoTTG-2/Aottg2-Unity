# CL Debugger Quick Reference

## Setup (One Time)
1. Settings > UI > Enable "Enable CL Debugger"
2. Restart game
3. Done! Server now starts with game

## Debug Workflows

### ?? Quick Start (Recommended)
```
Game Launch ? Server Starts ? Connect Anytime!
```

**Steps:**
1. Launch game (server auto-starts)
2. Press F5 in VSCode
3. Server is already running - instant connection!
4. Load any game mode
5. Breakpoints work immediately

### ?? Debug From Init
```
Enable Wait ? Launch Game ? Connect ? Play
```

**Settings:**
- ? Enable CL Debugger
- ? Wait For Debugger Connection

**Steps:**
1. Start VSCode debugger (F5)
2. Load game mode
3. Game waits: "Waiting for debugger connection..."
4. Debugger connects
5. Init/OnGameStart breakpoints hit!

### ?? Hot Attach (During Gameplay)
```
Playing ? Issue Occurs ? F5 ? Debug!
```

**Settings:**
- ? Enable CL Debugger
- ? Wait For Debugger Connection

**Steps:**
1. Load game mode (runs normally)
2. Playing the game
3. See an issue
4. Press F5 in VSCode
5. Set breakpoints
6. Reproduce issue
7. Breakpoints hit!

### ?? Pre-Connect (From Menu)
```
Main Menu ? F5 ? Load Game ? Already Connected!
```

**Settings:**
- ? Enable CL Debugger
- ? Wait For Debugger Connection

**Steps:**
1. At main menu, press F5 in VSCode
2. Already connected!
3. Load game mode
4. All breakpoints work from start

## Connection States

| Scenario | Server Running | Connected | Init Debuggable |
|----------|---------------|-----------|-----------------|
| Just launched game | ? | ? | ? (no wait) |
| Launched + F5 in VSCode | ? | ? | ? (if wait enabled) |
| In game, not connected | ? | ? | Too late |
| In game, F5 pressed | ? | ? | OnTick/OnFrame work! |

## Common Use Cases

### "I want to debug Init()"
? Enable "Wait For Debugger Connection"

### "I want to debug during gameplay"
? Just press F5 anytime (wait not needed)

### "I want to always be ready to debug"
? Keep debugger enabled, F5 connects instantly

### "I'm getting an error in OnTick"
? F5 during gameplay, set breakpoint, wait for next tick

## Troubleshooting

### "Debugger won't connect"
- Check: Is "Enable CL Debugger" enabled?
- Check: Did you restart game after enabling?
- Check: Is port 4711 free?

### "Missing Init() breakpoints"
- Enable "Wait For Debugger Connection"
- Or connect BEFORE loading game mode

### "Game hangs on 'Waiting for debugger'"
- Press F5 in VSCode to connect
- Or wait for timeout (default 30s)
- Or disable "Wait" if you don't need it

## Best Practices

? **Development**: Keep debugger enabled
? **Testing**: Hot-attach when needed
? **Init debugging**: Enable wait temporarily
? **Runtime debugging**: Disable wait, attach anytime
? **Performance testing**: Disable debugger completely

## Console Messages

```
[CL Debugger] Started debug server on port 4711
[CL Debugger] Server started early - ready for connections before game start
```
? Server is running, can connect

```
[CL Debugger] Waiting for debugger connection (timeout: 30s)...
```
? Game is paused, press F5 in VSCode

```
[CL Debugger] Debugger connected!
```
? Ready to debug!

```
[CL Debugger] Connection wait timed out
```
? Timeout reached, game continues without debugger

## VSCode Launch Config

Make sure your `.vscode/launch.json` has:
```json
{
    "type": "customlogic",
    "request": "attach",
    "name": "Attach to Custom Logic",
    "host": "localhost",
    "port": 4711
}
```

Press F5 to use this configuration!
