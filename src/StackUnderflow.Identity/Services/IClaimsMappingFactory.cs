namespace StackUnderflow.Identity.Services
{
    public interface IClaimsMappingFactory
    {
        BaseClaimsMapper CreateMapper(string providerName);
    }
}
