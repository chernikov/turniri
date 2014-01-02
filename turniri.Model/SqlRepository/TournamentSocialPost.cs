using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<TournamentSocialPost> TournamentSocialPosts
        {
            get
            {
                return Db.TournamentSocialPosts;
            }
        }

        public bool CreateTournamentSocialPost(TournamentSocialPost instance)
        {
            if (instance.ID == 0)
            {
                Db.TournamentSocialPosts.InsertOnSubmit(instance);
                Db.TournamentSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateTournamentSocialPost(TournamentSocialPost instance)
        {
            var cache = Db.TournamentSocialPosts.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
				cache.TournamentID = instance.TournamentID;
				cache.SocialPostID = instance.SocialPostID;
                Db.TournamentSocialPosts.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveTournamentSocialPost(int idTournamentSocialPost)
        {
            TournamentSocialPost instance = Db.TournamentSocialPosts.FirstOrDefault(p => p.ID == idTournamentSocialPost);
            if (instance != null)
            {
                Db.TournamentSocialPosts.DeleteOnSubmit(instance);
                Db.TournamentSocialPosts.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}