# City Information API

The City Information API is a powerful ASP.NET Core API designed to provide information about cities, including weather data and popular tourist places. It aims to assist developers in building applications that require city-related information. This README file provides an overview of the API, its features, and instructions on how to get started.

## Features

The City Information API offers the following features:

- **City Information**: Retrieve detailed information about a city, including its name, country, population, and geographical coordinates.
- **Weather Information**: Access current weather data for a specific city, including temperature, humidity, wind speed, and weather conditions.
- **Tourist Places**: Get a list of popular tourist places in a city, including their names, images, descriptions, and ratings.
- **Flexible Search**: Search for city information, weather data, or tourist places based on the city's name or unique identifier.
- **Secure and Scalable**: Built using ASP.NET Core, the API is secure, scalable, and can handle a high volume of requests.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download) (version 6.0 or higher)
- An API key from a weather data provider (e.g., OpenWeatherMap)
- An API key from a tourist places data provider (e.g., TripAdvisor)
- Dropbox account for storing images
- Fluent Validation package for request validation
- Xunit packages for unit testing
- Nlog Xunit packages for logging

### Installation

1. Clone this repository to your local machine or download the source code as a ZIP file.
2. Open a terminal or command prompt and navigate to the project's root directory.
3. Run the following command to restore the project dependencies:

   ```bash
   dotnet restore

4. Build the project by executing the following command:
    ```bash
    dotnet build

## Configuration
To configure the API, you need to provide the necessary API keys and other settings. Follow the steps below:

1. Open the appsettings.json file located in the project's root directory.

2. Update the values of the following configuration settings:

- "WeatherApi:ApiKey": Replace with your weather data provider API key.
- "TouristPlacesApi:ApiKey": Replace with your tourist places data provider API key.
#### Note: Make sure to keep your API keys secure and avoid committing them to version control systems.

## Storage Configuration
The City Information API uses Dropbox for storing images. To configure the storage, follow these steps:

<div style="margin: 20px;">
    <img src="https://cfl.dropboxstatic.com/static/metaserver/static/images/logo_catalog/dropbox_webclip_200_vis.png" alt="Image Description" width="200px" height="200px" />
</div>

1. Create a Dropbox account if you don't have one already.
2. Generate an access token from the Dropbox developer console.
3. Open the appsettings.json file located in the project's root directory.
4. Update the "StorageSettings:DropboxAccessToken" configuration setting with your generated access token.

## Request Validation
The City Information API utilizes Fluent Validation to perform request validation. Request models are automatically validated based on the validation rules defined in the corresponding validator classes. Validation errors are returned with appropriate status codes and error messages.

## Unit Testing
Unit tests are an essential part of any application. The City Information API includes unit tests to ensure the correctness of its functionalities and validate the request inputs. The tests utilize the Xunit framework.

## Usage

### Endpoints

The City Information API exposes the following endpoints:

- GET /api/cities: Retrieve a list of all available cities.
- GET /api/cities/{id}: Retrieve information about a specific city.
- GET /api/cities/{id}/weather: Retrieve the current weather information for a city.
- GET /api/cities/{id}/tourist-places: Retrieve a list of tourist places in a city.

## Authentication
The City Information API requires authentication using JWT tokens to access its endpoints. To obtain an access token, make a POST request to the /api/auth/login endpoint with valid credentials.   
The response will include an access token that you need to include in the Authorization header of subsequent requests.
