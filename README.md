Agri-Energy Connect Platform

#Log In details

Farmer Login details:
Email: Farmer@gmail.com
Password: Default123!

Employee Login details:
Email: Clive@gmail.com
Password: Default123!

#Overview

Agri-Energy Connect is a web application built with ASP.NET Core and SQLite. It allows two types of users—Farmers and Employees—to perform distinct actions:

##Farmers can:

-Register with Name, Surname, Email, Role (Farmer), Password, and Confirm Password.
-Add Products via an "Add Products" page.
-View Products on a page listing all products they have inserted.

##Employees can:

-Register with Name, Surname, Email, Role (Employee), Password, and Confirm Password.
-Add Farmers via an "Add Farmer" page (new farmers are created with the default password Default123!).
-View Products on a page where they can filter products by date range and category.

Both user types can Log Out of the application.

#Prerequisites

-.NET 8 SDK
-DB Browser for SQLite
-A modern web browser (Chrome, Edge, Firefox, Safari)

##Password Requirements

-When registering, passwords must meet the following security criteria:

-At least 8 characters in length
-At least one uppercase letter
-At least one number
-At least one symbol (e.g. !@#$%^&*)

##Installation & Setup

-Download the source code
-Locate the zipped project file (e.g., AgriEnergyConnect.zip).
-Download it to your local machine.
-Extract the zip archive
-Right-click the .zip file and select Extract All... (Windows) or double-click on macOS.
-Choose a destination folder (e.g., C:\Desktop\PROG7311_ST10267937_Part2_POE).
-Open the project in your IDE
-Launch Visual Studio or VS Code.
-Visual Studio: Select Open a project or solution, navigate to the extracted folder, and open PROG_Part2_POE.sln.

##Restore NuGet packages

cd /path/to/PROG_Part2_POE
dotnet restore

##Running the Application

From Visual Studio:

Build Solution
Click on green 'https' button


From the command line:

dotnet build
dotnet run 

-The application will start on https://localhost:5001 (HTTPS) and http://localhost:5000 (HTTP).
-Open your browser and navigate to the URL shown in the console.
-Or, press F5 (Debug) or Ctrl+F5 (Run without Debug) in Visual Studio.

##Database (SQLite)

-The app uses SQLite for data storage. The database file (app.db) is located in the Data folder of the project.

-Open DB Browser for SQLite
-File > Open Database
-Navigate to Data/app.db in your project folder.
-You can browse tables, run SQL queries, and inspect the data.

##Features

Registration & Authentication

-All users must register with Name, Surname, Email, Role, Password, and Confirm Password.
-Passwords must meet the security criteria outlined above.

##Farmer Role

-Add Products: Enter product name, description, price, category, and optionally upload an image.
-View Products: See a list of all products you have added, with options to edit or delete.

##Employee Role

-Add Farmer: Create a new farmer user; the new account uses the default password Default123!.
-View Products: Browse all products across farmers; filter by date range and product category.

##Logout

-Click Logout to securely end your session.

##Troubleshooting

-Database file missing: Ensure you have run the EF Core migrations or have the correct app.db in your Data folder.

##GitHub Repository

-View the full source code on GitHub: https://github.com/SamiMoosa/PROG7311_Part2_POE

##License

-This project is licensed under the MIT License. See LICENSE for details.

##Acknowledgements

This project was developed with the assistance of AI-powered conversational insights. ChatGPT 04-mini-high was used to brainstorm ideas, refine wording, and solve layout and design challenges. AI suggestions were used judiciously to enhance efficiency without replacing human decision-making.