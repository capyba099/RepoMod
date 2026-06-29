# RepoMod — моды для R.E.P.O.

Репозиторий с модами для игры **R.E.P.O.** (BepInEx).

---

## Spin Teleport Mod

Модель вашего игрока **всегда** быстро крутится и **каждую секунду** телепортируется к следующему игроку на карте.

| | |
|---|---|
| Вращение | 1080°/с (3 оборота в секунду) |
| Телепорт | каждую 1 секунду к следующему игроку |
| Режим | работает всегда: меню, лобби, раунд, смерть |

### Быстрая установка

1. Установите [BepInEx Pack для R.E.P.O.](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/)
2. Скопируйте папку [`BepInEx/plugins/SpinTeleportMod`](BepInEx/plugins/SpinTeleportMod) в папку `BepInEx/plugins/` вашей игры  
   или только файл [`RepoSpinTeleportMod.dll`](BepInEx/plugins/SpinTeleportMod/RepoSpinTeleportMod.dll) в `BepInEx/plugins/`
3. Запустите игру

### Скачать готовый плагин

- **DLL:** [BepInEx/plugins/SpinTeleportMod/RepoSpinTeleportMod.dll](BepInEx/plugins/SpinTeleportMod/RepoSpinTeleportMod.dll)
- **Манифест:** [BepInEx/plugins/SpinTeleportMod/manifest.json](BepInEx/plugins/SpinTeleportMod/manifest.json)

### Сборка из исходников

Требуется [.NET SDK 8+](https://dotnet.microsoft.com/download).

```bash
dotnet build -c Release
```

Результат: `bin/Release/netstandard2.1/RepoSpinTeleportMod.dll`

### Зависимости

- BepInEx 5.x
- R.E.P.O. (Steam)

### Примечания

- Влияет только на **вашего** локального игрока.
- Телепорт срабатывает, если на сервере есть другие игроки.
- В мультиплеере другие участники могут видеть необычное перемещение через Photon.

---

## Исходники игры

В репозитории также есть `Assembly-CSharp.rar` — декомпилированные исходники R.E.P.O. для разработки модов.
