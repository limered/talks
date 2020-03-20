---
marp: true
theme: dracula
---

# Patterns
# Part 3 - State

---
<!-- paginate: true -->

## State is everywhere 

* Routing / Navigation
* Interfaces
* Handy Buttons
* Games

Wichtig einen Weg zu haben, wie man States und Transitions <green>explizit</green> und <green>clean</green> implementiren kann.

---

## Explizit?

```js

function animate(elapsed){
    if( loadingState == 4 && !playerAdded ) { 
        updateWeapoSelectionInterface(); 
    }
    if( gameRunning && playerAdded ) {
        updatePlayers(elapsed);
        ...
        myPlayer = getPlayerById(myId);
        if(!myPlayer.isRespawning){
            updateCamera();
        }
    }
    if( gameRunning && gamePaused ){
        updatePauseInterface();
    }
    renderer.render( scene, camera );
    if( debug ){ running = false; }
}

```

---

## Clean?
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
## Idea
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

## Problem - Asynchronität
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

![bg 60% drop-shadow](moar.jpg)

---

![bg 60% drop-shadow](async.jpg)

---

## Example: LD Unity Framework

```cs
public interface IState<T>
{
    ReadOnlyCollection<Type> ValidNextStates { get; }
    void Enter(IStateContext<IState<T>, T> context);
    bool Exit();
}
```

```cs
public interface IStateContext<TState, T> where TState : IState<T>
{
    ReactiveProperty<TState> CurrentState { get; }
    ReactiveCommand<TState> BevoreStateChange { get; }
    ReactiveCommand<TState> AfterStateChange { get; }
    void Start(TState initialState);
    bool GoToState(TState state);
}
```

---

```cs
[NextValidStates(typeof(GameOver), typeof(Paused))]
public class Running : BaseState<Game>
{
    public override void Enter(StateContext<Game> context)
    {
        MessageBroker.Default.Receive<GameMsgEnd>()
            .Subscribe(end => context.GoToState(new GameOver()))
            .AddTo(this);

        MessageBroker.Default.Receive<GameMsgPause>()
            .Subscribe(pause => context.GoToState(new Paused()))
            .AddTo(this);
    }
}
```

---

```cs
public class StateContext<T> : IStateContext<BaseState<T>, T>{
    ...
    public bool GoToState(BaseState<T> state)
    {
        if (!CurrentState.Value.ValidNextStates.Contains(state.GetType()))
        {
            return false;
        }

        _bevoreStateChange.Execute(state);

        CurrentState.Value = state;
        CurrentState.Value.Enter(this);

        _afterStateChange.Execute(state);

        return true;
    }
    ...
}
```

---

```cs
public readonly StateContext<Game> GameStateContext = new StateContext<Game>();

private void Awake()
{
    IoC.RegisterSingleton(this);    

    GameStateContext.Start(new Loading());

    InstantiateSystems();
    Init();

    MessageBroker.Default.Publish(new GameMsgFinishedLoading());
}
```

---

## Usage

```cs
IoC.Game.GameStateContext.CurrentState
    .Where(state => state is Paused)
    .Subscribe(_ => SetPause());

void SetPause() 
{
    IoC.Game.UpdateAsObservable()
        .Where(_ => IoC.Game.GameStateContext.CurrentState.Value is Paused)
        .Subscribe(_ => CheckForContinueClick())
        .AddTo(IoC.Game.GameStateContext.CurrentState.Value);
}

void OnContinue()
{
    MessageBroker.Default.Publish(new GameMsgUnpause());
}
```

---

## Also, was ist denn jetzt so gut an den Dingern?

* Ausfolmulierung der Applikations-/Objektstates
* Neue States können eingeführt werden ohne alles aufzubohren (<green>Open/Close</green>)
* Jeder State kömmert sich genau nur um die Sachen, die für ihn zuständig sind (<green>Single Responsibility</green>)
* Transitionen werden klar
* Höhere <orange>Testbarkeit</orange> und schnellere Bugsuche

---

## Negatives

* Manchmal Overkill (wenige states / wenig transitionen)
* Vorsicht bei Asynchronität
* Viel Code / Erfordert Disziplin

---

## Aber, aber...wie stelle ich um?

1. Suche deinen <orange>Context</orange>!
1. Erstelle ein <orange>State Interface</orange> samt Methoden / Transitionen
1. Füge die <orange>Transitionen dem Context</orange> hinzu
1. <orange>Eine Klasse pro State</orange> erstellen und Teile den bestehenden Code auf die States auf
1. Identifiziere die Stellen deiner State Changes und ersetze sie durch 
    ```cs
    _context.GoToState(new State());
    // oder
    _context.MakeTransition();
    ```
1. <pink>Optional:</pink> Mache eine <orange>StateFactory</orange> und benutze ein <orange>Enum</orange> zur State Identifikation
1. Spawne <red>keine</red> multiplen StateMachines

---

<center>

[https://github.com/limered/talks](https://github.com/limered/talks)

</center>