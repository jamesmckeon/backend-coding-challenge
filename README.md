# Suggest API

Suggest is a Rest Api that provides autocomplete suggestions for large cities, airports, and hospitals.  While it currently integrates solely with the [GeoNames Api](http://www.geonames.org/), Suggest can be plugged into any combination of geo data apis (google, open street maps, etc.), and can easily be extended to provide suggestions for additional geo locations (e.g., post offices, stadiums, schools)

Suggest was developed as a solution to Coveo's back end coding [challenge](https://github.com/coveo/backend-coding-challenge)

## Building Queries

Suggest queries are constructed using any combination of the following:

- *q*: the place name to search for.  Name search is executed as "startsWith" rather than "contains", and is case and whitespace - insensitive (optional)
- *longitude*: used with latitude to specify a location to search near (required if latitude is provided) 
- *latitude*: used with longitude to specify a location to search near (required if longitude is provided)
- *type*: specifies what format should be returned (json or xml); default is xml (optional)
- *MaxResults*: the maximum number of results that should be returned, ordered descending by Certainty


Suggest currently offers three service methods
- suggestions (returns [large cities](https://en.wikipedia.org/wiki/Settlement_hierarchy))
- hospitals
- airports

## Response Data

A Suggest query returns a list of Suggestions:

- *Name*:  the name of a place, including state/province (depending on country) and country code. 
- *Latitude* 
- *Longitude*
- *Certainty*:  Indicates how relevant a search result/suggestion is to its source query
- *Links*: A collection of links that can be used to navigate the Api using the Suggestion's coordinates

Example JSON response :

```
{
    "Name": "Cary, IL, US",
    "Latitude": 42.21197,
    "Longitude": -88.23814,
    "Certainty": 1,
    "Links": [
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Airports?longitude=-88.23814&latitude=42.21197",
        "Rel": "Airports",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Hospitals?longitude=-88.23814&latitude=42.21197",
        "Rel": "Hospitals",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Suggestions?longitude=-88.23814&latitude=42.21197",
        "Rel": "Suggestions",
        "Method": "GET"
      }
    ]
  },
  {
    "Name": "Cicero, IL, US",
    "Latitude": 41.84559,
    "Longitude": -87.75394,
    "Certainty": 0.86,
    "Links": [
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Airports?longitude=-87.75394&latitude=41.84559",
        "Rel": "Airports",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Hospitals?longitude=-87.75394&latitude=41.84559",
        "Rel": "Hospitals",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Suggestions?longitude=-87.75394&latitude=41.84559",
        "Rel": "Suggestions",
        "Method": "GET"
      }
    ]
  },
  {
    "Name": "Chicago, IL, US",
    "Latitude": 41.85003,
    "Longitude": -87.65005,
    "Certainty": 0.8,
    "Links": [
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Airports?longitude=-87.65005&latitude=41.85003",
        "Rel": "Airports",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Hospitals?longitude=-87.65005&latitude=41.85003",
        "Rel": "Hospitals",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Suggestions?longitude=-87.65005&latitude=41.85003",
        "Rel": "Suggestions",
        "Method": "GET"
      }
    ]
  },
  {
    "Name": "Charleston, IL, US",
    "Latitude": 41.91419,
    "Longitude": -88.30869,
    "Certainty": 0.67,
    "Links": [
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Airports?longitude=-88.30869&latitude=41.91419",
        "Rel": "Airports",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Hospitals?longitude=-88.30869&latitude=41.91419",
        "Rel": "Hospitals",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Suggestions?longitude=-88.30869&latitude=41.91419",
        "Rel": "Suggestions",
        "Method": "GET"
      }
    ]
  },
  {
    "Name": "Crest Hill, IL, US",
    "Latitude": 41.55475,
    "Longitude": -88.09867,
    "Certainty": 0.67,
    "Links": [
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Airports?longitude=-88.09867&latitude=41.55475",
        "Rel": "Airports",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Hospitals?longitude=-88.09867&latitude=41.55475",
        "Rel": "Hospitals",
        "Method": "GET"
      },
      {
        "Href": "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/Suggestions?longitude=-88.09867&latitude=41.55475",
        "Rel": "Suggestions",
        "Method": "GET"
      }
    ]
  }
```
  
##  Query Examples

Airports near Portland, OR, USA: 
```
    hospitals?type=json&latitude=45.52345&Longitude=-122.67621
``` 
Airports with names starting with "Los A", json return type:
```
    airports?q=los%20a&type=json
```

## Endpoints
http://autocomplete.us-east-2.elasticbeanstalk.com/api

## Caching
Data caches are flushed every 4 hours, depending on the exact search executed


### Development Notes

Suggest is a .Net Web Api 2.0 app developed using Visual Studio 2015:
- [Nuget](http://nuget.org/) for package management
- unit tested with [NUnit](http://nunit.org/) and [Moq](https://github.com/moq/moq)
- DI via MS Unity
- [Fiddler](http://www.telerik.com/fiddler) and [Advanced Rest Client](https://chrome.google.com/webstore/detail/advanced-rest-client/hgmloofddffdnphfgcellkdfbfbjeloo?hl=en-US) for functional testing

## Deployment
Deployed to AWS via the AWS toolkit for Visual Studio


## Authors

* **Jim Mckeon** - *Initial work* -


## Acknowledgments
* [GeoNames](http://www.geonames.org/) for data of course
* Bounding box logic was found [here](https://stackoverflow.com/a/14315990/1342632)
