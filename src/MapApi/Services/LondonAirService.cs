namespace MapApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Interfaces;
    using MapApiCore.Models;
    using MapApiCore.Models.LondonAir;
    
    public class LondonAirService : IPollutionService
    {
        private Uri _baseUri = new Uri("http://api.erg.kcl.ac.uk");
        private const string dailyUri = "/AirQuality/Daily/MonitoringIndex/Latest/SiteCode={0}/JSON";

        private readonly HttpClient _httpClient;
        private Dictionary<string, string> _sites;
        
        public LondonAirService()
        {
            _httpClient = new HttpClient() {BaseAddress = _baseUri };

            _sites = new Dictionary<string, string>
            {
                { "TD0", "- National Physical Laboratory, Teddington" },
                { "BG3", "Barking and Dagenham - North Street" },
                { "BG1", "Barking and Dagenham - Rush Green" },
                { "BG2", "Barking and Dagenham - Scrattons Farm" },
                { "BN2", "Barnet - Finchley" },
                { "BN3", "Barnet - Strawberry Vale " },
                { "BN1", "Barnet - Tally Ho Corner" },
                { "BX5", "Bexley - Bedonwell " },
                { "BX2", "Bexley - Belvedere" },
                { "BX0", "Bexley - Belvedere FDMS" },
                { "BQ7", "Bexley - Belvedere West" },
                { "BQ8", "Bexley - Belvedere West FDMS" },
                { "BX4", "Bexley - Erith" },
                { "BQ6", "Bexley - Manor Road East Gravimetric" },
                { "BQ5", "Bexley - Manor Road West Gravimetric" },
                { "BX1", "Bexley - Slade Green" },
                { "BX9", "Bexley - Slade Green FDMS" },
                { "BX7", "Bexley - Thames Road North" },
                { "BX6", "Bexley - Thames Road North FDMS" },
                { "BX8", "Bexley - Thames Road South" },
                { "BX3", "Bexley - Thamesmead" },
                { "BQ3", "Bexley 8 - Traffic(E fast)" },
                { "BQ4", "Bexley 8 - Traffic(E slow - nearest BX7)" },
                { "BQ2", "Bexley 8 - Traffic(W fast)" },
                { "BQ1", "Bexley 8 - Traffic(W slow - nearest BX8)" },
                { "BT8", "Brent - ARK Franklin Primary Academy" },
                { "BT3", "Brent - Harlesden" },
                { "BT4", "Brent - Ikea" },
                { "BT2", "Brent - Ikea Car Park" },
                { "BT6", "Brent - John Keble Primary School" },
                { "BT1", "Brent - Kingsbury" },
                { "BT5", "Brent - Neasden Lane" },
                { "BT7", "Brent - St Marys Primary School" },
                { "BY5", "Bromley - Biggin Hill" },
                { "BY7", "Bromley - Harwood Avenue" },
                { "BY1", "Bromley - Rent Office" },
                { "BY4", "Bromley - Tweedy Rd" },
                { "BL0", "Camden - Bloomsbury" },
                { "KX4", "Camden - Coopers Lane" },
                { "CD9", "Camden - Euston Road" },
                { "IM1", "Camden - Holborn (Bee Midtown)" },
                { "CD3", "Camden - Shaftesbury Avenue" },
                { "CD4", "Camden - St Martins College (NOX 1)" },
                { "CD5", "Camden - St Martins College (NOX 2)" },
                { "CD1", "Camden - Swiss Cottage" },
                { "CT4", "City of London - Beech Street" },
                { "CT2", "City of London - Farringdon Street" },
                { "CT1", "City of London - Senator House" },
                { "CT3", "City of London - Sir John Cass School" },
                { "CT8", "City of London - Upper Thames Street" },
                { "CT6", "City of London - Walbrook Wharf" },
                { "CR6", "Croydon - Euston Road" },
                { "CR4", "Croydon - George Street" },
                { "CR5", "Croydon - Norbury" },
                { "CR8", "Croydon - Norbury Manor" },
                { "CR9", "Croydon - Park Lane" },
                { "CR2", "Croydon - Purley Way" },
                { "CR7", "Croydon - Purley Way A23" },
                { "CR3", "Croydon - Thornton Heath" },
                { "CY1", "Crystal Palace - Crystal Palace Parade" },
                { "EA2", "Ealing - Acton Town Hall" },
                { "EG1", "Ealing - Acton Town Hall (Partisol)" },
                { "EA0", "Ealing - Acton Town Hall FDMS" },
                { "EI3", "Ealing - Acton Vale " },
                { "EA9", "Ealing - Court Way" },
                { "EA1", "Ealing - Ealing Town Hall" },
                { "EI0", "Ealing - Greenford" },
                { "EA6", "Ealing - Hanger Lane Gyratory" },
                { "EA8", "Ealing - Horn Lane" },
                { "EI8", "Ealing - Horn Lane TEOM" },
                { "EA7", "Ealing - Southall" },
                { "EI7", "Ealing - Southall FDMS" },
                { "EI2", "Ealing - Southall Railway" },
                { "EI1", "Ealing - Western Avenue" },
                { "EN5", "Enfield - Bowes Primary School" },
                { "EN1", "Enfield - Bush Hill Park" },
                { "EN2", "Enfield - Church Street" },
                { "EN4", "Enfield - Derby Road" },
                { "EN7", "Enfield - Prince of Wales School" },
                { "EN3", "Enfield - Salisbury School" },
                { "GN0", "Greenwich - A206 Burrage Grove" },
                { "GR7", "Greenwich - Blackheath" },
                { "GR4", "Greenwich - Eltham" },
                { "GB6", "Greenwich - Falconwood" },
                { "GB0", "Greenwich - Falconwood FDMS" },
                { "GN4", "Greenwich - Fiveways Sidcup Rd A20" },
                { "GN6", "Greenwich - John Harrison Way" },
                { "GN2", "Greenwich - Millennium Village" },
                { "GN3", "Greenwich - Plumstead High Street" },
                { "GR5", "Greenwich - Trafalgar Road" },
                { "GN5", "Greenwich - Trafalgar Road (Hoskins St)" },
                { "GR9", "Greenwich - Westhorne Avenue" },
                { "GR8", "Greenwich - Woolwich Flyover" },
                { "HK4", "Hackney - Clapton" },
                { "HK6", "Hackney - Old Street" },
                { "HF1", "Hammersmith and Fulham - Broadway" },
                { "HF2", "Hammersmith and Fulham - Brook Green" },
                { "HF3", "Hammersmith and Fulham - Scrubs Lane" },
                { "HF4", "Hammersmith and Fulham - Shepherds Bush" },
                { "HG4", "Haringey  - Priory Park South" },
                { "HG3", "Haringey - Bounds Green" },
                { "HG1", "Haringey - Haringey Town Hall" },
                { "HG2", "Haringey - Priory Park" },
                { "HR2", "Harrow - Pinner Road" },
                { "HR1", "Harrow - Stanmore" },
                { "HV2", "Havering - Harold Hill" },
                { "HV1", "Havering - Rainham" },
                { "HV3", "Havering - Romford" },
                { "LH2", "Heathrow Airport" },
                { "LH0", "Hillingdon - Harlington" },
                { "HI2", "Hillingdon - Hillingdon Hospital" },
                { "HI0", "Hillingdon - Keats Way" },
                { "HI3", "Hillingdon - Oxford Avenue" },
                { "HI1", "Hillingdon - South Ruislip" },
                { "HS3", "Hounslow - Brentford" },
                { "HS1", "Hounslow - Brentford" },
                { "HS5", "Hounslow - Brentford" },
                { "HS4", "Hounslow - Chiswick High Road" },
                { "HS2", "Hounslow - Cranford" },
                { "HS9", "Hounslow - Feltham" },
                { "HS7", "Hounslow - Hatton Cross" },
                { "HS6", "Hounslow - Heston Road" },
                { "HS8", "Hounslow and Ealing - Gunnersbury Avenue" },
                { "IS6", "Islington - Arsenal" },
                { "IS5", "Islington - Duncan Terrace" },
                { "IS4", "Islington - Foxham Gardens" },
                { "IS2", "Islington - Holloway Road" },
                { "IS1", "Islington - Upper Street" },
                { "KC2", "Kensington and Chelsea - Cromwell Road" },
                { "KC5", "Kensington and Chelsea - Earls Court Rd" },
                { "KG2", "Kensington and Chelsea - Green Screen BG" },
                { "KG1", "Kensington and Chelsea - Green Screen RS" },
                { "KC4", "Kensington and Chelsea - Kings Road" },
                { "KC3", "Kensington and Chelsea - Knightsbridge" },
                { "KC1", "Kensington and Chelsea - North Ken" },
                { "KC7", "Kensington and Chelsea - North Ken FDMS" },
                { "KF1", "Kensington and Chelsea - North Ken FIDAS" },
                { "WE0", "Kensington and Chelsea - Pembroke Road" },
                { "KT1", "Kingston - Chessington" },
                { "A30", "Kingston - Kingston Bypass A3" },
                { "KT2", "Kingston - Town Centre" },
                { "KT5", "Kingston Upon Thames - Cromwell Road" },
                { "KT6", "Kingston Upon Thames - Kingston Vale" },
                { "KT3", "Kingston Upon Thames - Sopwith Way" },
                { "KT4", "Kingston Upon Thames - Tolworth Broadway" },
                { "LB5", "Lambeth - Bondway Interchange" },
                { "LB4", "Lambeth - Brixton Road" },
                { "LB1", "Lambeth - Christchurch Road" },
                { "LB3", "Lambeth - Loughborough Junct" },
                { "LB6", "Lambeth - Streatham Green" },
                { "LB2", "Lambeth - Vauxhall Cross" },
                { "LW1", "Lewisham - Catford" },
                { "HP1", "Lewisham - Honor Oak Park" },
                { "LW4", "Lewisham - Loampit Vale" },
                { "LW3", "Lewisham - Mercury Way" },
                { "LW2", "Lewisham - New Cross" },
                { "TD5", "London Teddington Bushy Park " },
                { "ME5", "Merton - Esampler1" },
                { "ME6", "Merton - Esampler2" },
                { "ME2", "Merton - Merton Road" },
                { "ME1", "Merton - Morden Civic Centre" },
                { "ME9", "Merton - Morden Civic Centre 2" },
                { "ME7", "Merton - Willow Lane Industrial Estate" },
                { "NM2", "Newham - Cam Road" },
                { "NM1", "Newham - Tant Avenue" },
                { "NM3", "Newham - Wren Close" },
                { "RB3", "Redbridge - Fullwell Cross" },
                { "RB4", "Redbridge - Gardner Close" },
                { "RB2", "Redbridge - Ilford Broadway" },
                { "RB7", "Redbridge - Ley Street" },
                { "RB1", "Redbridge - Perth Terrace" },
                { "RB5", "Redbridge - South Woodford" },
                { "RI2", "Richmond Upon Thames - Barnes Wetlands" },
                { "RI1", "Richmond Upon Thames - Castelnau" },
                { "RHG", "Richmond Upon Thames - Chertsey Road" },
                { "AZ99", "Sk B Site" },
                { "SK5", "Southwark - A2 Old Kent Road" },
                { "SK6", "Southwark - Elephant and Castle" },
                { "SK7", "Southwark - Heygate" },
                { "SK1", "Southwark - Larcom Street" },
                { "SK2", "Southwark - Old Kent Road" },
                { "SK8", "Southwark - Tower Bridge Road" },
                { "ST8", "Sutton - Beddington Lane " },
                { "ST5", "Sutton - Beddington Lane north" },
                { "ST3", "Sutton - Carshalton" },
                { "ST2", "Sutton - North Cheam" },
                { "ST1", "Sutton - Robin Hood School" },
                { "ST7", "Sutton - Therapia Lane" },
                { "ST4", "Sutton - Wallington" },
                { "ST6", "Sutton - Worcester Park" },
                { "GF7", "Tim Setup test" },
                { "TH3", "Tower Hamlets - Bethnal Green" },
                { "TH4", "Tower Hamlets - Blackwall" },
                { "TH2", "Tower Hamlets - Mile End Road" },
                { "TH6", "Tower Hamlets - Millwall Park" },
                { "TH1", "Tower Hamlets - Poplar" },
                { "TH5", "Tower Hamlets - Victoria Park" },
                { "WL3", "Waltham Forest - Chingford" },
                { "WL4", "Waltham Forest - Crooked Billet" },
                { "WL1", "Waltham Forest - Dawlish Road" },
                { "WL5", "Waltham Forest - Leyton" },
                { "WL2", "Waltham Forest - Mobile" },
                { "WAA", "Wandsworth - Battersea" },
                { "WA1", "Wandsworth - Garratt Lane" },
                { "WA4", "Wandsworth - High Street" },
                { "WAC", "Wandsworth - Lavender Hill (Clapham Jct)" },
                { "WA9", "Wandsworth - Putney" },
                { "WA7", "Wandsworth - Putney High Street" },
                { "WA8", "Wandsworth - Putney High Street Facade" },
                { "WA3", "Wandsworth - Roehampton" },
                { "WA6", "Wandsworth - Tooting" },
                { "WAB", "Wandsworth - Tooting High Street" },
                { "WA2", "Wandsworth - Wandsworth Town Hall" },
                { "BP0", "Westminster - Bridge Place" },
                { "WMA", "Westminster - Buckingham Palace Road" },
                { "WMC", "Westminster - Cavendish Square" },
                { "WM4", "Westminster - Charing Cross Library" },
                { "WM5", "Westminster - Covent Garden" },
                { "GV2", "Westminster - Duke Street (Grosvenor)" },
                { "GV1", "Westminster - Ebury Street (Grosvenor)" },
                { "WM0", "Westminster - Horseferry Road" },
                { "MY1", "Westminster - Marylebone Road" },
                { "MY7", "Westminster - Marylebone Road FDMS" },
                { "WM6", "Westminster - Oxford Street" },
                { "WMZ", "Westminster - Oxford Street 2" },
                { "WMB", "Westminster - Oxford Street East" },
                { "NB1", "Westminster - Strand (Northbank BID)" },
                { "WM8", "Westminster - Victoria" },
                { "WM9", "Westminster - Victoria (Victoria BID)" },
                { "VS1", "Westminster - Victoria Street" }
            };
        }

        public async Task<List<Marker>> GetPollutionDataForAllSites()
        {
            var markers = new List<Marker>();

            foreach (var site in this._sites)
            {
                var requestUri = string.Format(dailyUri, site.Key);
                var pollutionItem = await this.ReadFromRemoteApiAsync(requestUri);

                if (pollutionItem != null)
                {
                    var marker = this.ConvertToMarker(pollutionItem);
                    markers.Add(marker);
                }
            }

            return markers;
        }

        private Marker ConvertToMarker(LondonAirPollution pollutionItem)
        {
            var site = pollutionItem.DailyAirQualityIndex.LocalAuthority.Site;
            var longitude = double.Parse(site.Longitude);
            var latitude = double.Parse(site.Latitude);
            var speciesWithAirQualityIndex = site.Species.FirstOrDefault(s => s.AirQualityIndex != null);
            var airQualityIndexValue = speciesWithAirQualityIndex != null ? int.Parse(speciesWithAirQualityIndex.AirQualityIndex) : 0;

            return new Marker(new Coordinate(longitude, latitude), airQualityIndexValue, site.SiteName);
        }

        private async Task<LondonAirPollution> ReadFromRemoteApiAsync(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var londonAirPollutionEntity = await response.Content.ReadAsAsync<LondonAirPollution>();
                return londonAirPollutionEntity;
            }

            return null;
        }
    }
}