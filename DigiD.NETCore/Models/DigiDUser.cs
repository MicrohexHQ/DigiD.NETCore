using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiD.NETCore.Models
{
    /// <summary>
    /// DigiD betrouwbaarheidsniveau
    /// </summary>
    public enum BetrouwbaarheidsNiveau
    {
        Unspecified = 0,
        /// <summary>
        /// urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport 
        /// </summary>
        Basis = 1,

        /// <summary>
        /// urn:oasis:names:tc:SAML:2.0:ac:classes:MobileTwoFactorContract 
        /// </summary>
        Midden = 2,

        /// <summary>
        /// urn:oasis:names:tc:SAML:2.0:ac:classes:Smartcard
        /// </summary>
        Substantieel = 3,

        /// <summary>
        /// urn:oasis:names:tc:SAML:2.0:ac:classes:SmartcardPKI
        /// </summary>
        Hoog = 4
    }

    public enum GebruikerIDType
    {
        Unspecified = 0,
        BSN = 1,
        SOFI = 2
    }

    /// <summary>
    /// Ingelogde DigiD gebruiker
    /// </summary>
    public class DigiDUser
    {
        /// <summary>
        /// BSN/SOFI nummer
        /// </summary>
        public string GebruikerID { get; set; }

        /// <summary>
        /// Geeft aan of GebruikerID een BSN of SOFI nr betreft
        /// </summary>
        public GebruikerIDType GebruikerIDType { get; set; }

        /// <summary>
        /// Betrouwbaarheidsniveau waar de gebruiker voor heeft gekozen
        /// </summary>
        public BetrouwbaarheidsNiveau BetrouwbaarheidsNiveau { get; set; }

        /// <summary>
        /// IP-adres van de gebruiker
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Inlogdatum bij DigiD
        /// </summary>
        public DateTime InlogDatumUtc { get; set; }

        /// <summary>
        /// T/m wanneer de inlog geldig blijft
        /// </summary>
        public DateTime GeldigTmUtc { get; set; }
    }
}
