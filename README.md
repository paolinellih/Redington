CalculatorAPI (Backend)
A C# .NET 8 API that provides probability calculation services endpoints. The API supports structured responses, health checks, and includes logging functionality.

Prerequisites
.NET 8 SDK installed.
Visual Studio 2022 or any compatible IDE (e.g., Rider, VS Code).
Access to an internet connection to restore NuGet packages.

Running the API
Clone the repository:

git clone <repository-url>
cd CalculatorAPI
Navigate to the CalculatorAPI folder.

Restore dependencies:

dotnet restore
Build the solution:

dotnet build
Run the application:

dotnet run
The API will start on the following URLs by default:

HTTP: http://localhost:5000
HTTPS: https://localhost:44300

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CalculatorAPP - Frontend (React.js with VITE)
A React.js application built using the VITE toolchain to provide a user interface for interacting with the CalculatorAPI.

Prerequisites
Node.js (version 16 or later recommended).
A package manager like npm or yarn.
Running the Frontend
Clone the repository:

git clone <repository-url>
cd frontend
Navigate to the frontend folder.

Install dependencies:

npm install
Run the development server:

npm run dev
Open your browser and navigate to the URL provided by the VITE server, typically:

http://localhost:5173
Connecting Backend and Frontend
The React app should point to the backend API endpoints for data. Update the backend API base URL in the frontend configuration file if required. For example:

src/config.js:
export const API_BASE_URL = 'https://localhost:44300';
Test the connection by navigating to the frontend application and interacting with the UI to trigger API calls.
