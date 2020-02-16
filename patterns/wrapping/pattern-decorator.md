
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

### Decorator
* Mocks
* Kombination von Effekten 
    ```cs
    class CompressionDecorator : DataSource {...}
    class EncryptionDecorator : CompressionDecorator {...}
    ```


## Nachteile
* Decorator-Chaining ist nicht übersichtlich und rigide -> möglichst flach halten