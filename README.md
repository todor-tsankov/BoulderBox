## :eyeglasses: Project Introduction

**BoulderBox Website** is my defense project for **ASP.NET Core MVC** course at [SoftUni](https://softuni.bg/ "SoftUni") (September-December 2020).

It can be seen running on: [https://boulderbox.azurewebsites.net/](https://boulderbox.azurewebsites.net/).

## :pencil2: Overview

**BoulderBox** is a website for climbers. Each climber can explore the various countries, cities, gyms and sort/filter them in any way he likes.
In each gym there are boulders, which are short routes (2-5m) that are climbed without rope and there is a matress underneath for protection.
Climbers can add an ascent of a boulder and see their own ascents in their profile and also create their own boulder. Each climber participates in the ranking. 
There are 4 types of ranking: weekly, monthly, yearly and all time. For each climber the top 10 ascents are taken for each type. 
The points for one ascent come from the grade of the boulder, which gives the base points (harder boulder means more points). 
Bonus points are given for the style in which the ascent is done. 
In addition, there's a simple forum with categories, posts in the categories and comments for each post where climbers (or normal people) can
ask for advice, find a climbing buddy or suggest improvements or problems with the website.
The last form of communication is the chat, which is just for fun. Its messages are not stored anywhere so you don't have to worry about whether
something stupid you said 5 years ago is going to come up and embarass you.

## :hammer: Built With
- ASP.NET [CORE 5.0](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-5.0) MVC
- MSSQL Server
- Entity Framework [CORE 5.0](https://docs.microsoft.com/en-us/ef/core/)
- ASP.NET Core areas
- Razor
- SignalR
- SendGrid
- Hangfire
- Cloudinary (for storing images)
- Bootstrap 4
- AJAX
- jQuery
- JavaScript
- XUnit (for testing)
- Moq (for testing)

## :wrench: DB Diagram
![](https://res.cloudinary.com/boulderbox/image/upload/v1608549504/DatabaseDiagram_fumqsq.jpg)
