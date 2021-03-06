using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenRealEstate.Core;
using OpenRealEstate.Transmorgrifiers.Core;

namespace OpenRealEstate.Transmorgrifiers.Json
{
    public class JsonTransmorgrifier : ITransmorgrifier
    {
        /// <inheritdoc />
        public string Name => "Json";

        /// <inheritdoc />
        public ParsedResult Parse(string data,
                                  Listing existingListing = null,
                                  bool areBadCharactersRemoved = false)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentException(nameof(data));
            }

            var result = new ParsedResult()
            {
                TransmorgrifierName = Name
            };

            JToken token;

            try
            {
                token = JToken.Parse(data);
            }
            catch (Exception exception)
            {
                result.Errors = new[]
                {
                    new ParsedError(exception.Message, data)
                };
                return result;
            }

            // Do we have a single listing or an array of listings?
            if (token is JArray)
            {
                // We have multiple listings...
                foreach (var item in token.Children())
                {
                    var parsedResult = ParseObject(item.ToString());
                    MergeParsedResults(parsedResult, result);
                }
            }
            else
            {
                // We have just a single listing ...
                var parsedResult = ParseObject(data);
                MergeParsedResults(parsedResult, result);
            }

            return result;
        }

        private ParsedResult ParseObject(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(nameof(json));
            }

            Listing listing = null;
            Exception error = null;

            try
            {
                listing = JsonConvertHelpers.DeserializeObject(json);
            }
            catch (Exception exception)
            {
                error = exception;
            }

            var parsedResult = new ParsedResult
            {
                Listings = new List<ListingResult>(),
                Errors = new List<ParsedError>(),
                TransmorgrifierName = Name
            };

            if (listing != null)
            {
                var listingResult = new ListingResult
                {
                    Listing = listing,
                    SourceData = json,
                    Warnings = new List<string>()
                };
                parsedResult.Listings.Add(listingResult);
            }

            if (error != null)
            {
                parsedResult.Errors.Add(new ParsedError(error.Message, json));
            }

            return parsedResult;
        }

        private static void MergeParsedResults(ParsedResult source,
                                               ParsedResult destination)
        {
            if (source.Listings != null &&
                source.Listings.Any())
            {
                foreach (var listingResult in source.Listings)
                {
                    if (destination.Listings == null)
                    {
                        destination.Listings = new List<ListingResult>();
                    }

                    destination.Listings.Add(listingResult);
                }
            }

            if (source.Errors != null &&
                source.Errors.Any())
            {
                foreach (var parsedError in source.Errors)
                {
                    if (destination.Errors == null)
                    {
                        destination.Errors = new List<ParsedError>();
                    }

                    destination.Errors.Add(parsedError);
                }
            }
        }
    }
}
