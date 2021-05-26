// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.DbFunctions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Entity
{
  /// <summary>
  /// Provides common language runtime (CLR) methods that expose EDM canonical functions
  /// for use in <see cref="T:System.Data.Entity.DbContext" /> or <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" /> LINQ to Entities queries.
  /// </summary>
  /// <remarks>
  /// Note that this class was called EntityFunctions in some previous versions of Entity Framework.
  /// </remarks>
  public static class DbFunctions
  {
    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<Decimal> collection) => DbFunctions.BootstrapFunction<Decimal, double?>((Expression<Func<IEnumerable<Decimal>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<Decimal?> collection) => DbFunctions.BootstrapFunction<Decimal?, double?>((Expression<Func<IEnumerable<Decimal?>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<double> collection) => DbFunctions.BootstrapFunction<double, double?>((Expression<Func<IEnumerable<double>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<double?> collection) => DbFunctions.BootstrapFunction<double?, double?>((Expression<Func<IEnumerable<double?>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<int> collection) => DbFunctions.BootstrapFunction<int, double?>((Expression<Func<IEnumerable<int>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<int?> collection) => DbFunctions.BootstrapFunction<int?, double?>((Expression<Func<IEnumerable<int?>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<long> collection) => DbFunctions.BootstrapFunction<long, double?>((Expression<Func<IEnumerable<long>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDev EDM function to calculate
    /// the standard deviation of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation. </returns>
    [DbFunction("Edm", "StDev")]
    public static double? StandardDeviation(IEnumerable<long?> collection) => DbFunctions.BootstrapFunction<long?, double?>((Expression<Func<IEnumerable<long?>, double?>>) (c => DbFunctions.StandardDeviation(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<Decimal> collection) => DbFunctions.BootstrapFunction<Decimal, double?>((Expression<Func<IEnumerable<Decimal>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<Decimal?> collection) => DbFunctions.BootstrapFunction<Decimal?, double?>((Expression<Func<IEnumerable<Decimal?>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<double> collection) => DbFunctions.BootstrapFunction<double, double?>((Expression<Func<IEnumerable<double>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<double?> collection) => DbFunctions.BootstrapFunction<double?, double?>((Expression<Func<IEnumerable<double?>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<int> collection) => DbFunctions.BootstrapFunction<int, double?>((Expression<Func<IEnumerable<int>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<int?> collection) => DbFunctions.BootstrapFunction<int?, double?>((Expression<Func<IEnumerable<int?>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<long> collection) => DbFunctions.BootstrapFunction<long, double?>((Expression<Func<IEnumerable<long>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical StDevP EDM function to calculate
    /// the standard deviation for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The standard deviation for the population. </returns>
    [DbFunction("Edm", "StDevP")]
    public static double? StandardDeviationP(IEnumerable<long?> collection) => DbFunctions.BootstrapFunction<long?, double?>((Expression<Func<IEnumerable<long?>, double?>>) (c => DbFunctions.StandardDeviationP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<Decimal> collection) => DbFunctions.BootstrapFunction<Decimal, double?>((Expression<Func<IEnumerable<Decimal>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<Decimal?> collection) => DbFunctions.BootstrapFunction<Decimal?, double?>((Expression<Func<IEnumerable<Decimal?>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<double> collection) => DbFunctions.BootstrapFunction<double, double?>((Expression<Func<IEnumerable<double>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<double?> collection) => DbFunctions.BootstrapFunction<double?, double?>((Expression<Func<IEnumerable<double?>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<int> collection) => DbFunctions.BootstrapFunction<int, double?>((Expression<Func<IEnumerable<int>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<int?> collection) => DbFunctions.BootstrapFunction<int?, double?>((Expression<Func<IEnumerable<int?>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<long> collection) => DbFunctions.BootstrapFunction<long, double?>((Expression<Func<IEnumerable<long>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Var EDM function to calculate
    /// the variance of the collection.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance. </returns>
    [DbFunction("Edm", "Var")]
    public static double? Var(IEnumerable<long?> collection) => DbFunctions.BootstrapFunction<long?, double?>((Expression<Func<IEnumerable<long?>, double?>>) (c => DbFunctions.Var(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<Decimal> collection) => DbFunctions.BootstrapFunction<Decimal, double?>((Expression<Func<IEnumerable<Decimal>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<Decimal?> collection) => DbFunctions.BootstrapFunction<Decimal?, double?>((Expression<Func<IEnumerable<Decimal?>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<double> collection) => DbFunctions.BootstrapFunction<double, double?>((Expression<Func<IEnumerable<double>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<double?> collection) => DbFunctions.BootstrapFunction<double?, double?>((Expression<Func<IEnumerable<double?>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<int> collection) => DbFunctions.BootstrapFunction<int, double?>((Expression<Func<IEnumerable<int>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<int?> collection) => DbFunctions.BootstrapFunction<int?, double?>((Expression<Func<IEnumerable<int?>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<long> collection) => DbFunctions.BootstrapFunction<long, double?>((Expression<Func<IEnumerable<long>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical VarP EDM function to calculate
    /// the variance for the population.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="collection"> The collection over which to perform the calculation. </param>
    /// <returns> The variance for the population. </returns>
    [DbFunction("Edm", "VarP")]
    public static double? VarP(IEnumerable<long?> collection) => DbFunctions.BootstrapFunction<long?, double?>((Expression<Func<IEnumerable<long?>, double?>>) (c => DbFunctions.VarP(c)), collection);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Left EDM function to return a given
    /// number of the leftmost characters in a string.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="stringArgument"> The input string. </param>
    /// <param name="length"> The number of characters to return </param>
    /// <returns> A string containing the number of characters asked for from the left of the input string. </returns>
    [DbFunction("Edm", "Left")]
    public static string Left(string stringArgument, long? length) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Right EDM function to return a given
    /// number of the rightmost characters in a string.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="stringArgument"> The input string. </param>
    /// <param name="length"> The number of characters to return </param>
    /// <returns> A string containing the number of characters asked for from the right of the input string. </returns>
    [DbFunction("Edm", "Right")]
    public static string Right(string stringArgument, long? length) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Reverse EDM function to return a given
    /// string with the order of the characters reversed.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="stringArgument"> The input string. </param>
    /// <returns> The input string with the order of the characters reversed. </returns>
    [DbFunction("Edm", "Reverse")]
    public static string Reverse(string stringArgument) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical GetTotalOffsetMinutes EDM function to
    /// return the number of minutes that the given date/time is offset from UTC. This is generally between +780
    /// and -780 (+ or - 13 hrs).
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateTimeOffsetArgument"> The date/time value to use. </param>
    /// <returns> The offset of the input from UTC. </returns>
    [DbFunction("Edm", "GetTotalOffsetMinutes")]
    public static int? GetTotalOffsetMinutes(DateTimeOffset? dateTimeOffsetArgument) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical TruncateTime EDM function to return
    /// the given date with the time portion cleared.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The date/time value to use. </param>
    /// <returns> The input date with the time portion cleared. </returns>
    [DbFunction("Edm", "TruncateTime")]
    public static DateTimeOffset? TruncateTime(DateTimeOffset? dateValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical TruncateTime EDM function to return
    /// the given date with the time portion cleared.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The date/time value to use. </param>
    /// <returns> The input date with the time portion cleared. </returns>
    [DbFunction("Edm", "TruncateTime")]
    public static DateTime? TruncateTime(DateTime? dateValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical CreateDateTime EDM function to
    /// create a new <see cref="T:System.DateTime" /> object.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month (1-based). </param>
    /// <param name="day"> The day (1-based). </param>
    /// <param name="hour"> The hours. </param>
    /// <param name="minute"> The minutes. </param>
    /// <param name="second"> The seconds, including fractional parts of the seconds if desired. </param>
    /// <returns> The new date/time. </returns>
    [DbFunction("Edm", "CreateDateTime")]
    public static DateTime? CreateDateTime(
      int? year,
      int? month,
      int? day,
      int? hour,
      int? minute,
      double? second)
    {
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);
    }

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical CreateDateTimeOffset EDM function to
    /// create a new <see cref="T:System.DateTimeOffset" /> object.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="year"> The year. </param>
    /// <param name="month"> The month (1-based). </param>
    /// <param name="day"> The day (1-based). </param>
    /// <param name="hour"> The hours. </param>
    /// <param name="minute"> The minutes. </param>
    /// <param name="second"> The seconds, including fractional parts of the seconds if desired. </param>
    /// <param name="timeZoneOffset"> The time zone offset part of the new date. </param>
    /// <returns> The new date/time. </returns>
    [DbFunction("Edm", "CreateDateTimeOffset")]
    public static DateTimeOffset? CreateDateTimeOffset(
      int? year,
      int? month,
      int? day,
      int? hour,
      int? minute,
      double? second,
      int? timeZoneOffset)
    {
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);
    }

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical CreateTime EDM function to
    /// create a new <see cref="T:System.TimeSpan" /> object.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="hour"> The hours. </param>
    /// <param name="minute"> The minutes. </param>
    /// <param name="second"> The seconds, including fractional parts of the seconds if desired. </param>
    /// <returns> The new time span. </returns>
    [DbFunction("Edm", "CreateTime")]
    public static TimeSpan? CreateTime(int? hour, int? minute, double? second) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddYears EDM function to
    /// add the given number of years to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The input date/time. </param>
    /// <param name="addValue"> The number of years to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddYears")]
    public static DateTimeOffset? AddYears(DateTimeOffset? dateValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddYears EDM function to
    /// add the given number of years to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The input date/time. </param>
    /// <param name="addValue"> The number of years to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddYears")]
    public static DateTime? AddYears(DateTime? dateValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMonths EDM function to
    /// add the given number of months to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The input date/time. </param>
    /// <param name="addValue"> The number of months to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMonths")]
    public static DateTimeOffset? AddMonths(DateTimeOffset? dateValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMonths EDM function to
    /// add the given number of months to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The input date/time. </param>
    /// <param name="addValue"> The number of months to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMonths")]
    public static DateTime? AddMonths(DateTime? dateValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddDays EDM function to
    /// add the given number of days to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The input date/time. </param>
    /// <param name="addValue"> The number of days to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddDays")]
    public static DateTimeOffset? AddDays(DateTimeOffset? dateValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddDays EDM function to
    /// add the given number of days to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue"> The input date/time. </param>
    /// <param name="addValue"> The number of days to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddDays")]
    public static DateTime? AddDays(DateTime? dateValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddHours EDM function to
    /// add the given number of hours to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of hours to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddHours")]
    public static DateTimeOffset? AddHours(DateTimeOffset? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddHours EDM function to
    /// add the given number of hours to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of hours to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddHours")]
    public static DateTime? AddHours(DateTime? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddHours EDM function to
    /// add the given number of hours to a time span.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of hours to add. </param>
    /// <returns> A resulting time span. </returns>
    [DbFunction("Edm", "AddHours")]
    public static TimeSpan? AddHours(TimeSpan? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMinutes EDM function to
    /// add the given number of minutes to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of minutes to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMinutes")]
    public static DateTimeOffset? AddMinutes(DateTimeOffset? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMinutes EDM function to
    /// add the given number of minutes to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of minutes to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMinutes")]
    public static DateTime? AddMinutes(DateTime? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMinutes EDM function to
    /// add the given number of minutes to a time span.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of minutes to add. </param>
    /// <returns> A resulting time span. </returns>
    [DbFunction("Edm", "AddMinutes")]
    public static TimeSpan? AddMinutes(TimeSpan? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddSeconds EDM function to
    /// add the given number of seconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of seconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddSeconds")]
    public static DateTimeOffset? AddSeconds(DateTimeOffset? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddSeconds EDM function to
    /// add the given number of seconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of seconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddSeconds")]
    public static DateTime? AddSeconds(DateTime? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddSeconds EDM function to
    /// add the given number of seconds to a time span.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of seconds to add. </param>
    /// <returns> A resulting time span. </returns>
    [DbFunction("Edm", "AddSeconds")]
    public static TimeSpan? AddSeconds(TimeSpan? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMilliseconds EDM function to
    /// add the given number of milliseconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of milliseconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMilliseconds")]
    public static DateTimeOffset? AddMilliseconds(
      DateTimeOffset? timeValue,
      int? addValue)
    {
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);
    }

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMilliseconds EDM function to
    /// add the given number of milliseconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of milliseconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMilliseconds")]
    public static DateTime? AddMilliseconds(DateTime? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMilliseconds EDM function to
    /// add the given number of milliseconds to a time span.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of milliseconds to add. </param>
    /// <returns> A resulting time span. </returns>
    [DbFunction("Edm", "AddMilliseconds")]
    public static TimeSpan? AddMilliseconds(TimeSpan? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMicroseconds EDM function to
    /// add the given number of microseconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of microseconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMicroseconds")]
    public static DateTimeOffset? AddMicroseconds(
      DateTimeOffset? timeValue,
      int? addValue)
    {
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);
    }

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMicroseconds EDM function to
    /// add the given number of microseconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of microseconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddMicroseconds")]
    public static DateTime? AddMicroseconds(DateTime? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddMicroseconds EDM function to
    /// add the given number of microseconds to a time span.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of microseconds to add. </param>
    /// <returns> A resulting time span. </returns>
    [DbFunction("Edm", "AddMicroseconds")]
    public static TimeSpan? AddMicroseconds(TimeSpan? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddNanoseconds EDM function to
    /// add the given number of nanoseconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of nanoseconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddNanoseconds")]
    public static DateTimeOffset? AddNanoseconds(
      DateTimeOffset? timeValue,
      int? addValue)
    {
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);
    }

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddNanoseconds EDM function to
    /// add the given number of nanoseconds to a date/time.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of nanoseconds to add. </param>
    /// <returns> A resulting date/time. </returns>
    [DbFunction("Edm", "AddNanoseconds")]
    public static DateTime? AddNanoseconds(DateTime? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical AddNanoseconds EDM function to
    /// add the given number of nanoseconds to a time span.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue"> The input date/time. </param>
    /// <param name="addValue"> The number of nanoseconds to add. </param>
    /// <returns> A resulting time span. </returns>
    [DbFunction("Edm", "AddNanoseconds")]
    public static TimeSpan? AddNanoseconds(TimeSpan? timeValue, int? addValue) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffYears EDM function to
    /// calculate the number of years between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue1"> The first date/time. </param>
    /// <param name="dateValue2"> The second date/time. </param>
    /// <returns> The number of years between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffYears")]
    public static int? DiffYears(DateTimeOffset? dateValue1, DateTimeOffset? dateValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffYears EDM function to
    /// calculate the number of years between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue1"> The first date/time. </param>
    /// <param name="dateValue2"> The second date/time. </param>
    /// <returns> The number of years between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffYears")]
    public static int? DiffYears(DateTime? dateValue1, DateTime? dateValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMonths EDM function to
    /// calculate the number of months between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue1"> The first date/time. </param>
    /// <param name="dateValue2"> The second date/time. </param>
    /// <returns> The number of months between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMonths")]
    public static int? DiffMonths(DateTimeOffset? dateValue1, DateTimeOffset? dateValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMonths EDM function to
    /// calculate the number of months between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue1"> The first date/time. </param>
    /// <param name="dateValue2"> The second date/time. </param>
    /// <returns> The number of months between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMonths")]
    public static int? DiffMonths(DateTime? dateValue1, DateTime? dateValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffDays EDM function to
    /// calculate the number of days between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue1"> The first date/time. </param>
    /// <param name="dateValue2"> The second date/time. </param>
    /// <returns> The number of days between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffDays")]
    public static int? DiffDays(DateTimeOffset? dateValue1, DateTimeOffset? dateValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffDays EDM function to
    /// calculate the number of days between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="dateValue1"> The first date/time. </param>
    /// <param name="dateValue2"> The second date/time. </param>
    /// <returns> The number of days between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffDays")]
    public static int? DiffDays(DateTime? dateValue1, DateTime? dateValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffHours EDM function to
    /// calculate the number of hours between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of hours between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffHours")]
    public static int? DiffHours(DateTimeOffset? timeValue1, DateTimeOffset? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffHours EDM function to
    /// calculate the number of hours between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of hours between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffHours")]
    public static int? DiffHours(DateTime? timeValue1, DateTime? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffHours EDM function to
    /// calculate the number of hours between two time spans.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first time span. </param>
    /// <param name="timeValue2"> The second time span. </param>
    /// <returns> The number of hours between the first and second time spans. </returns>
    [DbFunction("Edm", "DiffHours")]
    public static int? DiffHours(TimeSpan? timeValue1, TimeSpan? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMinutes EDM function to
    /// calculate the number of minutes between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of minutes between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMinutes")]
    public static int? DiffMinutes(DateTimeOffset? timeValue1, DateTimeOffset? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMinutes EDM function to
    /// calculate the number of minutes between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of minutes between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMinutes")]
    public static int? DiffMinutes(DateTime? timeValue1, DateTime? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMinutes EDM function to
    /// calculate the number of minutes between two time spans.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first time span. </param>
    /// <param name="timeValue2"> The second time span. </param>
    /// <returns> The number of minutes between the first and second time spans. </returns>
    [DbFunction("Edm", "DiffMinutes")]
    public static int? DiffMinutes(TimeSpan? timeValue1, TimeSpan? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffSeconds EDM function to
    /// calculate the number of seconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of seconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffSeconds")]
    public static int? DiffSeconds(DateTimeOffset? timeValue1, DateTimeOffset? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffSeconds EDM function to
    /// calculate the number of seconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of seconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffSeconds")]
    public static int? DiffSeconds(DateTime? timeValue1, DateTime? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffSeconds EDM function to
    /// calculate the number of seconds between two time spans.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first time span. </param>
    /// <param name="timeValue2"> The second time span. </param>
    /// <returns> The number of seconds between the first and second time spans. </returns>
    [DbFunction("Edm", "DiffSeconds")]
    public static int? DiffSeconds(TimeSpan? timeValue1, TimeSpan? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMilliseconds EDM function to
    /// calculate the number of milliseconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of milliseconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMilliseconds")]
    public static int? DiffMilliseconds(DateTimeOffset? timeValue1, DateTimeOffset? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMilliseconds EDM function to
    /// calculate the number of milliseconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of milliseconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMilliseconds")]
    public static int? DiffMilliseconds(DateTime? timeValue1, DateTime? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMilliseconds EDM function to
    /// calculate the number of milliseconds between two time spans.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first time span. </param>
    /// <param name="timeValue2"> The second time span. </param>
    /// <returns> The number of milliseconds between the first and second time spans. </returns>
    [DbFunction("Edm", "DiffMilliseconds")]
    public static int? DiffMilliseconds(TimeSpan? timeValue1, TimeSpan? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMicroseconds EDM function to
    /// calculate the number of microseconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of microseconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMicroseconds")]
    public static int? DiffMicroseconds(DateTimeOffset? timeValue1, DateTimeOffset? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMicroseconds EDM function to
    /// calculate the number of microseconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of microseconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffMicroseconds")]
    public static int? DiffMicroseconds(DateTime? timeValue1, DateTime? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffMicroseconds EDM function to
    /// calculate the number of microseconds between two time spans.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first time span. </param>
    /// <param name="timeValue2"> The second time span. </param>
    /// <returns> The number of microseconds between the first and second time spans. </returns>
    [DbFunction("Edm", "DiffMicroseconds")]
    public static int? DiffMicroseconds(TimeSpan? timeValue1, TimeSpan? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffNanoseconds EDM function to
    /// calculate the number of nanoseconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of nanoseconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffNanoseconds")]
    public static int? DiffNanoseconds(DateTimeOffset? timeValue1, DateTimeOffset? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffNanoseconds EDM function to
    /// calculate the number of nanoseconds between two date/times.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first date/time. </param>
    /// <param name="timeValue2"> The second date/time. </param>
    /// <returns> The number of nanoseconds between the first and second date/times. </returns>
    [DbFunction("Edm", "DiffNanoseconds")]
    public static int? DiffNanoseconds(DateTime? timeValue1, DateTime? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical DiffNanoseconds EDM function to
    /// calculate the number of nanoseconds between two time spans.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="timeValue1"> The first time span. </param>
    /// <param name="timeValue2"> The second time span. </param>
    /// <returns> The number of nanoseconds between the first and second time spans. </returns>
    [DbFunction("Edm", "DiffNanoseconds")]
    public static int? DiffNanoseconds(TimeSpan? timeValue1, TimeSpan? timeValue2) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Truncate EDM function to
    /// truncate the given value to the number of specified digits.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="value"> The value to truncate. </param>
    /// <param name="digits"> The number of digits to preserve. </param>
    /// <returns> The truncated value. </returns>
    [DbFunction("Edm", "Truncate")]
    public static double? Truncate(double? value, int? digits) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Truncate EDM function to
    /// truncate the given value to the number of specified digits.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="value"> The value to truncate. </param>
    /// <param name="digits"> The number of digits to preserve. </param>
    /// <returns> The truncated value. </returns>
    [DbFunction("Edm", "Truncate")]
    public static Decimal? Truncate(Decimal? value, int? digits) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Like EDM operator to match an expression.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="searchString"> The string to search. </param>
    /// <param name="likeExpression"> The expression to match against. </param>
    /// <returns> True if the searched string matches the expression; otherwise false. </returns>
    public static bool Like(string searchString, string likeExpression) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method invokes the canonical Like EDM operator to match an expression.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function is translated to a corresponding function in the database.
    /// </remarks>
    /// <param name="searchString"> The string to search. </param>
    /// <param name="likeExpression"> The expression to match against. </param>
    /// <param name="escapeCharacter"> The string to escape special characters with, must only be a single character. </param>
    /// <returns> True if the searched string matches the expression; otherwise false. </returns>
    public static bool Like(string searchString, string likeExpression, string escapeCharacter) => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method acts as an operator that ensures the input
    /// is treated as a Unicode string.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function impacts the way the LINQ query is translated to a query that can be run in the database.
    /// </remarks>
    /// <param name="value"> The input string. </param>
    /// <returns> The input string treated as a Unicode string. </returns>
    public static string AsUnicode(string value) => value;

    /// <summary>
    /// When used as part of a LINQ to Entities query, this method acts as an operator that ensures the input
    /// is treated as a non-Unicode string.
    /// </summary>
    /// <remarks>
    /// You cannot call this function directly. This function can only appear within a LINQ to Entities query.
    /// This function impacts the way the LINQ query is translated to a query that can be run in the database.
    /// </remarks>
    /// <param name="value"> The input string. </param>
    /// <returns> The input string treated as a non-Unicode string. </returns>
    public static string AsNonUnicode(string value) => value;

    private static TOut BootstrapFunction<TIn, TOut>(
      Expression<Func<IEnumerable<TIn>, TOut>> methodExpression,
      IEnumerable<TIn> collection)
    {
      if (collection is IQueryable queryable)
        return queryable.Provider.Execute<TOut>((Expression) Expression.Call(((MethodCallExpression) methodExpression.Body).Method, (Expression) Expression.Constant((object) collection)));
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_DbFunctionDirectCall);
    }
  }
}
