- Anpassen von Componenten mit unterschiedlichen Schnittstellen
- Erweitern von Componenten durch weitere Funktionalität ohne sie groß zu verändern


# Move

## Problem 3: Testen von Fremdcode ohne Interface, der innerhalb einer Methode instanziiert werden und mutiert muss um 3:47 wärend einer Vollmondnacht (Version 1.2)
## bzw: Das Gleiche++

---

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

<center>

[https://github.com/limered/talks](https://github.com/limered/talks)

</center>
