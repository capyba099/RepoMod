# Spin Teleport Mod for R.E.P.O.

BepInEx-мод для игры **R.E.P.O.**: ваша модель игрока быстро крутится и каждую секунду телепортируется к следующему игроку на карте.

## Поведение

- Модель локального игрока вращается со скоростью **1080°/с** (3 оборота в секунду).
- Каждую **1 секунду** персонаж телепортируется к следующему живому игроку в лобби.
- Работает только во время активного раунда (`GameDirector.gameState.Main`), не в меню и не когда вы мертвы.

## Установка

1. Установите [BepInEx Pack для R.E.P.O.](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/) через r2modman, Gale или вручную.
2. Скопируйте `RepoSpinTeleportMod.dll` в папку `BepInEx/plugins/`.
3. Запустите игру.

### Через r2modman / Gale

Соберите мод (`dotnet build`) и положите DLL в профиль модов, либо установите пакет с Thunderstore после публикации.

## Сборка из исходников

Требуется [.NET SDK 8+](https://dotnet.microsoft.com/download).

```bash
dotnet build -c Release
```

Готовый файл: `bin/Release/netstandard2.1/RepoSpinTeleportMod.dll`

Для автодеплоя в игру создайте `Directory.Repo.props` рядом с проектом:

```xml
<Project>
  <PropertyGroup>
    <GameDirectory>C:\Path\To\REPO\</GameDirectory>
    <ProfileName>Default</ProfileName>
    <BepInExDirectory>$(AppData)\r2modmanPlus-local\REPO\profiles\$(ProfileName)\BepInEx</BepInExDirectory>
  </PropertyGroup>
</Project>
```

## Зависимости

- BepInEx 5.x
- R.E.P.O. (Steam)

## Примечания

- Мод влияет только на **вашего** локального игрока.
- В одиночной игре телепорт не сработает, если на карте нет других игроков.
- В мультиплеере другие игроки могут видеть необычное перемещение вашего персонажа через сетевую синхронизацию Photon.
