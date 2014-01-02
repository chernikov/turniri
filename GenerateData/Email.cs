using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData
{
    public class Email
    {
        public static string[] EmailDomains = {"@hotmail.com", "@mail.ru", "@yandex.ru", "@gmail.com", "@mailbox.com", "@yahoo.com" };


        public enum TypeOfGenerate
        {
            NameDotSurname = 0x01, 
            AttrDotSurname = 0x02,
            NameSurname = 0x03,
            NameDotSurnameYear = 0x04,
            AttrDotSurnameYear = 0x05,
            NameSurnameYear = 0x06,
            
        }

        public static string GetRandom(string Name, string Surname)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            var typeOfGenerate = (TypeOfGenerate)((rand.Next() + 1) % 7);

            var year = (rand.Next() % 90) + 1910;

            var domainIndex = rand.Next(EmailDomains.Count());

            var domain = EmailDomains[domainIndex];
            switch (typeOfGenerate)
            {
                case TypeOfGenerate.NameDotSurname :
                    return string.Format("{0}.{1}{2}", Name, Surname, domain);
                case TypeOfGenerate.AttrDotSurname :
                    return string.Format("{0}.{1}{2}", Name.Substring(0,1).ToUpper(), Surname, domain);
                case TypeOfGenerate.NameSurname:
                    return string.Format("{0}{1}{2}", Name, Surname, domain);
                case TypeOfGenerate.NameDotSurnameYear:
                    return string.Format("{0}.{1}{2}{3}", Name, Surname, year, domain);
                case TypeOfGenerate.AttrDotSurnameYear:
                    return string.Format("{0}.{1}{2}{3}", Name.Substring(0, 1).ToUpper(), Surname, year, domain);
                default :
                    return string.Format("{0}{1}{2}{3}", Name, Surname, year, domain);
            }
        }
    }
}
