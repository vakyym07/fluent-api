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
            
            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
            {
                return SerializeFinalTypes(obj);
            }

            var indentation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties().Where(p => !IsExcluding(p)))
            {
                var propertyValue = !printingSettings.TrimmedProperties.ContainsKey(propertyInfo.Name) ? 
                    propertyInfo.GetValue(obj):
                    printingSettings.TrimmedProperties[propertyInfo.Name]((string)propertyInfo.GetValue(obj));
                if (!printingSettings.SerializationModesForProperties.ContainsKey(propertyInfo.Name))
                    sb.Append(indentation + propertyInfo.Name + " = " + PrintToString(propertyValue, nestingLevel + 1));
                else
                    sb.Append(indentation + printingSettings.SerializationModesForProperties[propertyInfo.Name](
                                  propertyValue) + Environment.NewLine);
            }
            return sb.ToString();
        }

        private string SerializeFinalTypes(object obj)
        {
            if (obj == null)
                return "null" + Environment.NewLine;

            if (printingSettings.SerializationModesForTypes.ContainsKey(obj.GetType()))
                return printingSettings.SerializationModesForTypes[obj.GetType()](obj) + Environment.NewLine;

            if (!printingSettings.Culture.ContainsKey(obj.GetType())) return obj + Environment.NewLine;
            var cultureInfo = printingSettings.Culture[obj.GetType()];
            var objRepr = obj.GetType().GetMethod("ToString", new[] { typeof(CultureInfo) })?
                .Invoke(obj, new[] { cultureInfo });
            return objRepr + Environment.NewLine;
        }

        private bool IsExcluding(PropertyInfo propertyInfo)
        {
            return printingSettings.ExcludingTypes.Contains(propertyInfo.PropertyType)
                   || printingSettings.ExcludingProperties.Contains(propertyInfo.Name);
        }
    }
}