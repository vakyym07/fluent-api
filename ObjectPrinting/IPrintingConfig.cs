namespace ObjectPrinting
{
    public interface IPrintingConfig<TOwner>
    {
        PrintingSettings GetPrintingSettings { get; }
    }
}