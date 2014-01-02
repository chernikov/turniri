using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using turniri.Model;
using turniri.Tools;

namespace turniri.Global
{
    public class SearchEngine
    {
        public static IEnumerable<Product> Get(string SearchString, IQueryable<Product> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank;
            foreach (var entry in source)
            {
                rank = 0;
                if (entry.Name != null)
                {
                    rank += Regex.Matches(entry.Name.ToLowerInvariant(), regex).Count;
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<User> Search(string SearchString, IQueryable<User> source, bool all = false)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                rank = FindUser(regex, entry);
                if (all)
                {
                    rank += FindGame(regex, entry);
                    rank += FindTeam(regex, entry);
                    rank += FindGroup(regex, entry);
                }

                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<User> SearchWithGroup(string SearchString, IQueryable<User> source, int gameID)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                rank = FindUser(regex, entry);

                var group = entry.UserGroups.FirstOrDefault(p => p.Status == (int)UserGroup.StatusEnum.Granded && p.Group.GameID == gameID);

                if (group != null)
                {
                    if (group.Group.Name != null)
                    {
                        rank += Regex.Matches(group.Group.Name.ToLowerInvariant(), regex).Count;
                    }
                    if (group.Group.Url != null)
                    {
                        rank += Regex.Matches(group.Group.Url.ToLowerInvariant(), regex).Count;
                    }
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<Game> Search(string SearchString, IQueryable<Game> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                if (entry.Name != null)
                {
                    rank += Regex.Matches(entry.Name.ToLowerInvariant(), regex).Count;
                }
                if (entry.Platform.Name != null)
                {
                    rank += Regex.Matches(entry.Platform.Name.ToLowerInvariant(), regex).Count;
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        private static int FindUser(string regex, User entry)
        {
            int rank = 0;
            if (entry.Login != null)
            {
                rank += Regex.Matches(entry.Login.ToLowerInvariant(), regex).Count;
            }
            if (entry.Email != null)
            {
                rank += Regex.Matches(entry.Email.ToLowerInvariant(), regex).Count;
            }
            if (entry.ICQ != null)
            {
                rank += Regex.Matches(entry.ICQ.ToLowerInvariant(), regex).Count;
            }

            if (entry.Phone != null)
            {
                rank += Regex.Matches(entry.Phone.ToLowerInvariant(), regex).Count;
            }

            if (entry.Skype != null)
            {
                rank += Regex.Matches(entry.Skype.ToLowerInvariant(), regex).Count;
            }
            if (entry.FirstName != null)
            {
                rank += Regex.Matches(entry.FirstName.ToLowerInvariant(), regex).Count;
            }
            if (entry.LastName != null)
            {
                rank += Regex.Matches(entry.LastName.ToLowerInvariant(), regex).Count;
            }
            return rank;
        }

        private static int FindGame(string regex, User entry)
        {
            int rank = 0;
            foreach (var game in entry.UserGames.Select(p => p.Game))
            {
                if (game.Name != null)
                {
                    rank += Regex.Matches(game.Name.ToLowerInvariant(), regex).Count;
                }
            }
            return rank;
        }

        private static int FindTeam(string regex, User entry)
        {
            int rank = 0;
            foreach (var team in entry.UserTeams.Select(p => p.Team))
            {
                if (team.Name != null)
                {
                    rank += Regex.Matches(team.Name.ToLowerInvariant(), regex).Count;
                }
            }
            return rank;
        }

        private static int FindGroup(string regex, User entry)
        {
            int rank = 0;
            foreach (var group in entry.UserGroups.Where(p => p.Status == (int)UserGroup.StatusEnum.Granded).Select(p => p.Group))
            {
                if (group.Name != null)
                {
                    rank += Regex.Matches(group.Name.ToLowerInvariant(), regex).Count;
                }

                if (group.Url != null)
                {
                    rank += Regex.Matches(group.Url.ToLowerInvariant(), regex).Count;
                }
            }
            return rank;
        }

        private static int FindParticipant(string regex, Participant entry)
        {
            int rank = 0;
            if (entry.Name != null)
            {
                rank += Regex.Matches(entry.Name.ToLowerInvariant(), regex).Count;
            }

            if (entry.User != null)
            {
                rank += FindUser(regex, entry.User);
            }
            return rank;
        }

        public static IEnumerable<Tournament> Search(string SearchString, IQueryable<Tournament> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                rank = 0;
                if (entry.Name != null)
                {
                    rank += Regex.Matches(entry.Name.ToLowerInvariant(), regex).Count;
                }
                foreach (var participant in entry.Participants)
                {
                    rank += FindParticipant(regex, participant);
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<MoneyDetail> Search(string SearchString, IQueryable<MoneyDetail> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {

                rank = 0;
                if (string.Compare(SearchString, "сайт", true) == 0 && entry.IsFee)
                {
                    rank++;
                }
                rank += Regex.Matches(entry.Global.ToString().ToLowerInvariant(), regex).Count;
                if (!string.IsNullOrEmpty(entry.Description))
                {
                    rank += Regex.Matches(entry.Description.ToLowerInvariant(), regex).Count;
                }
                if (entry.UserID.HasValue)
                {
                    rank += Regex.Matches(entry.User.Login.ToLowerInvariant(), regex).Count;
                }
                if (entry.GroupID.HasValue)
                {
                    rank += Regex.Matches(entry.Group.Name.ToLowerInvariant(), regex).Count;
                }
                if (entry.TournamentID.HasValue)
                {
                    rank += Regex.Matches(entry.Tournament.Name.ToLowerInvariant(), regex).Count;
                }
                if (entry.SumGold != 0)
                {
                    rank += Regex.Matches(entry.SumGold.ToString().ToLowerInvariant(), regex).Count;
                }
                if (entry.SumWood != 0)
                {
                    rank += Regex.Matches(entry.SumWood.ToString().ToLowerInvariant(), regex).Count;
                }
                if (entry.SumCrystal != 0)
                {
                    rank += Regex.Matches(entry.SumCrystal.ToString().ToLowerInvariant(), regex).Count;
                }
                if (SearchString == "gold" && entry.SumGold != 0)
                {
                    rank++;
                }
                if (SearchString == "wood" && entry.SumWood != 0)
                {
                    rank++;
                }
                if (SearchString == "crystal" && entry.SumCrystal != 0)
                {
                    rank++;
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<Recharge> Search(string SearchString, IQueryable<Recharge> source)
        {
            var term = StringExtension.CleanContent(SearchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            int rank = 0;
            foreach (var entry in source)
            {
                if (!string.IsNullOrEmpty(entry.Description))
                {
                    rank += Regex.Matches(entry.Description.ToLowerInvariant(), regex).Count;
                }
                if (!string.IsNullOrEmpty(entry.User.Login))
                {
                    rank += Regex.Matches(entry.User.Login.ToLowerInvariant(), regex).Count;
                }
                if (entry.Sum != 0)
                {
                    rank += Regex.Matches(entry.Sum.ToString().ToLowerInvariant(), regex).Count;
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }

        public static IEnumerable<Product> Search(string searchString, IQueryable<Product> source)
        {
            var term = StringExtension.CleanContent(searchString.ToLowerInvariant().Trim(), false);
            var terms = term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", terms));

            foreach (var entry in source)
            {
                var rank = 0;
                if (!string.IsNullOrWhiteSpace(entry.Name))
                {
                    rank += Regex.Matches(entry.Name.ToLowerInvariant(), regex).Count;
                }
                if (!string.IsNullOrWhiteSpace(entry.SubTitle))
                {
                    rank += Regex.Matches(entry.SubTitle.ToLowerInvariant(), regex).Count;
                }
                if (!string.IsNullOrWhiteSpace(entry.Description))
                {
                    rank += Regex.Matches(entry.Description.ToLowerInvariant(), regex).Count;
                }
                if (rank > 0)
                {
                    yield return entry;
                }
            }
        }
    }
}