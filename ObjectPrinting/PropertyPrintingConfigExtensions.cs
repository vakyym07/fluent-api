using System;
using System.Globalization;

namespace ObjectPrinting
{
    internal static class PropertyPrintingConfigExtensions
    {
        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig, CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, int>) propertyPrintingConfig).PrintingConfig;
            ((IPrintingConfig<TOwner>)printingConfig).GetPrintingSettings.SetCultureForType(typeof(int), cultureInfo);
            return printingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig, CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, double>)propertyPrintingConfig).PrintingConfig;
            ((IPrintingConfig<TOwner>)printingConfig).GetPrintingSettings.SetCultureForType(typeof(double), cultureInfo);
            return printingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, long> propertyPrintingConfig, CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, long>)propertyPrintingConfig).PrintingConfig;
            ((IPrintingConfig<TOwner>)printingConfig).GetPrintingSettings.SetCultureForType(typeof(long), cultureInfo);
            return printingConfig;
        }

        public static PrintingConfig<TOwner> TrimmedToLength<TOwner>(
            this PropertyPrintingConfig<TOwner, string> propertyPrintingConfig, int length)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, string>)propertyPrintingConfig).PrintingConfig;
            var propertyName = ((IPropertyPrintingConfig<TOwner, string>) propertyPrintingConfig).PropertyName;
            ((IPrintingConfig<TOwner>)printingConfig).GetPrintingSettings
                .TrimmedProperty(propertyName, s => s.Substring(0, length));
            return printingConfig;
        }

        public static string PrintToString<T>(this T obj, Func<PrintingConfig<T>, PrintingConfig<T>> config)
        {
            return config(ObjectPrinter.For<T>()).PrintToString(obj);
        }
    }
}