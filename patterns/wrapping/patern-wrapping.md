---
marp: true
theme: dracula
---

# Patterns
# Teil 1 - Wrapping

<center>

![width:500px drop-shadow](pic-fachbeitrag_geschenke-1000x600.jpg)

</center>

---
<!-- paginate: true -->

## Wrapping?

```cs
public class Wrapper
{
    private Original _original;

    public Wrapper(Original original)
    {
        _original = original;
    }

    public string ToString()
    {
        return _original.ToString();
    }
}
```

---

## Wozu?

- Kapseln von nicht testbaren Componenten
- Anpassen von Componenten mit unterschiedlichen Schnittstellen
- Erweitern von Componenten durch weitere Funktionalität ohne sie groß zu verändern
- <green>Einfacheres Testen</green>

---

## Kapseln - DateTime.Now

```cs
public async Task Execute(CancellationToken ct)
{
    ...
    await FinalizeBackup(ct);
    ...
}
```

```cs
private async Task FinalizeBackup(CancellationToken ct)
{
    ct.ThrowIfCancellationRequested();
    await _remoteBackupJson.SetFinishDate(DateTime.Now);
}
```
---

```cs
public async Task SetCurrentTimeStamp(){
    await _cut.Execute(new CancellationToken());

    _remoteBackupJsonMock
        .Verify(json => json.SetFinishDate(DateTime.Now), Times.Once())
}
```

# <red>Fail</red>

---

```cs
public interface IDateTime
{
    DateTime Now {get; }
}
```

```cs
public class DateTimeAdapter:IDateTime
{
    public DateTime Now => DateTime.Now;
}
```

---

```cs
public class BackupWorker : IBackupWorker
{
    private readonly IDateTime _dateTimeAdapter;

    public BackupWorker(IDateTime dateTimeAdapter)
    {
        _dateTimeAdapter = dateTimeAdapter;
    }
}
```

```cs
private async Task FinalizeBackup(CancellationToken ct)
{
    ct.ThrowIfCancellationRequested();
    await _remoteBackupJson.SetFinishDate(_dateTimeAdapter.Now);
}
```
---

```cs
public async Task SetCurrentTimeStamp(){
    var now = DateTime.Now;

    _dateTimeAdapterMock
        .Setup(adapter => adapter.Now)
        .Returns(now)

    await _cut.Execute(new CancellationToken());

    _remoteBackupJsonMock
        .Verify(json => json.SetFinishDate(now), Times.Once())
}
```
# <green>Pass</green>
---

## Anpassen - Adapter

```cs
public class RemoteCalendar
{
    private readonly IDateTime _dateTime;

    public void SetTomorrow()
    {
        _dateTime.Add(1, DateTimeModifier.Day);
        ...
    }
}
```
```cs
enum DateTimeModifier
{
    Day = 1,
    Week = 2,
    ...
}
```
---

### DateTime bietet die Methode nicht an

```cs
public struct DateTime{
    public DateTime AddDays(double value);
}
```
---

```cs
public interface IDateTime
{
    DateTime Now { get; }
    void Add(int i, DateTimeModifier resolution);
}
```

```cs
public class DateTimeAdapter : IDateTime
{
    ...
    void Add(int i, DateTimeModifier resolution)
    {
        switch(resolution)
        {
            case DateTimeModifier.Day:
                _original.AddDays(i);
                break;
        }
    }
}
```
---

```cs
public class RemoteCalendar
{
    private readonly IDateTime _dateTime;

    public RemoteCalendar(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public void SetTomorrow()
    {
        _dateTime.Add(1, DateTimeModifier.Day);
        ...
    }
}
```

---
## Decorator - Mocks

```cs
public class DecoratedDateTime: IDateTime
{
    public int LastI {get; private set; }
    public DateTimeModifier LastResolution {get; private set; }

    private IDateTime _original;

    public DecoratedDateTime(IDateTime original){...}

    public void Add(int i, DateTimeModifier resolution)
    {
        LastI = i;
        LastResolution = resolution;

        _original.Add(i, resolution);
    }
}
```
---

```cs
_decoratedDateTime = new DecoratedDateTime(DateTime.Now);

_cut = new RemoteCalendar(_decoratedDateTime);
```

```cs
public SetsTomorrowsDate(){
    _cut.SetTomorrow();

    Assert.AreEqual(1, _decoratedDateTime.LastI);
    Assert.AreEqual(Date.Day, _decoratedDateTime.LastResolution);
}
```

# <green>Pass</green>

---

## Geiler Scheiß aber wann benutze ich das?
### Wrapper / Adapter
* Meist an <orange>Applikationsgrenzen</orange>
* <orange>Schnittstellen</orange> zu Fremdcode / Lagacy Code
* Nicht passende Schnittstellen kompatibel machen

### Decorator
* Mocks
* Kombination von Effekten 
    ```cs
    class CompressionDecorator : DataSource {...}
    class EncryptionDecorator : CompressionDecorator {...}
    ```

---

## Nachteile
* Mehr Code
* Configuration wird komplexer
* Decorator-Chaining ist nicht übersichtlich und rigide -> möglichst flach halten

---

<center>

[https://github.com/limered/talks](https://github.com/limered/talks)

</center>
