using microCommerce.Domain.Directory;
using System.Collections.Generic;

namespace microCommerce.Providers.Directory
{
    public interface ICountryProvider
    {
        Country GetCountryById(int countryId);

        IList<Country> GetAllCountries(bool showHidden = false);

        Country GetCountryByTwoLetterIsoCode(string twoLetterIsoCode);

        void InsertCountry(Country country);

        void UpdateCountry(Country country);

        void DeleteCountry(Country country);
    }
}