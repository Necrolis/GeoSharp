GeoSharp
========

An offline Geocoding library for .Net coded in C#.
Based on the awesome Java lib by AReallyGoodName: https://github.com/AReallyGoodName/OfflineReverseGeocode

## Requirements

This library is built for .Net 4, however it should work fine with 3.5 and Mono.
The data that is fed in needs to be in the geoname format as described [here](http://download.geonames.org/export/dump/readme.txt).
[Here](http://download.geonames.org/export/dump/) you'll find various sets of data to use for the geocoding.

## Performance

The look ups are very fast, `O(logn)`, however building the initial KD-Tree is not so fast, `O(kn * logn)`),
so pre-selecting your initial datasets and filtering out minor places can be used to improve load speed and reduce
memory usage.

## Usage

Load a database from a file
```cs
GeoSharp.ReverseGeoCode geocode = new GeoSharp.ReverseGeoCode(@"C:\allCountries.txt", true);
Console.WriteLine(geocode.NearestPlaceName(40.730885, -73.997383));
```

Load a database from a `Stream`
```cs
using (MemoryStream ms = new MemoryStream(MemoryDB))
{
	GeoSharp.ReverseGeoCode geocode = new GeoSharp.ReverseGeoCode(, true);
	Console.WriteLine(geocode.NearestPlaceName(40.730885, -73.997383));
}
```

## License

Licensed under **LGPL 2.1**
