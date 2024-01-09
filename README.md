# NewRainfallApi

## Overview

NewRainfallApi is a .NET project designed to provide rainfall reading data through a RESTful API. Leveraging the Environment Agency's flood monitoring service, the API allows users to retrieve rainfall readings for specific stations, along with features like limiting the number of readings. The project includes error handling, response structures, and a custom JSON converter for efficient data processing.

## Base URL

`http://localhost:3000/api/rainfall`

## Endpoints

### Get Rainfall Readings

#### Endpoint

`GET /id/{stationId}/readings`

#### Description

Retrieve the latest rainfall readings for the specified station ID.

#### Parameters

- `stationId` (required): The ID of the reading station.
- `count` (optional): The number of readings to return. Defaults to 10.

#### Responses
- **200 OK:** Successful response with a list of rainfall readings.
```json
[
  {
    "dateMeasured": "2024-01-08T12:30:00Z",
    "amountMeasured": 12.5
  },
  // Additional readings...
]
```

- **400 Bad Request:** Invalid request, with details about the error.
```json
{
  "errors": [
    {
      "message": "Count must be between 1 and 100.",
      "detail": [
        {
          "propertyName": "count",
          "message": "Invalid value."
        }
      ]
    }
  ]
}
```

- **404 Not Found:** No readings found for the specified station ID.
```json
{
  "errors": [
    {
      "message": "No readings found for the specified stationId",
      "detail": [
        {
          "propertyName": "stationId",
          "message": "No stationId found."
        }
      ]
    }
  ]
}
```

- **500 Internal Server Error:** An internal server error occurred.
```json
{
  "errors": [
    {
      "message": "Internal server error."
    }
  ]
}
```

### Author
Mark Jason T. Galang

### Applying for
.NET Developer position (Junior)

### Version
1.0

### Powered by
.NET 8 (Long-term support)

