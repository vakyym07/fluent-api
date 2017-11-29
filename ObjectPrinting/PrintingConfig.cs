using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner> : IPrintingConfig<TOwner>
    {
        private readonly PrintingSettings printingSettings;

        public PrintingConfig()
        {
            printingSettings = new PrintingSettings();
        }

        PrintingSettings IPrintingConfig<TOwner>.GetPrintingSettings => printingSettings;

        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        public PrintingConfig<TOwner> Exclude<TPropType>()
        {
            printingSettings.ExcludingTypes.Add(typeof(TPropType));
            return this;
        }

        public PrintingConfig<TOwner> Exclude<TPropType>(Expression<Func<TOwner, TPropType>> memberSelector)
        {
            var propertyName = ((MemberExpression) memberSelector.Body).Member.Name;
            printingSettings.ExcludingProperties.Add(propertyName);
            return this;
        }

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>()
        {
            return new PropertyPrintingConfig<TOwner, TPropType>(this);
        }

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>(
            Expression<Func<TOwner, TPropType>> memberSelector)
        {
            var propertyName = ((MemberExpression) memberSelector.Body).Member.Name;
            return new PropertyPrintingConfig<TOwner, TPropType>(this, propertyName);
        }

        private string PrintToString(object obj, int nestingLevel)
        {
            //TODO apply configurations

            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan), typeof(Guid)
            };
            if (finalTypes.Contains(obj.GetType()))
                return SerializeFinalTypes(obj);

            var indentation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties().Where(p => !IsExcluding(p)))
            {
                var trimmedProps = printingSettings.TrimmedProperties; 
                var propertyValue = !trimmedProps.ContainsKey(propertyInfo.Name) ? 
                    propertyInfo.GetValue(obj):
                    trimmedProps[propertyInfo.Name]((string)propertyInfo.GetValue(obj));

                var propsModes = printingSettings.SerializationModesForProperties;
                if (!propsModes.ContainsKey(propertyInfo.Name))
                    sb.Append(indentation + propertyInfo.Name + " = " + PrintToString(propertyValue, nestingLevel + 1));
                else
                    sb.Append(indentation + propsModes[propertyInfo.Name](propertyValue) + Environment.NewLine);
            }
            return sb.ToString();
        }

        private string SerializeFinalTypes(object obj)
        {
            var typesModes = printingSettings.SerializationModesForTypes;
            if (typesModes.ContainsKey(obj.GetType()))
                return typesModes[obj.GetType()](obj) + Environment.NewLine;

            var culture = printingSettings.Culture;
            if (!culture.ContainsKey(obj.GetType()))
                return obj + Environment.NewLine;

            var cultureInfo = culture[obj.GetType()];
            var objToString = obj.GetType().GetMethod("ToString", new[] {typeof(CultureInfo)});
            var objRepr = objToString?.Invoke(obj, new[] { cultureInfo });
            return objRepr + Environment.NewLine;
        }

        private bool IsExcluding(PropertyInfo propertyInfo)
        {
            return printingSettings.ExcludingTypes.Contains(propertyInfo.PropertyType)
                   || printingSettings.ExcludingProperties.Contains(propertyInfo.Name);
        }
    }
}