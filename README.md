# Notch
Using simple tab-indented (notched) text to represent the hierarchical design of a system in a technology-agnostic format that can be parsed into a queryable hierarchy of Composites that can be used to generate artifacts like documentation and diagrams, and technology-specific artifacts like code, databases, etc.

For Example, considering the following Notch:
    
Server
  API
    USPS
      Methods
        GetAddressInformationByPostalCode
        GetForecastByState
  CSV
    Fields
      PostalCode     
Server
  API
    Weather
      Methods
        GetForecastByPostalCode
        GetForecastByState
DatabaseServer
  Database
    Weather
      Tables
        Forecast
          Fields
            ForecastDate
            LatLongRectangle
            Description
            State
            PostalCode
        Satellite
          Fields
            Name
            PolarCoordinates
            OwningCountry

Once parsed, you can run a query asking for  
- APIs in the system
- Servers in the system 
- Anything with a field name of PostalCode (CSV and Forecast database table)
.
