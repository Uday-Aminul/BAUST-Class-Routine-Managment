# BAUST Class Routine Management System

<img src="ClassRoutineManagement.jpg" alt="Class Routine Management" width="600" />

A production timetable system in active use by the CSE department at Bangladesh Army University of Science and Technology (BAUST). It replaces a fully manual routine-creation process with an automated, conflict-free scheduler.

---

## The Problem

Teachers at BAUST used to build the semester class routine by hand. With multiple teachers, lab rooms, sections, and 0.75-credit sessionals shared across alternating weeks, manual scheduling produced conflicts and took hours to fix.

## The Solution

Admins enter classrooms, lab rooms, teachers, level-term sections, and courses through a simple frontend. The backend generates a complete, conflict-free weekly routine automatically.

### Scheduling Algorithm

The core of the system is a **custom constraint-satisfaction algorithm** written from scratch. It resolves:

- **Teacher availability conflicts** — a teacher cannot be placed in two places at once.
- **Classroom and lab room clashes** — each room hosts one session per slot.
- **Alternating-week sessional logic** — 0.75-credit sessionals are placed across EVEN/ODD weeks so two lab groups share a single time slot.

It also includes a **slot-swap mechanism**: if a teacher is unavailable for a target slot, the engine exchanges an already-placed course with another to free the slot — without creating new conflicts.

---

## Built With

**Backend**
<img alt="C#" src="https://img.shields.io/badge/-C%23-68217A?style=flat-square&logo=csharp&logoColor=white" />
<img alt="ASP.NET Core" src="https://img.shields.io/badge/-ASP.NET_Core-512BD4?style=flat-square&logo=.net&logoColor=white" />
<img alt="Entity Framework Core" src="https://img.shields.io/badge/-Entity_Framework_Core-68217A?style=flat-square&logo=dotnet&logoColor=white" />
<img alt="MS SQL Server" src="https://img.shields.io/badge/-MS_SQL_Server-CC2927?style=flat-square&logo=microsoft-sql-server&logoColor=white" />

**Frontend**
<img alt="React" src="https://img.shields.io/badge/-React-61DAFB?style=flat-square&logo=react&logoColor=black" />

**Testing & CI/CD**
<img alt="xUnit" src="https://img.shields.io/badge/-xUnit-5C9FD3?style=flat-square&logo=xunit&logoColor=white" />
<img alt="Moq" src="https://img.shields.io/badge/-Moq-FF0000?style=flat-square&logo=moq&logoColor=white" />
<img alt="GitHub Actions" src="https://img.shields.io/badge/-GitHub_Actions-2088FF?style=flat-square&logo=github-actions&logoColor=white" />

**Tools**
<img alt="Docker" src="https://img.shields.io/badge/-Docker-2496ED?style=flat-square&logo=docker&logoColor=white" />
<img alt="Docker Compose" src="https://img.shields.io/badge/-Docker_Compose-2496ED?style=flat-square&logo=docker&logoColor=white" />
<img alt="Git" src="https://img.shields.io/badge/-Git-F05032?style=flat-square&logo=git&logoColor=white" />
<img alt="Swagger" src="https://img.shields.io/badge/-Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black" />
<img alt="Postman" src="https://img.shields.io/badge/-Postman-FF6C37?style=flat-square&logo=postman&logoColor=white" />
<img alt="VS Code" src="https://img.shields.io/badge/-VS_Code-007ACC?style=flat-square&logo=visual-studio-code&logoColor=white" />

---

## Getting Started

### Prerequisites

- [Docker](https://www.docker.com/) and Docker Compose
- (Optional, for local development without Docker) .NET SDK and Node.js

### Run with Docker Compose

```bash
git clone https://github.com/Uday-Aminul/BAUST-Class-Routine-Managment.git
cd BAUST-Class-Routine-Managment
docker compose up --build
```
