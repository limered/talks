---
marp: true
theme: dracula
---

# Patterns - Wrapping

---

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

[https://github.com/limered/talks](https://github.com/limered/talks)