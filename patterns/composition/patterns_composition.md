---
marp: true
theme: dracula
---

# Patterns
# Teil 2 - Build Bridges, Not Trees

---
<!-- paginate: true -->

## Wozu?

* <green>Composition over Inheritance</green>
* Trennung in <pink>Abstraktion</pink> und <pink>Implementierung<pink>
* Erstellung von Plattform unabhängigen Componenten
* Erweiterungen ohne die jeweils andere Seite der Brücke anzufassen (<green>Open-Close</green>)
* Hight-Level Logik in <pink>Absstraktion</pink> und Detail Logik in <pink>Implementierung</pink> 
(<green>Single Responsibility</green>)

---

<center>

![bg w:800 drop-shadow hue-rotate:-60deg saturate:2.5](problem.png)

</center>

---


<center>

![bg w:800 drop-shadow hue-rotate:-60deg saturate:2.5](solution-en.png)

</center>

---

## Example: Backup SDK - Backup

* Zwei Plattformen: IOS und macOS
* Eine Funktion: Backup von Bildern

---

## Schön?

```swift
class IOSBackupService : BackupServiceProtocol {
    func generateBackup(...) -> Backup {
        let images = getImages()
        let imageMetadate = createImageMetadate(images)
        return createBackup(images, imageMetadate)
    }
}

class MacOSBackupService : BackupServiceProtocol {
    func generateBackup(...) -> Backup {
        let photos = getPhotosFromLibrary()
        let images = getImagesFromFolders()
        let imageMetadate = createImageMetadate(images + photos)
        return createBackup(images + photos, imageMetadate)
    }
}
```
---

## <red>Schön Blöd!</red>

```swift
protocol BackupServiceProtocol {
    func generateBackup(...) -> Backup
}
```

```swift
class BaseBackupService : BackupServiceProtocol {
    func createImageMetadate(images: [File]) -> [MetaData]
    func createBackup(
        images: [File], imageMetaData: [MetaData]
    ) -> Backup
}
```

```swift
class IOSBackupService : BaseBackupService {...}
class MacOSBackupService : BaseBackupService {...}
```

---

## <green>Schön</green>

```swift
protocol BackupServiceProtocol {
    func generateBackup(...) -> Backup
}
```

```swift
class BackupService : BackupServiceProtocol {
    private let _repository : RepositoryProtocol
    init(repository: RepositoryProtocol = IoC.Resolve()){...}

    func generateBackup(...) -> Backup {
        let images = _repository.getImages()
        let metadata = _metadataCreator.getMetadata(images)
        return createBackup(images, imageMetadate)
    }

    func createBackup(
        images: [File], imageMetaData: [MetaData]
    ) -> Backup {...}
}
```

---

## Was ist mit Videos?

* Weitere <pink>Implementierungen</pink>?
    ```swift 
    VideoRepository : RepositoryProtocol
    ```

* Weitere <pink>Abstraktionen</pink>?
    ```swift
    VideoBackupService : BackupServiceProtocol
    ```

---

## Nope, Strategy!

```swift
protocol FileLoadStrategyProtocol{
    func getFiles() -> [File]
}
```

```swift
class VideoLoadStrategy: FileLoadStrategyProtocol {
    func getFiles() -> [File]{
        // Algorithm to load Vieos
        return videos
    }
}

class ImageLoadStrategy: FileLoadStrategyProtocol {
    func getFiles() -> [File]{
        // Algorithm to load Images
        return images
    }
}
```

---

```swift
class FileLoader : FileLoaderProtocol {
    func load(using strategy: FileLoadStrategyProtocol) -> [File] {
        return strategy.getFiles();
    }
}
```

```swift
class IOSRepository: RepositoryProtocol {
    private let _fileLoader: FileLoaderProtocol

    func getImages() -> [File] {
        let imageStrategy = ImageLoadStrategy()
        return _fileLoader.load(using: imageStrategy)
    }

    func getVideos() -> [File] {
        // Alternative for better Tests
        let videoStrategy: VideoLoadStrategy = IoC.Resolve()
        return _fileLoader.load(using: videoStrategy)
    }
}
```

---


<center>

[https://github.com/limered/talks](https://github.com/limered/talks)

</center>

---

## Bildquellen

* Seite 3: https://refactoring.guru/design-patterns/bridge
* Seite 4: https://refactoring.guru/design-patterns/bridge