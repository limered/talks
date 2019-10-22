---
marp: true
theme: default
---

# Lessons Learned - SWIFT
am Beispiel vom Backup SDK

---
<!-- paginate: true -->

# Backup was?

- SDK für die Hidrive App auf IOS und macOS
- Nur Backup und Restore Funktion
- App nicht von uns

---

# Optionals

```swift
// normale Variable
var a = 1        
var b = NSDate() 
var c:Int        // compile error
var d:NSDate     // compile error

print(c)         // error: variable 'c' used before being initialized

// Optionals
var e:Int?       // nil
var f:NSDate?    // nil

// Implicitly Unwrapped Optionals
var g:Int!       // nil
var h:NSDate!    // nil

```
---

```swift
var e:Int?
e = 3
print(e + 1)     // error!
print(e! + 1)

var g:Int!
g=3
print(g+1)       // OK

print(g!)        // error, if nil
```

---

![bg 60%](https://sweatpantsandcoffee.com/wp-content/uploads/2017/10/940x450-Ryan-Reynold-Reaction-GIFs.jpg)

---

```swift
class ViewController: UIViewController {
  @IBOutlet weak var textView: UITextView!  // not nil shortly before View init
  ...
}
```

---
 
## Example: IoC
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

## Besser

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

## Guarding

```swift
struct Animal {
    let species: String
    init?(species: String) {
        if species.isEmpty { return nil }
        self.species = species
    }
}

func isAnimalAlive (_ animal: Animal?) -> Bool {
    guard let notNilAnimal = animal else { return false }
    print(notNilAnimal)
    return true
}

let anim = Animal(species:"")
print(isAnimalAlive(anim))

```

---

## Example: Guard

```swift
func merge(_ mediaMetaData: MediaMetaData?) -> MediaMetaData? {
    guard let mediaMetaData = mediaMetaData else {
        return MediaMetaData(items: [])
    }
    return MediaMetaData(items: mediaMetaData.items)
}
```

```swift
func merge(_ mediaMetaData: MediaMetaData?) -> MediaMetaData {
    if mediaMetaData == nil { 
        return MediaMetaData(items: [])
    } 
    return MediaMetaData(items: mediaMetaData!.items)
}
```
---

```swift
open class Version : ... {
    ...
    init?(_ string: String) {
        let regex = try! NSRegularExpression(pattern: "^\\s*(\\d+)\\.(\\d+)\\s*$", options: [])

        guard let match = regex.firstMatch(
            in: string, options:[], range: NSRange(location: 0, length: string.length)
        ) 
        else 
        {
            return nil
        }
        ...
    }
    ...

    static func deserialize(_ str: Any?) throws -> Version {
        guard let versionString = str.string else {throw JsonError.serializationFailed}
        guard let v = Version(versionString) else {throw JsonError.serializationFailed}
        return v
    }
}
```

---

# Example: Native Classes



---

# Was damit?

- So früh, wie möglich loswerden mit guard
- Nur im Notfall nutzen
- Native Klassen, die es benutzen adaptieren
- Failable Initializer nie nutzen!

---

## @testable


---

# Weird
## Operator Precedence

```swift
let x = foo * bar as Baz
let y = foo && bar as Baz
```

