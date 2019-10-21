---
marp: true
theme: default
---

# Lessons Learned - SWIFT
am Beispiel vom Backup SDK

... oder

```swift
struct Animal {
    let species: String
    init?(species: String) {
        if species.isEmpty { return nil }
        self.species = species
    }
}
```

---
<!-- paginate: true -->

# Überblick

1. Optionals


---

# Optionals

```swift
// normale Variable
var a = 1        // Werttyp (Int)
var b = NSDate() // Referenztyp (NSDate)
var c:Int        // Int, hat noch kein Wert
var d:NSDate     // NSDate, hat noch kein Wert
print(c)         // error: variable 'c' used before being initialized

// Optionals
var e:Int?       // enthält anfänglich nil
var f:NSDate?    // enthält anfänglich nil

// Implicitly Unwrapped Optionals
var g:Int!       // enthält anfänglich nil
var h:NSDate!    // enthält anfänglich nil

```
---

```swift
var e:Int?       // enthält anfänglich nil
e = 3
println(e + 1)    // nicht erlaubt!
println(e! + 1)

var g:Int!       // enthält anfänglich nil
g=3
println(g+1)     // OK
```

---

```swift
struct Animal {
    let species: String
    init?(species: String) {
        if species.isEmpty { return nil }
        self.species = species
    }
}

func checkAnimal () -> Bool {
    let animal = Animal(species:"")
    guard animal != nil else { return false }
    return true
}

```

---
 
### In Anwendung
```swift
class CalendarService: CalendarServiceProtocol{
    
    private let calendarRepository: CalendarRepositoryProtocol!
    private let fileIOService: FileIOServiceProtocol!
    
    init(calendarRepository: CalendarRepositoryProtocol? = IoC.resolveOrNil(),
         fileIOService: FileIOServiceProtocol? = IoC.resolveOrNil()) {

        self.calendarRepository = calendarRepository
        self.fileIOService = fileIOService
    }
}
```

---

### Besser

```swift
class CalendarService: CalendarServiceProtocol{
    
    private let calendarRepository: CalendarRepositoryProtocol
    private let fileIOService: FileIOServiceProtocol
    
    init(calendarRepository: CalendarRepositoryProtocol = IoC.resolveOrNil()!,
         fileIOService: FileIOServiceProtocol = IoC.resolveOrNil()!) {

        self.calendarRepository = calendarRepository
        self.fileIOService = fileIOService
    }
}
```

---

# Was damit?

- So früh, wie möglich loswerden mit guard
- Nur im Notfall nutzen
- Failable Initializer nie nutzen!
- Native Klassen, die es benutzen adaptieren

---

## @testable


---

# Weird
## Operator Precedence

```swift
let x = foo * bar as Baz
let y = foo && bar as Baz
```