---
marp: true
theme: dracula
---

# Fast C#
# Value Types

---

## Wat?

```csharp
public struct/class Point
{
    public int x;
    public int y;
}
```
## Notes

* Stack/Heap
* Memory for Class: Header, Pointer, Values

---

## List<Point>()

## Notes

* Allocation Layout
* Comparison

---

## From Class to Stuct

### Notes

* `Object.Equals` -> `ValueType.Equals`
* Slow, because Boxing (make a object on heap)
* Override Equals, so no boxing of this
* Overload Equals, so no boxung of input
* add IEquitable<Point> for correct list comparison
* override GetHashCode for dictonary etc

---

## Demo