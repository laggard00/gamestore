
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using nosqlconverter;
using nosqlconverter.Converters;
using nosqlconverter.DBContexts;

Console.WriteLine("Hello, World!");
BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
var mongoClient = new MongoClient("mongodb://localhost:27017");
var northWind = new NorthwindDbContext();
using ( var context = new GameStoreDbContext()){
    var Converter = new MongoToGameStoreOrderDetails(context,northWind, mongoClient);
    await Converter.ConvertToGameStore();
}


Console.WriteLine("Success");


Console.ReadLine();