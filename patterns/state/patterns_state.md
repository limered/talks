---
marp: true
theme: dracula
---

# Patterns
# Part 3 - State

---
<!-- paginate: true -->
## Wozu?
```cs
class Document 
{
    public string State { get; set; }

    public void Publish() 
    {
        switch (State) 
        {
        case "draft":
            State = "moderation";
            break;
        case "moderation":
            if (currentUser.role == "admin")
            {
                State = "published";
            }
            break;
        case "published":
            // Do Nothing
            break;
        }
    }
}
```

---

```cs
class Context {
    IState State;
    void changeState(IState state){}
    void doSomething(){ State.doSomething(); }
}
```
```cs
interface IState {
    void doSomething();
}

class ConcreteState : IState {
    Context _context;
    void doSomething(){ 
        ...
        _context.changeState(new OtherConcreteState(_context)); 
    }
}
```
---
## Example: Sync Client

```cs
public enum SyncState
{
    Idle,
    Running,
    Paused,
    Offline,
    Suspended,
    Disabled,
    InsufficientStorage
}
```

---

```cs
public interface ISyncStateContext : INotifyPropertyChanged
{
    SyncState State { get; }
    event OnSyncStateChanged SyncStateChanged;
    void GotoState(SyncState nextState);

    void Start();
    void Pause();
    void Cancel();
    void Suspend();
    void Continue();
    void Enable();
    void Disable();
    void RunAgain();
}
```

---
```cs
public interface IState
{
    ISyncStateContext Context { get; set; }
    void Enter();
    // private void Leave();

    void Start();
    void Cancel();
    void Pause();
    void Suspend();
    void Continue();
    void Enable();
    void Disable();
    void RunAgain();

    
}
```
---

## Spezielles

* Für State Changes wird ein Enum verwendet
    ```cs
    _context.GotoState(SyncState.Idle);
    ```
* Um den nächsten State zu konstruieren wird eine Factory auf basis von AutoFac verwendet
    ```cs
    private readonly IIndex<SyncState, IState> _states;

    public IState Create(SyncState state)
    {
        return _states[state];
    }
    ```
---

## Probleme (Wie ich sie verstehe :wink: )
### Asynchronität
* Grundsätzlich ist eine State Machine synchron, ein State nach dem Anderen
* UI und Datei Änderungen sind es nicht
* Normalerweise kein Problem, weil State-Änderungen abgewiesen werden, wenn sie nicht passen
    ```cs
    public class Paused : IState
    {
        public void Pause(){}

        public void Start() {
            _backgroundServiceManager.StartServices();
            _storageService.StartFileWatcher();
            Context.GotoState(SyncState.Running);
        }
    }
    ```
---

### Wo ist dann das Problem?
Zu lange Exit Transitionen + Mehrmalige Event Aufrufe
```cs
public class Suspended : IState
{
    public void Continue()
    {
        _taskFactory.Run(ContinueInternal);
    }
}
```
---

```cs
public async Task ContinueInternal()
{
    try
    {
        var tryCounter = 5;
        while (tryCounter > 0)
        {
            await _taskFactory.Delay(3000);
            if (_internetConnectionService.ReadTestWebsite(_webRequestAdapter.DefaultWebProxy))
            {
                LeaveToRunning();
                return;
            }

            tryCounter--;
        }

        LeaveToOffline();
    }
    catch (Exception e)
    {
        _logger.Error("<Suspended> -> <Continue> failed", e);
    }
}
```

---



---

<center>

[https://github.com/limered/talks](https://github.com/limered/talks)

</center>