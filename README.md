## Prerequisites

Ensure you have the following installed:

- **.NET 8 SDK:** [Download & Install .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server:** A running instance of SQL Server Express LocalDB.
- **.NET EF Core Tools:** Install the command-line tools for Entity Framework Core.
  ```sh
  dotnet tool install --global dotnet-ef
  ```

## Configuration

The database connection string don't need to be configured before running the application, but if needed, it can be.

1.  Open the `appsettings.json` file located at `FileBrowser.Api/appsettings.json`.
2.  Locate the `ConnectionStrings` section.
3.  Update the `FileBrowserConnection` value with the connection string for your SQL Server instance.

**Example `appsettings.json`:**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "FileBrowserConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FileBrowserDb;Trusted_Connection=true;"
  },
  "AllowedHosts": "*"
}
```

## Database Setup

This project uses Entity Framework Core for database migrations. To create and seed the database, run the following command from the **root directory** of the project:

```sh
dotnet ef database update --project FileBrowser.Data
```

This command will read the connection string from `FileBrowser.Api/appsettings.json`, create the database if it doesn't exist, and apply all migrations to set up the required tables.

## Running the Application

You can run the application directly using the .NET CLI.

1.  **Build the solution:**

    ```sh
    dotnet build
    ```

2.  **Run the API project:**

    ```sh
    dotnet run --project FileBrowser.Api --launch-profile https
    ```

By default, the API will be available at `https://localhost:7262` and `http://localhost:5126`.
Or you can test API's with swagger: `https://localhost:7262/swagger/index.html` and `http://localhost:5126/swagger/index.html`

## API Usage

Once the application is running, you can interact with the API.

-   **Swagger UI:** A Swagger UI will be available at `/swagger` for exploring and testing the API endpoints.
    (e.g., `https://localhost:7262/swagger/index.html`, `http://localhost:5126/swagger/index.html`)

-   **Main Endpoints:**
    -   `GET /api/folders`: Retrieves all folders.
    -   `GET /api/folders/{id}`: Retrieves a specific folder by its Id.
    -   `POST /api/folders`: Creates a new folder.
    -   `PUT /api/folders/{id}`: Updates an existing folder.
    -   `DELETE /api/folders/{id}`: Deletes a folder by Id.
    -   `GET /api/folders/subfolders/{parentId}`: Retrieves all files and folders within a specific folder.

    -   `GET /api/files`: Retrieves all files.
    -   `GET /api/files/{id}`: Retrieves a specific file by its Id.
    -   `POST /api/files`: Creates a new file.
    -   `PUT /api/files/{id}`: Updates an existing file.
    -   `DELETE /api/file/{id}`: Deletes a file by Id.
    -   `GET /api/file/folder/{folderId}`: Retrieves all files within a specific folder.
    -   `GET /api/file/search`: Search files across all folders by file name.
    -   `GET /api/file/search-in-folder/{folderId}`: Search files in a specific folder by file name.
