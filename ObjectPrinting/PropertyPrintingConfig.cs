using System;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private readonly PrintingConfig<TOwner> printingConfig;
        private readonly string propertyName;

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig)
        {
            this.printingConfig = printingConfig;
        }

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig, string propertyName)
        {
            this.printingConfig = printingConfig;
            this.propertyName = propertyName;
        }

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>
            .PrintingConfig => printingConfig;

        string IPropertyPrintingConfig<TOwner, TPropType>
            .PropertyName => propertyName;

        public PrintingConfig<TOwner> Using(Func<TPropType, string> modeFunc)
        {
            var printingSettings = ((IPrintingConfig<TOwner>) printingConfig)
                .GetPrintingSettings;
            if (propertyName != null)
                printingSettings.SerializationModesForProperties.Add(propertyName, p => modeFunc((TPropType)p));
            else printingSettings.SerializationModesForTypes.Add(typeof(TPropType), p => modeFunc((TPropType)p));
            return printingConfig;
        }
    }

    public interface IPropertyPrintingConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
        string PropertyName { get; }
    }
}