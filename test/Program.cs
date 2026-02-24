
using System.Diagnostics;
using System.Net;
using MaxMind.Db;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
string dbPath = "/Users/jshook/Downloads/citydb.mmdb";
Reader reader = new Reader(dbPath);

CityResponse? response = reader.Find<CityResponse>(IPAddress.Parse("8.8.8.8"));

reader.FindCityResponse(IPAddress.Parse("8.8.8.8"), out ValueCityResponse _);

return;

// var cityLookupSuccess = _CityDatabaseReader.TryCity(ipAddress, out var cityResult);
//             if (cityLookupSuccess && cityResult != null)
//             {
//                 // Fields could be null even on success
//                 result.Location.City = cityResult.City.Name;
//                 result.Location.Accuracy_radius = cityResult.Location.AccuracyRadius;
//                 result.Location.Longitude = cityResult.Location.Longitude;
//                 result.Location.Latitude = cityResult.Location.Latitude;
//             }

//             var ispLookupSuccess = _IspDatabaseReader.TryIsp(ipAddress, out var ispResult);
//             if (ispLookupSuccess && ispResult != null)
//             {
//                 // Fields could be null even on success
//                 result.Traits.Autonomous_system_number = ispResult.AutonomousSystemNumber;
//             }