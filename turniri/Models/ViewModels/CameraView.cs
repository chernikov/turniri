using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Model;
using ManageAttribute;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ninject;

namespace turniri.Models.ViewModels
{
    public class CameraView
    {
        public int _ID
        {
            get { return ID; }
            set { ID = value; }
        }

        public int ID { get; set; }

        public int TournamentID { get; set; }

        public int? MatchID { get; set; }


        private List<Match> Matches
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                var repository = kernel.Get<IRepository>("RequestScoped");
                return repository.Matches.Where(p => p.TournamentID == TournamentID).ToList();
            }
        }

        public IEnumerable<SelectListItem> MatchesSelectList
        {
            get
            {
                yield return new SelectListItem()
                {
                    Value = "",
                    Text = "<<Нет>>",
                    Selected = !MatchID.HasValue
                };
                foreach (var match in Matches)
                {
                    yield return new SelectListItem()
                     {
                         Value = match.ID.ToString(),
                         Text = match.Desc,
                         Selected = match.ID == MatchID
                     };
                };
            }
        }

        [Required(ErrorMessage = "Введите наименование")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Добавьте код трансляции камеры")]
        public string Code { get; set; }

        public bool Enabled { get; set; }
    }
}