using System;
using System.ComponentModel;
using StackUnderflow.API.Exceptions;
using StackUnderflow.API.Services.Sorting;
using StackUnderflow.Common.Extensions;

namespace StackUnderflow.API.Helpers
{
    public class SortingDirectionDtoConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string source = value as string;
            if (source != null)
            {
                var orderByCriteriaWithDirection = source.Split(' ');
                SortDirectionDto orderByDirection = SortDirectionDto.Asc;     // Default value.
                if (orderByCriteriaWithDirection.Length > 1
                    && !Enum.TryParse(orderByCriteriaWithDirection[1].CapitalizeFirstLetter(),
                    out orderByDirection))
                {
                    throw new InvalidPropertyMappingException($"Unknown ordering direction: {orderByCriteriaWithDirection[1]}");
                }
                return new SortCriteriaDto
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
