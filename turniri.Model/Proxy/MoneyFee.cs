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

            //Return / ������� 
            TournamentUserReturn = 0x23,
            TournamentGroupReturn = 0x24,

            //Cancel / ������
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
                        return "���������� �������";
                    case TypeEnum.ChargeYandex:
                        return "���������� ����� ������.������";
                    case TypeEnum.ChargeQiwi:
                        return "���������� ����� Qiwi";
                    case TypeEnum.ChargeWebMoney:
                        return "���������� ����� WebMoney";
                    case TypeEnum.ChargeRobokassa:
                        return "���������� ����� Robokassa";
                    case TypeEnum.WithdrawYandex:
                        return "����� �� ������.������";
                    case TypeEnum.WithdrawQiwi:
                        return "����� �� Qiwi";
                    case TypeEnum.WithdrawWebMoney:
                        return "����� �� WebMoney";
                    case TypeEnum.WithdrawRobokassa:
                        return "����� �� Robokassa";
                    case TypeEnum.TournamentUserFee:
                        return "����� ������ �� ������";
                    case TypeEnum.TournamentGroupFee:
                        return "����� ������� �� ������";
                    case TypeEnum.TournamentUserReturn:
                        return "������� ������� �� ������";
                    case TypeEnum.TournamentGroupReturn:
                        return "������� �������� �� ������";
                    case TypeEnum.TournamentUserCancel:
                        return "������� ������ �� ������ �������";
                    case TypeEnum.TournamentGroupCancel:
                        return "������� ������� �� ������ �������";
                    case TypeEnum.TournamentUserAward:
                        return "����������� ������";
                    case TypeEnum.TournamentGroupAward:
                        return "����������� �������";
                    case TypeEnum.UserToUser:
                        return "��� ��������� ������� ������";
                    case TypeEnum.UserToGroup:
                        return "��� ��������� ������� � ������";
                    case TypeEnum.GroupToUser:
                        return "��� ��������� �� ������ ������";
                    case TypeEnum.GroupToGroup:
                        return "��� ��������� �� ������ � ������";
                }
                return string.Empty;
            }
        }
    }
}