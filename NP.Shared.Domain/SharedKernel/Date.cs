using NP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel
{
    public abstract class Date : SeedWork.ValueObject
    {
        public DateTime? Value { get; }
        public string? PersianDate => Value.ToPersianDate();

        protected Date() : base()
        {
        }
        protected Date(DateTime? value) : this()
        {
            if (value is not null)
            {
                value = value.Value.Date;
            }

            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
        public override string ToString()
        {
            if (Value is null)
            {
                return "";
            }

            string result = Value.Value.ToString("yyyy/MM/dd");

            return result;
        }

        public static bool operator <(Date left, Date right)
        {
            return left.Value < right.Value;
        }
        public static bool operator <=(Date left, Date right)
        {
            return left.Value <= right.Value;
        }
        public static bool operator >(Date left, Date right)
        {
            return left.Value > right.Value;
        }
        public static bool operator >=(Date left, Date right)
        {
            return left.Value >= right.Value;
        }

    }
}
