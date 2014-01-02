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
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeAdmin).ToString(), Text = "Пополнение админом", Selected = Type == (int)MoneyFee.TypeEnum.ChargeAdmin };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeYandex).ToString(), Text = "Пополнение через Яндекс.деньги", Selected = Type == (int)MoneyFee.TypeEnum.ChargeYandex };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeQiwi).ToString(), Text = "Пополнение через Qiwi", Selected = Type == (int)MoneyFee.TypeEnum.ChargeQiwi };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeWebMoney).ToString(), Text = "Пополнение через WebMoney", Selected = Type == (int)MoneyFee.TypeEnum.ChargeWebMoney };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.ChargeRobokassa).ToString(), Text = "Пополнение через Robokassa", Selected = Type == (int)MoneyFee.TypeEnum.ChargeRobokassa };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawYandex).ToString(), Text = "Вывод на Яндекс.деньги", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawYandex };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawQiwi).ToString(), Text = "Вывод на Qiwi", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawQiwi };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawWebMoney).ToString(), Text = "Вывод на WebMoney", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawWebMoney };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.WithdrawRobokassa).ToString(), Text = "Вывод на Robokassa", Selected = Type == (int)MoneyFee.TypeEnum.WithdrawRobokassa };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserFee).ToString(), Text = "Взнос игрока на турнир", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserFee };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupFee).ToString(), Text = "Взнос команды на турнир", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupFee };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserReturn).ToString(), Text = "Возврат игроком за турнир", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserReturn };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupReturn).ToString(), Text = "Возврат командой за турнир", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupFee };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserCancel).ToString(), Text = "Возврат игроку по отмене турнира", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserCancel };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupCancel).ToString(), Text = "Возврат команде по отмене турнира", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupCancel };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentUserAward).ToString(), Text = "Награждение игрока", Selected = Type == (int)MoneyFee.TypeEnum.TournamentUserAward };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.TournamentGroupAward).ToString(), Text = "Награждение команды", Selected = Type == (int)MoneyFee.TypeEnum.TournamentGroupAward };

                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.UserToUser).ToString(), Text = "При трансфере игроком игроку", Selected = Type == (int)MoneyFee.TypeEnum.UserToUser };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.UserToGroup).ToString(), Text = "При трансфере игроком в группу", Selected = Type == (int)MoneyFee.TypeEnum.UserToGroup };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.GroupToUser).ToString(), Text = "При трансфере из группы игроку", Selected = Type == (int)MoneyFee.TypeEnum.GroupToUser };
                yield return new SelectListItem() { Value = ((int)MoneyFee.TypeEnum.GroupToGroup).ToString(), Text = "При трансфере из группы в группу", Selected = Type == (int)MoneyFee.TypeEnum.GroupToGroup };
            }
        }

        public double PercentFee { get; set; }


    }
}