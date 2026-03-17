# MyPortfolio

A personal portfolio website built with **ASP.NET Core MVC** to showcase my projects, technical skills, and background as a software developer.  
The website includes a responsive user interface, a projects section connected to a database, project preview pages with screenshots, and a contact form for direct communication.

## Features

- Responsive portfolio website built with ASP.NET Core MVC
- Projects section with data stored in a database
- Individual project preview pages
- Multiple screenshots for each project
- Contact form for visitors to get in touch
- Clean and structured UI for presenting skills and work
- Server-side rendering with Razor views

## Tech Stack

- **C#**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Razor Views**
- **HTML**
- **CSS**
- **Bootstrap**
- **JavaScript**

## Project Status

This portfolio website is an active work in progress.  
The main structure, project presentation, and database-driven content are already implemented, while some features are still being improved for production readiness.

## Planned Improvements

- Add authentication and authorization for admin-only functionality
- Debug and fix contact form submission
- Improve validation and error handling on forms
- Enhance UI/UX and responsiveness
- Add easier content management for projects and uploaded documents


## Project Structure

```bash
MyPortfolio/
│
├── Controllers/        # Handles requests and application flow
├── Models/             # Data models and view models
├── Views/              # Razor views for the UI
├── Data/               # Database context and configuration
├── wwwroot/            # Static files (CSS, JS, images)
├── Migrations/         # Entity Framework Core migrations
└── Program.cs          # Application startup configuration
