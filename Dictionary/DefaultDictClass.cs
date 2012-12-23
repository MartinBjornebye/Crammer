using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MB.Crammer
{
    public static class DefaultDictClass
    {

        public static string DictionaryDefault =
                            "<?xml version='1.0' encoding='utf-8'?>" +
                            "<Root>" +
                            "<Header>" +
                            "<Title>Worlds Capitals</Title>" +
                            "<Fonts>" +
                            "<a Name='Times New Roman' Size='27.75' Color='-16777216' Bold='True' />" +
                            "<b Name='Verdana' Size='21.75' Color='-16744448' Bold='True' />" +
                            "</Fonts>" +
                            "</Header>" +
                            "<Entries>" +
                            "<r a=\"England\" b=\"London\" s=\"634308034522031359\" on=\"1\" />" +
                            "<r a=\"Thailand\" b=\"Bangkok\" s=\"634308034522031273\" on=\"1\" />" +
                            "<r a=\"Netherlands\" b=\"Amsterdam\" s=\"634308034522031258\" on=\"1\" />" +
                            "<r a=\"Germany\" b=\"Berlin\" s=\"634308034522031282\" on=\"1\" />" +
                            "<r a=\"Switzerland\" b=\"Bern\" s=\"634308034522031283\" on=\"1\" />" +
                            "<r a=\"New Zealand\" b=\"Wellington\" s=\"634308034522031482\" on=\"1\" />" +
                            "<r a=\"Australia\" b=\"Canberra\" s=\"634308034522031297\" on=\"1\" />" +
                            "<r a=\"Nigeria\" b=\"Abuja\" s=\"634308034522031251\" on=\"1\" />" +
                            "<r a=\"Ghana\" b=\"Accra\" s=\"634308034522031252\" on=\"1\" />" +
                            "<r a=\"Pitcairn Islands\" b=\"Adamstown\" s=\"634308034522031253\" on=\"1\" />" +
                            "<r a=\"Ethiopia\" b=\"Addis Ababa\" s=\"634308034522031254\" on=\"1\" />" +
                            "<r a=\"Algeria\" b=\"Algiers\" s=\"634308034522031255\" on=\"1\" />" +
                            "<r a=\"Niue\" b=\"Alofi\" s=\"634308034522031256\" on=\"1\" />" +
                            "<r a=\"Jordan\" b=\"Amman\" s=\"634308034522031257\" on=\"1\" />" +
                            "<r a=\"Andorra\" b=\"Andorra la Vella\" s=\"634308034522031259\" on=\"1\" />" +
                            "<r a=\"Turkey\" b=\"Ankara\" s=\"634308034522031260\" on=\"1\" />" +
                            "<r a=\"Madagascar\" b=\"Antananarivo\" s=\"634308034522031261\" on=\"1\" />" +
                            "<r a=\"Samoa\" b=\"Apia\" s=\"634308034522031262\" on=\"1\" />" +
                            "<r a=\"Turkmenistan\" b=\"Ashgabat\" s=\"634308034522031263\" on=\"1\" />" +
                            "<r a=\"Eritrea\" b=\"Asmara\" s=\"634308034522031264\" on=\"1\" />" +
                            "<r a=\"Kazakhstan\" b=\"Astana\" s=\"634308034522031265\" on=\"1\" />" +
                            "<r a=\"Paraguay\" b=\"Asunción\" s=\"634308034522031266\" on=\"1\" />" +
                            "<r a=\"Greece\" b=\"Athens\" s=\"634308034522031267\" on=\"1\" />" +
                            "<r a=\"Cook Islands\" b=\"Avarua\" s=\"634308034522031268\" on=\"1\" />" +
                            "<r a=\"Iraq\" b=\"Baghdad\" s=\"634308034522031269\" on=\"1\" />" +
                            "<r a=\"Azerbaijan\" b=\"Baku\" s=\"634308034522031270\" on=\"1\" />" +
                            "<r a=\"Mali\" b=\"Bamako\" s=\"634308034522031271\" on=\"1\" />" +
                            "<r a=\"Brunei\" b=\"Bandar Seri Begawan\" s=\"634308034522031272\" on=\"1\" />" +
                            "<r a=\"Central African Republic\" b=\"Bangui\" s=\"634308034522031274\" on=\"1\" />" +
                            "<r a=\"Gambia\" b=\"Banjul\" s=\"634308034522031275\" on=\"1\" />" +
                            "<r a=\"Saint Kitts and Nevis\" b=\"Basseterre\" s=\"634308034522031276\" on=\"1\" />" +
                            "<r a=\"Peoples Republic of China\" b=\"Beijing\" s=\"634308034522031277\" on=\"1\" />" +
                            "<r a=\"Lebanon\" b=\"Beirut\" s=\"634308034522031278\" on=\"1\" />" +
                            "<r a=\"United Kingdom Northern Ireland\" b=\"Belfast\" s=\"634308034522031279\" on=\"1\" />" +
                            "<r a=\"Serbia\" b=\"Belgrade\" s=\"634308034522031280\" on=\"1\" />" +
                            "<r a=\"Belize\" b=\"Belmopan\" s=\"634308034522031281\" on=\"1\" />" +
                            "<r a=\"Kyrgyzstan\" b=\"Bishkek\" s=\"634308034522031284\" on=\"1\" />" +
                            "<r a=\"Guinea-Bissau\" b=\"Bissau\" s=\"634308034522031285\" on=\"1\" />" +
                            "<r a=\"Colombia\" b=\"Bogotá\" s=\"634308034522031286\" on=\"1\" />" +
                            "<r a=\"Brazil\" b=\"Brasília\" s=\"634308034522031287\" on=\"1\" />" +
                            "<r a=\"Slovakia\" b=\"Bratislava\" s=\"634308034522031288\" on=\"1\" />" +
                            "<r a=\"Republic of the Congo\" b=\"Brazzaville\" s=\"634308034522031289\" on=\"1\" />" +
                            "<r a=\"Barbados\" b=\"Bridgetown\" s=\"634308034522031290\" on=\"1\" />" +
                            "<r a=\"Belgium\" b=\"Brussels\" s=\"634308034522031291\" on=\"1\" />" +
                            "<r a=\"Romania\" b=\"Bucharest\" s=\"634308034522031292\" on=\"1\" />" +
                            "<r a=\"Hungary\" b=\"Budapest\" s=\"634308034522031293\" on=\"1\" />" +
                            "<r a=\"Argentina\" b=\"Buenos Aires\" s=\"634308034522031294\" on=\"1\" />" +
                            "<r a=\"Burundi\" b=\"Bujumbura\" s=\"634308034522031295\" on=\"1\" />" +
                            "<r a=\"Egypt\" b=\"Cairo\" s=\"634308034522031296\" on=\"1\" />" +
                            "<r a=\"Venezuela\" b=\"Caracas\" s=\"634308034522031298\" on=\"1\" />" +
                            "<r a=\"Wales\" b=\"Cardiff\" s=\"634308034522031299\" on=\"1\" />" +
                            "<r a=\"Saint Lucia\" b=\"Castries\" s=\"634308034522031400\" on=\"1\" />" +
                            "<r a=\"United States Virgin Islands\" b=\"Charlotte Amalie\" s=\"634308034522031301\" on=\"1\" />" +
                            "<r a=\"Moldova\" b=\"Chisinau\" s=\"634308034522031302\" on=\"1\" />" +
                            "<r a=\"Turks and Caicos Islands\" b=\"Cockburn Town\" s=\"634308034522031303\" on=\"1\" />" +
                            "<r a=\"Guinea\" b=\"Conakry\" s=\"634308034522031304\" on=\"1\" />" +
                            "<r a=\"Denmark\" b=\"Copenhagen\" s=\"634308034522031305\" on=\"1\" />" +
                            "<r a=\"Senegal\" b=\"Dakar\" s=\"634308034522031306\" on=\"1\" />" +
                            "<r a=\"Syria\" b=\"Damascus\" s=\"634308034522031307\" on=\"1\" />" +
                            "<r a=\"Bangladesh\" b=\"Dhaka\" s=\"634308034522031308\" on=\"1\" />" +
                            "<r a=\"East Timor (Timor-Leste)\" b=\"Dili\" s=\"634308034522031309\" on=\"1\" />" +
                            "<r a=\"Djibouti\" b=\"Djibouti\" s=\"634308034522031310\" on=\"1\" />" +
                            "<r a=\"Tanzania\" b=\"Dodoma\" s=\"634308034522031311\" on=\"1\" />" +
                            "<r a=\"Qatar\" b=\"Doha\" s=\"634308034522031312\" on=\"1\" />" +
                            "<r a=\"Isle of Man\" b=\"Douglas\" s=\"634308034522031313\" on=\"1\" />" +
                            "<r a=\"Ireland\" b=\"Dublin\" s=\"634308034522031314\" on=\"1\" />" +
                            "<r a=\"Tajikistan\" b=\"Dushanbe\" s=\"634308034522031315\" on=\"1\" />" +
                            "<r a=\"Scotland\" b=\"Edinburgh\" s=\"634308034522031316\" on=\"1\" />" +
                            "<r a=\"United Kingdom Akrotiri and Dhekelia\" b=\"Episkopi Cantonment\" s=\"634308034522031317\" on=\"1\" />" +
                            "<r a=\"Sierra Leone\" b=\"Freetown\" s=\"634308034522031318\" on=\"1\" />" +
                            "<r a=\"Tuvalu\" b=\"Funafuti\" s=\"634308034522031319\" on=\"1\" />" +
                            "<r a=\"Botswana\" b=\"Gaborone\" s=\"634308034522031320\" on=\"1\" />" +
                            "<r a=\"Cayman Islands\" b=\"George Town\" s=\"634308034522031312\" on=\"1\" />" +
                            "<r a=\"Guyana\" b=\"Georgetown\" s=\"634308034522031322\" on=\"1\" />" +
                            "<r a=\"Gibraltar\" b=\"Gibraltar\" s=\"634308034522031323\" on=\"1\" />" +
                            "<r a=\"South Georgia and the South Sandwich Islands\" b=\"Grytviken\" s=\"1\" on=\"1\" />" +
                            "<r a=\"Guatemala\" b=\"Guatemala City\" s=\"2\" on=\"1\" />" +
                            "<r a=\"Saint Barthelemy\" b=\"Gustavia\" s=\"3\" on=\"1\" />" +
                            "<r a=\"Guam\" b=\"Hagåtña\" s=\"4\" on=\"1\" />" +
                            "<r a=\"Bermuda\" b=\"Hamilton\" s=\"5\" on=\"1\" />" +
                            "<r a=\"Vietnam\" b=\"Hanoi\" s=\"6\" on=\"1\" />" +
                            "<r a=\"Zimbabwe\" b=\"Harare\" s=\"7\" on=\"1\" />" +
                            "<r a=\"Somaliland\" b=\"Hargeisa\" s=\"8\" on=\"1\" />" +
                            "<r a=\"Cuba\" b=\"Havana\" s=\"9\" on=\"1\" />" +
                            "<r a=\"Finland\" b=\"Helsinki\" s=\"10\" on=\"1\" />" +
                            "<r a=\"Solomon Islands\" b=\"Honiara\" s=\"11\" on=\"1\" />" +
                            "<r a=\"Pakistan\" b=\"Islamabad\" s=\"12\" on=\"1\" />" +
                            "<r a=\"Indonesia\" b=\"Jakarta\" s=\"13\" on=\"1\" />" +
                            "<r a=\"Saint Helena\" b=\"Jamestown\" s=\"14\" on=\"1\" />" +
                            "<r a=\"Israel\" b=\"Jerusalem\" s=\"15\" on=\"1\" />" +
                            "<r a=\"Afghanistan\" b=\"Kabul\" s=\"16\" on=\"1\" />" +
                            "<r a=\"Uganda\" b=\"Kampala\" s=\"17\" on=\"1\" />" +
                            "<r a=\"Nepal\" b=\"Kathmandu\" s=\"18\" on=\"1\" />" +
                            "<r a=\"Sudan\" b=\"Khartoum\" s=\"19\" on=\"1\" />" +
                            "<r a=\"Ukraine\" b=\"Kiev (Kyiv)\" s=\"20\" on=\"1\" />" +
                            "<r a=\"Rwanda\" b=\"Kigali\" s=\"21\" on=\"1\" />" +
                            "<r a=\"Jamaica\" b=\"Kingston\" s=\"22\" on=\"1\" />" +
                            "<r a=\"Norfolk Island\" b=\"Kingston\" s=\"23\" on=\"1\" />" +
                            "<r a=\"Saint Vincent and the Grenadines\" b=\"Kingstown\" s=\"24\" on=\"1\" />" +
                            "<r a=\"Democratic Republic of the Congo\" b=\"Kinshasa\" s=\"25\" on=\"1\" />" +
                            "<r a=\"Malaysia\" b=\"Kuala Lumpur\" s=\"26\" on=\"1\" />" +
                            "<r a=\"Kuwait\" b=\"Kuwait City\" s=\"27\" on=\"1\" />" +
                            "<r a=\"Bolivia\" b=\"La Paz\" s=\"28\" on=\"1\" />" +
                            "<r a=\"Western Sahara\" b=\"Laâyoune\" s=\"29\" on=\"1\" />" +
                            "<r a=\"Gabon\" b=\"Libreville\" s=\"30\" on=\"1\" />" +
                            "<r a=\"Malawi\" b=\"Lilongwe\" s=\"31\" on=\"1\" />" +
                            "<r a=\"Peru\" b=\"Lima\" s=\"32\" on=\"1\" />" +
                            "<r a=\"Portugal\" b=\"Lisbon\" s=\"33\" on=\"1\" />" +
                            "<r a=\"Slovenia\" b=\"Ljubljana\" s=\"34\" on=\"1\" />" +
                            "<r a=\"Togo\" b=\"Lomé\" s=\"35\" on=\"1\" />" +
                            "<r a=\"Angola\" b=\"Luanda\" s=\"36\" on=\"1\" />" +
                            "<r a=\"Zambia\" b=\"Lusaka\" s=\"37\" on=\"1\" />" +
                            "<r a=\"Luxembourg\" b=\"Luxembourg City\" s=\"38\" on=\"1\" />" +
                            "<r a=\"Spain\" b=\"Madrid\" s=\"39\" on=\"1\" />" +
                            "<r a=\"Marshall Islands\" b=\"Majuro\" s=\"40\" on=\"1\" />" +
                            "<r a=\"Equatorial Guinea\" b=\"Malabo\" s=\"41\" on=\"1\" />" +
                            "<r a=\"Maldives\" b=\"Malé\" s=\"42\" on=\"1\" />" +
                            "<r a=\"Mayotte\" b=\"Mamoudzou\" s=\"43\" on=\"1\" />" +
                            "<r a=\"Nicaragua\" b=\"Managua\" s=\"44\" on=\"1\" />" +
                            "<r a=\"Bahrain\" b=\"Manama\" s=\"45\" on=\"1\" />" +
                            "<r a=\"Philippines\" b=\"Manila\" s=\"46\" on=\"1\" />" +
                            "<r a=\"Mozambique\" b=\"Maputo\" s=\"47\" on=\"1\" />" +
                            "<r a=\"Saint Martin\" b=\"Marigot\" s=\"48\" on=\"1\" />" +
                            "<r a=\"Lesotho\" b=\"Maseru\" s=\"49\" on=\"1\" />" +
                            "<r a=\"Wallis and Futuna\" b=\"Mata-Utu\" s=\"49\" on=\"1\" />" +
                            "<r a=\"Swaziland\" b=\"Mbabane\" s=\"50\" on=\"1\" />" +
                            "<r a=\"Mexico\" b=\"Mexico City\" s=\"51\" on=\"1\" />" +
                            "<r a=\"Belarus\" b=\"Minsk\" s=\"52\" on=\"1\" />" +
                            "<r a=\"Somalia\" b=\"Mogadishu\" s=\"53\" on=\"1\" />" +
                            "<r a=\"Monaco\" b=\"Monaco\" s=\"54\" on=\"1\" />" +
                            "<r a=\"Liberia\" b=\"Monrovia\" s=\"55\" on=\"1\" />" +
                            "<r a=\"Uruguay\" b=\"Montevideo\" s=\"56\" on=\"1\" />" +
                            "<r a=\"Comoros\" b=\"Moroni\" s=\"57\" on=\"1\" />" +
                            "<r a=\"Russia\" b=\"Moscow\" s=\"58\" on=\"1\" />" +
                            "<r a=\"Oman\" b=\"Muscat\" s=\"59\" on=\"1\" />" +
                            "<r a=\"Kenya\" b=\"Nairobi\" s=\"60\" on=\"1\" />" +
                            "<r a=\"Bahamas\" b=\"Nassau\" s=\"61\" on=\"1\" />" +
                            "<r a=\"Myanmar\" b=\"Naypyidaw\" s=\"62\" on=\"1\" />" +
                            "<r a=\"Chad\" b=\"NDjamena\" s=\"63\" on=\"1\" />" +
                            "<r a=\"India\" b=\"New Delhi\" s=\"64\" on=\"1\" />" +
                            "<r a=\"Palau\" b=\"Ngerulmud\" s=\"65\" on=\"1\" />" +
                            "<r a=\"Niger\" b=\"Niamey\" s=\"66\" on=\"1\" />" +
                            "<r a=\"Cyprus\" b=\"Nicosia\" s=\"67\" on=\"1\" />" +
                            "<r a=\"Northern Cyprus\" b=\"Nicosia\" s=\"68\" on=\"1\" />" +
                            "<r a=\"Mauritania\" b=\"Nouakchott\" s=\"69\" on=\"1\" />" +
                            "<r a=\"New Caledonia\" b=\"Nouméa\" s=\"70\" on=\"1\" />" +
                            "<r a=\"Tonga\" b=\"Nuku?alofa\" s=\"71\" on=\"1\" />" +
                            "<r a=\"Greenland\" b=\"Nuuk\" s=\"72\" on=\"1\" />" +
                            "<r a=\"Aruba\" b=\"Oranjestad\" s=\"73\" on=\"1\" />" +
                            "<r a=\"Norway\" b=\"Oslo\" s=\"74\" on=\"1\" />" +
                            "<r a=\"Canada\" b=\"Ottawa\" s=\"75\" on=\"1\" />" +
                            "<r a=\"Burkina Faso\" b=\"Ouagadougou\" s=\"76\" on=\"1\" />" +
                            "<r a=\"American Samoa\" b=\"Pago Pago\" s=\"77\" on=\"1\" />" +
                            "<r a=\"Federated States of Micronesia\" b=\"Palikir\" s=\"78\" on=\"1\" />" +
                            "<r a=\"Panama\" b=\"Panama City\" s=\"79\" on=\"1\" />" +
                            "<r a=\"French Polynesia\" b=\"Papeete\" s=\"80\" on=\"1\" />" +
                            "<r a=\"Suriname\" b=\"Paramaribo\" s=\"81\" on=\"1\" />" +
                            "<r a=\"France\" b=\"Paris\" s=\"82\" on=\"1\" />" +
                            "<r a=\"Cambodia\" b=\"Phnom Penh\" s=\"83\" on=\"1\" />" +
                            "<r a=\"Montserrat\" b=\"Plymouth\" s=\"84\" on=\"1\" />" +
                            "<r a=\"Montenegro\" b=\"Podgorica\" s=\"85\" on=\"1\" />" +
                            "<r a=\"Mauritius\" b=\"Port Louis\" s=\"86\" on=\"1\" />" +
                            "<r a=\"Papua New Guinea\" b=\"Port Moresby\" s=\"87\" on=\"1\" />" +
                            "<r a=\"Vanuatu\" b=\"Port Vila\" s=\"88\" on=\"1\" />" +
                            "<r a=\"Haiti\" b=\"Port-au-Prince\" s=\"89\" on=\"1\" />" +
                            "<r a=\"Trinidad and Tobago\" b=\"Port of Spain\" s=\"90\" on=\"1\" />" +
                            "<r a=\"Benin\" b=\"Porto-Novo\" s=\"91\" on=\"1\" />" +
                            "<r a=\"Czech Republic\" b=\"Prague\" s=\"92\" on=\"1\" />" +
                            "<r a=\"Cape Verde\" b=\"Praia\" s=\"93\" on=\"1\" />" +
                            "<r a=\"South Africa\" b=\"Pretoria\" s=\"94\" on=\"1\" />" +
                            "<r a=\"Kosovo\" b=\"Pristina\" s=\"95\" on=\"1\" />" +
                            "<r a=\"North Korea\" b=\"Pyongyang\" s=\"96\" on=\"1\" />" +
                            "<r a=\"Ecuador\" b=\"Quito\" s=\"97\" on=\"1\" />" +
                            "<r a=\"Morocco\" b=\"Rabat\" s=\"98\" on=\"1\" />" +
                            "<r a=\"Palestinian Authority (de facto)\" b=\"Ramallah\" s=\"99\" on=\"1\" />" +
                            "<r a=\"Iceland\" b=\"Reykjavík\" s=\"100\" on=\"1\" />" +
                            "<r a=\"Latvia\" b=\"Riga\" s=\"101\" on=\"1\" />" +
                            "<r a=\"Saudi Arabia\" b=\"Riyadh\" s=\"102\" on=\"1\" />" +
                            "<r a=\"British Virgin Islands\" b=\"Road Town\" s=\"103\" on=\"1\" />" +
                            "<r a=\"Italy\" b=\"Rome\" s=\"104\" on=\"1\" />" +
                            "<r a=\"Dominica\" b=\"Roseau\" s=\"105\" on=\"1\" />" +
                            "<r a=\"Northern Mariana Islands\" b=\"Saipan\" s=\"106\" on=\"1\" />" +
                            "<r a=\"Costa Rica\" b=\"San José\" s=\"107\" on=\"1\" />" +
                            "<r a=\"Puerto Rico\" b=\"San Juan\" s=\"108\" on=\"1\" />" +
                            "<r a=\"San Marino\" b=\"San Marino\" s=\"109\" on=\"1\" />" +
                            "<r a=\"El Salvador\" b=\"San Salvador\" s=\"110\" on=\"1\" />" +
                            "<r a=\"Yemen\" b=\"Sanaá\" s=\"111\" on=\"1\" />" +
                            "<r a=\"Chile\" b=\"Santiago\" s=\"112\" on=\"1\" />" +
                            "<r a=\"Dominican Republic\" b=\"Santo Domingo\" s=\"113\" on=\"1\" />" +
                            "<r a=\"São Tomé and Príncipe\" b=\"São Tomé\" s=\"114\" on=\"1\" />" +
                            "<r a=\"Bosnia and Herzegovina\" b=\"Sarajevo\" s=\"115\" on=\"1\" />" +
                            "<r a=\"South Korea\" b=\"Seoul\" s=\"116\" on=\"1\" />" +
                            "<r a=\"Christmas Island\" b=\"The Settlement\" s=\"117\" on=\"1\" />" +
                            "<r a=\"Singapore\" b=\"Singapore\" s=\"118\" on=\"1\" />" +
                            "<r a=\"Macedonia\" b=\"Skopje\" s=\"119\" on=\"1\" />" +
                            "<r a=\"Bulgaria\" b=\"Sofia\" s=\"120\" on=\"1\" />" +
                            "<r a=\"Kiribati\" b=\"South Tarawa\" s=\"121\" on=\"1\" />" +
                            "<r a=\"Sri Lanka (legislative)\" b=\"Sri Jayawardenepura\" s=\"122\" on=\"1\" />" +
                            "<r a=\"Grenada\" b=\"St. George's\" s=\"123\" on=\"1\" />" +
                            "<r a=\"Jersey\" b=\"St. Helier\" s=\"124\" on=\"1\" />" +
                            "<r a=\"Antigua and Barbuda\" b=\"St. John's\" s=\"125\" on=\"1\" />" +
                            "<r a=\"Guernsey\" b=\"St. Peter Port\" s=\"126\" on=\"1\" />" +
                            "<r a=\"Saint Pierre and Miquelon\" b=\"St. Pierre\" s=\"127\" on=\"1\" />" +
                            "<r a=\"Falkland Islands\" b=\"Stanley\" s=\"128\" on=\"1\" />" +
                            "<r a=\"Sweden\" b=\"Stockholm\" s=\"129\" on=\"1\" />" +
                            "<r a=\"Bolivia\" b=\"Sucre\" s=\"130\" on=\"1\" />" +
                            "<r a=\"Abkhazia\" b=\"Sukhumi\" s=\"131\" on=\"1\" />" +
                            "<r a=\"Fiji\" b=\"Suva\" s=\"132\" on=\"1\" />" +
                            "<r a=\"Republic of China (Taiwan)\" b=\"Taipei\" s=\"133\" on=\"1\" />" +
                            "<r a=\"Estonia\" b=\"Tallinn\" s=\"134\" on=\"1\" />" +
                            "<r a=\"Uzbekistan\" b=\"Tashkent\" s=\"135\" on=\"1\" />" +
                            "<r a=\"Georgia\" b=\"Tbilisi\" s=\"136\" on=\"1\" />" +
                            "<r a=\"Honduras\" b=\"Tegucigalpa\" s=\"137\" on=\"1\" />" +
                            "<r a=\"Iran\" b=\"Tehran\" s=\"138\" on=\"1\" />" +
                            "<r a=\"Bhutan\" b=\"Thimphu\" s=\"139\" on=\"1\" />" +
                            "<r a=\"Albania\" b=\"Tirana\" s=\"140\" on=\"1\" />" +
                            "<r a=\"Transnistria\" b=\"Tiraspol\" s=\"141\" on=\"1\" />" +
                            "<r a=\"Japan\" b=\"Tokyo\" s=\"142\" on=\"1\" />" +
                            "<r a=\"Faroe Islands\" b=\"Tórshavn\" s=\"143\" on=\"1\" />" +
                            "<r a=\"Libya\" b=\"Tripoli\" s=\"144\" on=\"1\" />" +
                            "<r a=\"South Ossetia\" b=\"Tskhinval\" s=\"145\" on=\"1\" />" +
                            "<r a=\"Tunisia\" b=\"Tunis\" s=\"146\" on=\"1\" />" +
                            "<r a=\"Mongolia\" b=\"Ulaanbaatar\" s=\"147\" on=\"1\" />" +
                            "<r a=\"Liechtenstein\" b=\"Vaduz\" s=\"148\" on=\"1\" />" +
                            "<r a=\"Malta\" b=\"Valletta\" s=\"149\" on=\"1\" />" +
                            "<r a=\"Anguilla\" b=\"The Valley\" s=\"150\" on=\"1\" />" +
                            "<r a=\"Vatican City\" b=\"Vatican City\" s=\"151\" on=\"1\" />" +
                            "<r a=\"Seychelles\" b=\"Victoria\" s=\"152\" on=\"1\" />" +
                            "<r a=\"Austria\" b=\"Vienna\" s=\"153\" on=\"1\" />" +
                            "<r a=\"Laos\" b=\"Vientiane\" s=\"154\" on=\"1\" />" +
                            "<r a=\"Lithuania\" b=\"Vilnius\" s=\"155\" on=\"1\" />" +
                            "<r a=\"Poland\" b=\"Warsaw\" s=\"156\" on=\"1\" />" +
                            "<r a=\"United States\" b=\"Washington, D.C.\" s=\"157\" on=\"1\" />" +
                            "<r a=\"Cocos (Keeling) Islands\" b=\"West Island\" s=\"158\" on=\"1\" />" +
                            "<r a=\"Netherlands Antilles\" b=\"Willemstad\" s=\"159\" on=\"1\" />" +
                            "<r a=\"Namibia\" b=\"Windhoek\" s=\"160\" on=\"1\" />" +
                            "<r a=\"Côte d'Ivoire\" b=\"Yamoussoukro\" s=\"161\" on=\"1\" />" +
                            "<r a=\"Cameroon\" b=\"Yaoundé\" s=\"162\" on=\"1\" />" +
                            "<r a=\"Nauru\" b=\"Yaren\" s=\"163\" on=\"1\" />" +
                            "<r a=\"Armenia\" b=\"Yerevan\" s=\"164\" on=\"1\" />" +
                            "<r a=\"Croatia\" b=\"Zagreb\" s=\"165\" on=\"1\" />" +
                            "<r a=\"United Arab Emirates\" b=\"Abu Dhabi\" s=\"166\" on=\"1\" />" +
                            "</Entries>" +
                            "</Root>";


    }
}
