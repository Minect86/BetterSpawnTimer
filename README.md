# BetterSpawnTimer
[![C# Language](https://img.shields.io/badge/Language-C%23-8A2BE2?style=flat&labelColor=6A1FBF&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/en-us/languages/csharp)
[![License: MIT](https://img.shields.io/badge/License-MIT-228B22?style=flat&labelColor=1C6B1C)](https://github.com/Minect86/BetterSpawnTimer/blob/master/LICENSE.txt)
[![Version](https://img.shields.io/badge/Version-1.0.1-1E90FF?style=flat&labelColor=1870CC)](https://github.com/Minect86/BetterSpawnTimer/releases/tag/1.0.1)
[![Game SCP:SL](https://img.shields.io/badge/Game-SCP:SL-FFA500?style=flat&labelColor=CC8400)](https://store.steampowered.com/app/700330/SCP_Secret_Laboratory/)
[![Framework Exiled](https://img.shields.io/badge/Framework-Exiled-FF0000?style=flat&labelColor=990000)](https://github.com/ExMod-Team/EXILED)

- **Exiled version:** `9.8.1`
- **SCP:SL version:** `14.1.3`

## Description / Описание

A simple plugin that shows the time until the nearest squad arrives.  
The color depends on the squad type:  
- **Mobile Task Force** - blue  
- **Chaos Insurgency** - green  

When the squad arrives, the time is replaced with the squad type text, for example, **Mobile Task Force** or **Chaos Insurgency**.  
During mini squads, the text is not shown, but you can set it yourself in the config.  

---

Простой плагин, показывающий время до прибытия ближайшего отряда.  
Цвет зависит от типа отряда:  
- **Mobile Task Force** - синий  
- **Chaos Insurgency** - зелёный  

Во время прибытия отряда вместо времени отображается текст с типом отряда, например, **Mobile Task Force** или **Chaos Insurgency**.  
Текст во время мини-отрядов не показывается, но вы можете задать его сами в конфиге.  

---

### Screenshot / Скриншот

![Plugin Screenshot](plugin.png)

---

### Default Config / Дефолтная конфигурация

```yaml
mtf: '<color=#6D9FF7>Mobile Task Force</color>'
ci: '<color=#608F38>Chaos Insurgency</color>'
mtf_time_color: '#6D9FF7'
ci_time_color: '#608F38'
mtf_mini: ' '
ci_mini: ' '
height: 33
