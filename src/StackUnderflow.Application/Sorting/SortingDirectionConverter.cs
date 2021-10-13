using System;
using System.ComponentModel;
using StackUnderflow.Common.Extensions;
using StackUnderflow.Application.Sorting.Models;

namespace StackUnderflow.Application.Sorting
{
    public class SortingDirectionConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string source = value as string;
            if (source != null)
            {
                var orderByCriteriaWithDirection = source.Split(' ');
                SortDirection orderByDirection = SortDirection.Asc;     // Default value.
                if (orderByCriteriaWithDirection.Length > 1
                    && !Enum.TryParse(orderByCriteriaWithDirection[1].CapitalizeFirstLetter(),
                    out orderByDirection))
                {
                    throw new InvalidPropertyMappingException($"Unknown ordering direction: {orderByCriteriaWithDirection[1]}");
                }
                return new SortCriteria
                {
                    SortByCriteria = orderByCriteriaWithDirection[0],
                    SortDirection = orderByDirection
                };
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
