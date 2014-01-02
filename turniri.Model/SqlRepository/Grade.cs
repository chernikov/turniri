using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace turniri.Model
{

    public partial class SqlRepository
    {
        public bool SetGrade(int senderID, int receiverID, int grade)
        {
            var exist =
                Db.Grades.FirstOrDefault(
                    p => p.SenderID == senderID && p.ReceiverID == receiverID);

            if (exist != null)
            {
                exist.Grade1 = grade;
                Db.Grades.Context.SubmitChanges();
                RecalculateGrades(receiverID);
                return true;
            }
            var newGrade = new Grade
            {
                SenderID = senderID,
                ReceiverID = receiverID,
                Grade1 = grade
            };
            Db.Grades.InsertOnSubmit(newGrade);
            Db.Grades.Context.SubmitChanges();
            RecalculateGrades(receiverID);
            return true;
        }

        private void RecalculateGrades(int userID)
        {
            var user = Db.Users.FirstOrDefault(p => p.ID == userID);
            if (user != null)
            {
                var gradePlus = user.Grades.Count(p => p.Grade1 == 1);
                user.CountPlus = gradePlus;

                var gradeMinus = user.Grades.Count(p => p.Grade1 == -1);
                user.CountMinus = gradeMinus;
                Db.Users.Context.SubmitChanges();
            }
        }
    }
}