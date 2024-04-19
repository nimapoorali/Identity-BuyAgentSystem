using NP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SharedKernel
{
    public class DateTimeP : SeedWork.ValueObject
    {
        public DateTime Value { get; }
        public string? PersianDate => Value.ToPersianDate();
        public static DateTimeP Now { get { return new DateTimeP(DateTime.Now); } }

        private DateTimeP() : base()
        {
        }
        private DateTimeP(DateTime value) : this()
        {
            Value = value;
        }

        public static DateTimeP Create(DateTime value)
        {
            return new DateTimeP(value);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
        public override string ToString()
        {
            return ToString("yyyy/MM/dd HH:mm");
        }
        public string ToString(string format)
        {
            string result = Value.ToString(format);

            return result;
        }

        public static bool operator <(DateTimeP left, DateTimeP right)
        {
            return left.Value < right.Value;
        }
        public static bool operator <=(DateTimeP left, DateTimeP right)
        {
            return left.Value <= right.Value;
        }
        public static bool operator >(DateTimeP left, DateTimeP right)
        {
            return left.Value > right.Value;
        }
        public static bool operator >=(DateTimeP left, DateTimeP right)
        {
            return left.Value >= right.Value;
        }

    }
}
