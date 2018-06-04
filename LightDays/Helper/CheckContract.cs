using Windows.Foundation.Metadata;

namespace Days.Helper
{
    public class CheckContract
    {
        public static bool isAPIContractExist(ushort number)
        {
            return ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", number);
        }
    }
}
