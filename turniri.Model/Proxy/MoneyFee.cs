using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace turniri.Model
{
    public partial class MoneyFee
    {
        public enum TypeEnum : int
        {
            //Charge
            ChargeAdmin = 0x01,

            ChargeYandex = 0x02,
            ChargeQiwi = 0x03,
            ChargeWebMoney = 0x04,
            ChargeRobokassa = 0x05,

            //Withdraw 

            WithdrawYandex = 0x12,
            WithdrawQiwi = 0x13,
            WithdrawWebMoney = 0x14,
            WithdrawRobokassa = 0x15,

            //Pay
            TournamentUserFee = 0x21,
            TournamentGroupFee = 0x22,

            //Return / возврат 
            TournamentUserReturn = 0x23,
            TournamentGroupReturn = 0x24,

            //Cancel / отмена
            TournamentUserCancel = 0x25,
            TournamentGroupCancel = 0x26,


            //Award
            TournamentUserAward = 0x31,
            TournamentGroupAward = 0x32,

            //Transfer
            UserToUser = 0x40,
            UserToGroup = 0x41,
            GroupToUser = 0x42,
            GroupToGroup = 0x43
        }

        public string TypeName
        {
            get
            {
                switch ((TypeEnum)Type)
                {
                    case TypeEnum.ChargeAdmin:
                        return "Пополнение админом";
                    case TypeEnum.ChargeYandex:
                        return "Пополнение через Яндекс.деньги";
                    case TypeEnum.ChargeQiwi:
                        return "Пополнение через Qiwi";
                    case TypeEnum.ChargeWebMoney:
                        return "Пополнение через WebMoney";
                    case TypeEnum.ChargeRobokassa:
                        return "Пополнение через Robokassa";
                    case TypeEnum.WithdrawYandex:
                        return "Вывод на Яндекс.деньги";
                    case TypeEnum.WithdrawQiwi:
                        return "Вывод на Qiwi";
                    case TypeEnum.WithdrawWebMoney:
                        return "Вывод на WebMoney";
                    case TypeEnum.WithdrawRobokassa:
                        return "Вывод на Robokassa";
                    case TypeEnum.TournamentUserFee:
                        return "Взнос игрока на турнир";
                    case TypeEnum.TournamentGroupFee:
                        return "Взнос команды на турнир";
                    case TypeEnum.TournamentUserReturn:
                        return "Возврат игроком за турнир";
                    case TypeEnum.TournamentGroupReturn:
                        return "Возврат командой за турнир";
                    case TypeEnum.TournamentUserCancel:
                        return "Возврат игроку по отмене турнира";
                    case TypeEnum.TournamentGroupCancel:
                        return "Возврат команде по отмене турнира";
                    case TypeEnum.TournamentUserAward:
                        return "Награждение игрока";
                    case TypeEnum.TournamentGroupAward:
                        return "Награждение команды";
                    case TypeEnum.UserToUser:
                        return "При трансфере игроком игроку";
                    case TypeEnum.UserToGroup:
                        return "При трансфере игроком в группу";
                    case TypeEnum.GroupToUser:
                        return "При трансфере из группы игроку";
                    case TypeEnum.GroupToGroup:
                        return "При трансфере из группы в группу";
                }
                return string.Empty;
            }
        }
    }
}