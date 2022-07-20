

# OpenRealEstate.NET Transmogrifier : JSON

This library handles JSON tranformations:
- Reads: A transmogrifier (i.e. converter) that converts an OpenRealEstate JSON file to an OpenRealEstate model.
- Writes: convert an OpenRealEstate model to some JSON `string`.

### Why would I want to use the 'Json Transmogrifier' versus just using the classic 'DeserializeObject' ?

Good question! The json string could be representing:
- a single Listing
- multiple Listings.

As such, you would need to know (somehow?) what the json string content is representing: single or multiple? The `JsonTransmogrifier` is smart enough to determine this and therefore deserialize it, accordingly. 


# Main Methods to use

- `JsonTransmorgrifier`: this class allows you to parse some json string and it will do all the heavy lifting. Returns a `ParsedResult` which contains 0+ listings, parsed.
  - `transmorgrifier.Parse(json)` : the main method to do all the parsing.
- `JsonConvertHelpers`: Some extension methods to serialize/write some Listing(s) to a string or stream.
  - `listing.SerializeObject()` : Main extension method to convert listing(s) to a json string.
  - `JsonConvertHelpers.DeserializeObject(json)` : Basic (not preferred) method to converting a json string to a listing.
  - `JsonConvertHelpers.DeserializeObjects(json)` : Basic (not preferred) method to converting a json string to a list of listings.

# Examples - WRITING: Converting an object to a json string.

## Converting a Listing to a json string

```
using OpenRealEstate.Transmorgrifiers.Json; 

var json = listing.SerializeObject();
```

## Converting a list of Listing's to a json string

Note: is the list is large, this will create a large `string` which would consume a large amount of memory.

```
using OpenRealEstate.Transmorgrifiers.Json; 

var json = listings.SerializeObject();
```

## Converting a list of Listings to a Stream
This is a great example of minimizing RAM usage. Instead of converting the _entire_ list to a single string, this will convert a single listing, then write the json content to the stream .. then rinse-repeat for the remaining items in the list.

```
using OpenRealEstate.Transmorgrifiers.Json; 

using (var streamWriter = File.CreateText(destinationFile))
{
    listings.SerializeObject(streamWriter);
}

```

# Examples - READING: Converting some json content into listing(s)

There are 4 main types of listings. With polymorphism, it's a bit tricky to determine which exact listing object to deserialize the json data, into.

Some static helper methods are here to simplify this process.

## Prefered way: Some json that could be representing a single or multiple Listings.

```
using OpenRealEstate.Transmorgrifiers.Json;

var result = transmorgrifier.Parse(json)

// result.Listings  <- 0 to many Listings
// result.UnhandledData <-- any source data it couldn't parse.
// result.Errors <-- 0 to many Errors encoured during processing
```

If we have a json string which has multiple listings, but one of the listing 'segments' might have some bad json, this could get captured in the `Errors` property.

## BASIC: Some json representing a single listing

```
using OpenRealEstate.Transmorgrifiers.Json;

var json = <some json representing a listing. E.g maybe the content of a file?>

var listing = JsonConvertHelpers.DeserializeObject(json);
```

## BASIC: Some json representing a number of listings
```
using OpenRealEstate.Transmorgrifiers.Json;

var json = <some json representing a listing. E.g maybe the content of a file?>

var listings = JsonConvertHelpers.DeserializeObjects(json);
```



---

## Contributing

Discussions and pull requests are encouraged :) Please ask all general questions in this repo or pick a specialized repo for specific, targetted issues. We also have a [contributing](https://github.com/OpenRealEstate/OpenRealEstate/blob/master/CONTRIBUTING.md) document which goes into detail about how to do this.

## Code of Conduct
Yep, we also have a [code of conduct](https://github.com/OpenRealEstate/OpenRealEstate/blob/master/CODE_OF_CONDUCT.md) which applies to all repositories in the OpenRealEstate organisation.

## Feedback
Yep, refer to the [contributing page](https://github.com/OpenRealEstate/OpenRealEstate/blob/master/CONTRIBUTING.md) about how best to give feedback - either good or needs-improvement :)

---
