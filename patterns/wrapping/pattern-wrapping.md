---
marp: true
theme: dracula
---

# Patterns
# Wrapping

<center>

![width:500px drop-shadow](eminem-review.jpg)

</center>

---
## Refactorings used

1. Extract Interface
1. Introduce Instance Delegator (oder so ähnlich) <- Wrapping right here
1. Extract Abstract Factory
1. ...
1. Profit

---
<!-- paginate: true -->

## Problem 1: Testen von statischem Code

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
public async Task SetCurrentTimeStamp(){
    await _cut.Execute(new CancellationToken());

    _remoteBackupJsonMock
        .Verify(json => json.SetFinishDate(DateTime.Now), Times.Once())
}
```

# <red>```DateTime.Now```</red> ist statisch und immer neu

---

# Lösung: Wrap that shit!

<center>

![width:400px drop-shadow](duckttapeball.jpg)

</center>

---

## Wie?

```cs
public class SimpleWrapper : IOldClass
{
    public string WrappedStaticMethod()
    {
        return Original.StaticMethod()
    }
}
```

---

Extract Interface

```cs
public interface IDateTime
{
    DateTime Now {get; }
}
```

Delegate Method

```cs
public class DateTimeAdapter:IDateTime
{
    public DateTime Now => DateTime.Now;
}
```

---

Constructor Injection

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
Usage
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

## Problem 2: Das gleiche, nur mit <green>```new```</green>

---

```cs
public IInternetMessageFormatWriter CreateWriter()
{
    var stringBuilder = new StringBuilder();
    return new InternetMessageFormatWriter(
        stringBuilder, 
        _internetMessageFormatLexicalTokenProvider, 
        _internetMessageFormatLogicalLineFolder);
}
```
---

Extract Interface
```cs
public interface IStringBuilder
{
    void Append(string content);
}
```
Delegate Method
```cs
public class StringBuilderAdapter : IStringBuilder
{
    private readonly StringBuilder _stringBuilder;
    public StringBuilderAdapter()
    {
        _stringBuilder = new StringBuilder();
    }

    public void Append(string content)
    {
        _stringBuilder.Append(content);
    }
}
```

---

Use (from IoC)
```cs
public IInternetMessageFormatWriter CreateWriter()
{
    var stringBuilder = IoC.Resolve<IStringBuilder>();
    return new InternetMessageFormatWriter(
        stringBuilder, 
        _internetMessageFormatLexicalTokenProvider, 
        _internetMessageFormatLogicalLineFolder);
}
```

---

## Problem 3: Testen von Fremdcode ohne Interface, der innerhalb einer Methode instanziiert werden und mutiert muss um 3:47 wärend einer Vollmondnacht (Version 1.2)
## bzw: Das Gleiche, nur mit Factory

---



---

## Was macht das ding so geil!

* <green>Einfacheres Testen</green> durch Kapselung und Entkopplung
* Extrem Wirkungsvoll mit IoC
* Manchmal ist zusätzlich eine Factory nötig
  
---

## Nachteile
* Mehr Code
* Steigerung der Programkomplexität
* Widerspricht miener Faulheit

---

## Geiler Scheiß aber wann benutze ich das?
* Meist an <orange>Applikationsgrenzen</orange>
* <orange>Schnittstellen</orange> zu Fremdcode / Lagacy Code
* Aufrufe von <orange>statischen</orange> Methoden/Klassen

---

<center>

[https://github.com/limered/talks](https://github.com/limered/talks)

</center>
