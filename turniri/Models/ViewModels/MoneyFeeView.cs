using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using turniri.Model;


namespace turniri.Models.ViewModels
{
    public class MoneyFeeView
    {
        public int ID { get; set; }

        public int Type { get; set; }

        public IEnumerable<SelectListItem> SelectListTypes
        {
            get
            {
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeAdmin).ToString(), Text = "���������� �������", Selected = Type == (int)MoneyFee.TypeEnum.ChargeAdmin };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeYandex).ToString(), Text = "���������� ����� ������.������", Selected = Type == (int)MoneyFee.TypeEnum.ChargeYandex };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeQiwi).ToString(), Text = "���������� ����� Qiwi", Selected = Type == (int)MoneyFee.TypeEnum.ChargeQiwi };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeWebMoney).ToString(), Text = "���������� ����� WebMoney", Selected = Type == (int)MoneyFee.TypeEnum.ChargeWebMoney };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeRobokassa).ToString(), Text = "���������� ����� Robokassa", Selected = Type == (int)MoneyFee.TypeEnum.ChargeRobokassa };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawYandex).ToString(), Text = "����� �� ������.������", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawYandex };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawQiwi).ToString(), Text = "����� �� Qiwi", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawQiwi };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawWebMoney).ToString(), Text = "����� �� WebMoney", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawWebMoney };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawRobokassa).ToString(), Text = "����� �� Robokassa", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawRobokassa };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserFee).ToString(), Text = "����� ������ �� ������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserFee };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupFee).ToString(), Text = "����� ������� �� ������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupFee };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserReturn).ToString(), Text = "������� ������� �� ������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserReturn };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupReturn).ToString(), Text = "������� �������� �� ������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupFee };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserCancel).ToString(), Text = "������� ������ �� ������ �������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserCancel };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupCancel).ToString(), Text = "������� ������� �� ������ �������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupCancel };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserAward).ToString(), Text = "����������� ������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserAward };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupAward).ToString(), Text = "����������� �������", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupAward };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.UserToUser).ToString(), Text = "��� ��������� ������� ������", Selected = Type == (int)MoneyFee.TypeEnum.UserToUser };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.UserToGroup).ToString(), Text = "��� ��������� ������� � ������", Selected = Type == (int)MoneyFee.TypeEnum.UserToGroup };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.GroupToUser).ToString(), Text = "��� ��������� �� ������ ������", Selected = Type == (int)MoneyFee.TypeEnum.GroupToUser };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.GroupToGroup).ToString(), Text = "��� ��������� �� ������ � ������", Selected = Type == (int)MoneyFee.TypeEnum.GroupToGroup };
            }
        }

        public double PercentFee { get; set; }


    }
}