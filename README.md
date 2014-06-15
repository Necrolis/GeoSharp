GeoSharp
========

An offline Geocoding library for .Net coded in C#.
Based on the awesome Java lib by AReallyGoodName: https://github.com/AReallyGoodName/OfflineReverseGeocode

## Requirements

- This library is built for .Net 4, however it should work fine with 3.5 and Mono.
- The data that is fed in needs to be in the `geoname` format as described [here](http://download.geonames.org/export/dump/readme.txt).
- [Here](http://download.geonames.org/export/dump/) you'll find various sets of formatted data to use for the reverse geocoding.

## Performance

The look ups are very fast, `O(logn)`, however building the initial KD-Tree is not so fast, `O(kn * logn)`,
so pre-selecting your datasets and filtering out places or areas that are not of interest (like water bodies) can be used to improve load speed and reduce
memory usage.

Filtering of data can be done offline using the `GeoDBOptimize` class to create tailored `geoname` tables.

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
	GeoSharp.ReverseGeoCode geocode = new GeoSharp.ReverseGeoCode(ms, true);
	Console.WriteLine(geocode.NearestPlaceName(40.730885, -73.997383));
}
```

## License

Licensed under **LGPL 2.1** with an exception intended for Android users to allow static linking of the unmodified publicly distributed version of the Library, see LICENSE_EXCEPTION for details
