using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SeedWork
{
    //TODO: must be resolved with FluentResult
    public abstract class Enumeration : IComparable
    {
        public int Value { get; private set; }
        public string Name { get; private set; }

        protected Enumeration() : base() => (Value, Name) = (0, "Undefined");
        protected Enumeration(int value, string name) => (Value, Name) = (value, name);

        public override string ToString() => Name;
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }
        public override int GetHashCode() => Value.GetHashCode();
        public int CompareTo(object? other) => Value.CompareTo(((Enumeration?)other)?.Value);

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();
        public static bool TryGetFromValueOrName<TEnumeration>(string valueOrName, out TEnumeration? enumeration)
            where TEnumeration : Enumeration
        {
            return TryParse(item => item.Name == valueOrName, out enumeration) ||
                   int.TryParse(valueOrName, out var value) &&
                   TryParse(item => item.Value == value, out enumeration);
        }
        public static TEnumeration FromValue<TEnumeration>(int? value) where TEnumeration : Enumeration
        {
            var matchingItem = Parse<TEnumeration, int?>(value, "nameOrValue", item => item.Value == value);
            return matchingItem;
        }
        public static TEnumeration FromName<TEnumeration>(string name) where TEnumeration : Enumeration
        {
            var matchingItem = Parse<TEnumeration, string>(name, "name", item => item.Name == name);
            return matchingItem;
        }

        private static bool TryParse<TEnumeration>(
            Func<TEnumeration, bool> predicate,
            out TEnumeration? enumeration)
            where TEnumeration : Enumeration
        {
            enumeration = GetAll<TEnumeration>().FirstOrDefault(predicate);
            return enumeration != null;
        }
        private static TEnumeration Parse<TEnumeration, TIntOrString>(
            TIntOrString nameOrValue,
            string description,
            Func<TEnumeration, bool> predicate)
            where TEnumeration : Enumeration
        {
            var matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                throw new InvalidOperationException(
                    $"'{nameOrValue}' is not a valid {description} in {typeof(TEnumeration)}");
            }

            return matchingItem;
        }
    }
}
