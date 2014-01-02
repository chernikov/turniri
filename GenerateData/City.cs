using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateData
{
    public class City
    {
        public static string[] CitiesArr = { "Abu Dhabi", "Abuja", "Accra", "Adamstown",
"Addis Ababa", "Algiers", "Alofi", "Amman", "Amsterdam", "Andorra la Vella",
"Ankara", "Antananarivo", "Apia", "Ashgabat", "Asmara", "Astana",
"Asuncion", "Athens", "Avarua", "Baghdad", "Baku", "Bamako",
"Bandar Seri Begawan", "Bangkok", "Bangui", "Banjul", "Basseterre", "Beijing",
"Beirut", "Belfast", "Belgrade", "Belmopan", "Berlin", "Bern",
"Bishkek", "Bissau", "Bogota", "Brasilia", "Bratislava", "Brazzaville",
"Bridgetown", "Brussels", "Bucharest", "Budapest", "Buenos Aires", "Bujumbura",
"Cairo", "Canberra", "Caracas", "Cardiff", "Castries", "Cayenne",
"Charlotte Amalie", "Chisinau", "Cockburn Town", "Conakry", "Copenhagen", "Dakar",
"Damascus", "Dhaka", "Dili", "Djibouti", "Dodoma", "Dar es Salaam",
"Doha", "Douglas", "Dublin", "Dushanbe", "Edinburgh", "Edinburgh of the Seven Seas",
"El Aaiun", "Episkopi Cantonment", "Flying Fish Cove", "Freetown", "Funafuti", "Gaborone",
"George Town", "Georgetown", "Georgetown", "Gibraltar", "Grytviken", "Guatemala City",
"Gustavia", "Hagatna", "Hamilton", "Hanga Roa", "Hanoi", "Harare",
"Hargeisa", "Havana", "Helsinki", "Honiara", "Islamabad", "Jakarta",
"Jamestown", "Jerusalem", "Juba", "Kabul", "Kampala", "Kathmandu",
"Khartoum", "Kiev", "Kigali", "Kingston", "Kingston", "Kingstown",
"Kinshasa", "Kuala Lumpur", "Kuwait City", "Libreville", "Lilongwe", "Lima",
"Lisbon", "Ljubljana", "Lome", "London", "Luanda", "Lusaka",
"Luxembourg", "Madrid", "Majuro", "Malabo", "Male", "Managua",
"Manama", "Manila", "Maputo", "Marigot", "Maseru", "Mata-Utu",
"Mbabane", "Melekeok", "Mexico City", "Minsk", "Mogadishu", "Monaco",
"Monrovia", "Montevideo", "Moroni", "Moscow", "Muscat", "N'Djamena",
"Nairobi", "Nassau", "Naypyidaw", "New Delhi", "Niamey", "Nicosia",
"Nicosia", "Nouakchott", "Noumea", "Nuuk", "Oranjestad", "Oslo",
"Ottawa", "Ouagadougou", "Pago Pago", "Palikir", "Panama City", "Papeete",
"Paramaribo", "Paris", "Philipsburg", "Phnom Penh", "Plymouth", "Podgorica",
"Port Louis", "Port Moresby", "Port of Spain", "Port Vila", "Port-au-Prince", "Porto-Novo",
"Prague", "Praia", "Cape Town", "Pristina", "Pyongyang", "Quito",
"Rabat", "Reykjavik", "Riga", "Riyadh", "Road Town", "Rome",
"Roseau", "Saipan", "San Jose", "San Juan", "San Marino", "San Salvador",
"Sanaa", "Santiago", "Santo Domingo", "Sarajevo", "Seoul", "Singapore",
"Skopje", "Sofia", "St. George's", "St. Helier", "St. John's", "St. Peter Port",
"St. Pierre", "Stanley", "Stepanakert", "Stockholm", "Sucre", "La Paz",
"Sukhumi", "Suva", "Sao Tome", "Taipei", "Tallinn", "Tarawa",
"Tashkent", "Tbilisi", "Tegucigalpa", "Tehran", "The Valley", "Thimphu",
"Tirana", "Tiraspol", "Tokyo", "Tripoli", "Tskhinvali", "Tunis",
"Torshavn", "Ulan Bator", "Vaduz", "Valletta", "Vatican City", "Victoria",
"Vienna", "Vientiane", "Vilnius", "Warsaw", "Washington, D.C.", "Wellington",
"West Island", "Willemstad", "Windhoek", "Yamoussoukro", "Yaounde", "Yerevan",
"Zagreb" };

        public static string GetRandom()
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            var index = rand.Next(CitiesArr.Count());
            return CitiesArr[index];
        }
    }
}
