# ❤️ Corazón del Saber

> Un videojuego 2D estilo Metroidvania desarrollado en Unity como proyecto del Técnico en Desarrollo de Videojuegos del SENA.

![Unity](https://img.shields.io/badge/Unity-6_LTS-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-Programming-purple?logo=csharp)
![Status](https://img.shields.io/badge/Status-In_Development-yellow)
![License](https://img.shields.io/badge/License-Educational-blue)

---

# 📖 Descripción

**Corazón del Saber** es un videojuego de exploración y plataformas en 2D inspirado en títulos como **Hollow Knight**.

El jugador controla a **Eira**, una joven exploradora que despierta en las antiguas ruinas de una civilización perdida después de un misterioso evento conocido como **La Desconexión**.

Durante su viaje deberá explorar el **Santuario del Agua**, derrotar criaturas corrompidas, obtener nuevas habilidades y descubrir los oscuros secretos de los antepasados a través de fragmentos de historia esparcidos por el mundo.

---

# 🎮 Características

- **Movimiento fluido en 2D:** Sistema de salto avanzado y Dash (Botas del Impulso).
- **Combate y Enemigos con IA:** Diversidad de enemigos (terrestres, voladores, Mini Boss) con sistema de retroceso (Knockback).
- **Exploración tipo Metroidvania:** Múltiples rutas, saltos de fe y salas de combate.
- **Sistema de Lore Dinámico:** Narrativa ambiental a través de *Ecos, Tablillas, Espejos y Terminales Antiguas del S.E.N.A.* impulsado por ScriptableObjects.
- **Interacciones de Entorno:** Bancos para restaurar vida, precipicios letales con teletransporte, y monumentos.
- **Sistema de Recompensas (Loot):** Probabilidad dinámica de caída de Orbes de Vida al derrotar enemigos.
- **Visuales y Cámara:** Cámara dinámica con Cinemachine, efectos de transición (Screen Fader) y Tilemaps de Unity.

---

# 🗺️ Zonas del juego

## 🌿 Planicie del Saber

Zona inicial donde el jugador aprende los controles básicos y conoce el mundo.

Incluye:
- Tutorial
- Entrenamiento
- Acceso al Santuario del Agua

## 💧 Santuario del Agua

Primera gran zona explorable del juego.

Características:
- Exploración vertical y saltos precisos.
- Puntos de Lore ocultos.
- Salas de combate intensas.
- Puntos de control (Bancos).
- Peligros ambientales (Abismos).
- Mini Boss y Reward Room.

---

# ⚔️ Enemigos

Actualmente el proyecto incluye:
- Enemigo terrestre (Patrullaje)
- Enemigo acuático / Volador
- Enemigo avanzado
- Mini Boss (Velumbra)

---

# 🧪 Objetos e Ítems

- ❤️ **Orbe de Vida:** Soltado por enemigos al morir (probabilidad configurable).
- 🏺 **Poción de Curación Rápida**
- 👢 **Botas del Impulso:** Desbloquea la habilidad de Dash.

---

# 🛠️ Tecnologías utilizadas

- Unity 6 LTS
- C# (Arquitectura orientada a objetos, Singleton, ScriptableObjects)
- Visual Studio 2022
- Git & GitHub
- Aseprite
- Cinemachine 3
- Unity Tilemap

---

# 📂 Estructura del proyecto (Clean Architecture)

El proyecto mantiene una estructura modular y organizada para facilitar el trabajo en equipo y la escalabilidad:

```
Assets
│
├── 📁 Animations
├── 📁 Art & Sprites
├── 📁 Audio
├── 📁 Materials
├── 📁 Prefabs
├── 📁 Scenes
├── 📁 Tilemaps
├── 📁 UI
└── 📁 Scripts
    ├── 📁 Player
    ├── 📁 Enemies
    ├── 📁 Environment (Bancos, Abismos, Pickups)
    ├── 📁 Lore (Gestor de historia y bases de datos)
    ├── 📁 Core (GameManager, Audio, Guardado)
    ├── 📁 UI_Menus
    └── 📁 Camera_Effects
```

---

# 🚀 Cómo ejecutar el proyecto
*Próximamente en Itch.io*

1. Clonar el repositorio:
```bash
git clone https://github.com/Camil0Dev/Corazon-del-Saber.git
```
2. Abrir el proyecto con **Unity 6 LTS**.
3. Abrir la escena principal.
4. Presionar **Play**.

---

# 🎯 Objetivos del proyecto

- Aplicar los conocimientos adquiridos durante el Técnico en Desarrollo de Videojuegos del SENA.
- Desarrollar un videojuego funcional utilizando Unity.
- Implementar mecánicas avanzadas de exploración, lore y combate.
- Fortalecer habilidades de programación en C# y limpieza de código.
- Aplicar metodologías ágiles (SCRUM).
- Gestionar versiones mediante Git y GitHub.

---

# 📸 Capturas

> Próximamente

---

# 📅 Estado del proyecto

🚧 En desarrollo (Fase de Pulido)

Actualmente se encuentra implementando:
- Diseño final de niveles y colocación de elementos de la historia.
- Balanceo de daño y drop rate de vida.
- Ajustes finales de animaciones y audio.

---

# 👨‍💻 Desarrollador

**Camilo Moreno**

Unity Developer | Gameplay Programmer

GitHub: https://github.com/Camil0Dev  
LinkedIn: [https://www.linkedin.com/in/camilo-moreno-dev/](https://www.linkedin.com/in/camilo-moreno-unity-developer/)

---

# 📄 Licencia

Proyecto desarrollado con fines académicos para el **Servicio Nacional de Aprendizaje (SENA)**.

No está autorizado su uso comercial sin el consentimiento del autor.
