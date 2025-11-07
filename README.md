# SCP-035
This plugin adds SCP-035 to the game.
*Requires Harmony0.dll*

As you can see, SCP-035 appears as a glowing red Tutorial role.
<img width="573" height="758" alt="image" src="https://github.com/user-attachments/assets/b04d80f7-34d2-4916-a98b-685a164351f9" />

The mask item appears as glowing red SCP-268. When it is used, the player turns into SCP-035.
<img width="420" height="355" alt="image" src="https://github.com/user-attachments/assets/4cf997b8-62c5-4889-a8f8-fd278a8689a7" />

Default config:
```
health: 300
divided_heal_amount: 3
divided_regeneration_amount: 3
scp035_item_picked_up_hint: <b><color=red>You picked up SCP-035.</color></b>
scp035_item_changed_hint: <b><color=red>You selected SCP-035.</color></b>
scp035_hint: <b><color=red>You are SCP-035</color></b>
scp035_hint_duration: 15
spawn_properties:
  static_position:
    x: 0
    y: 0
    z: 0
  spawn_rooms:
  - Hcz049
  - Hcz079
  - Hcz106
scp035_item_drop_chance: 50
```
Note: the static_position in spawn properties overrides spawn_rooms when it is set to anything other than 0.
