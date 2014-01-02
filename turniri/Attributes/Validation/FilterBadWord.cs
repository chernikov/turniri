using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using turniri.Models.ViewModels;
using turniri.Models.ViewModels.User;


namespace turniri.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterBadWordAttribute : UniqueValidationAttribute
    {
        protected override bool CheckProperty(object obj)
        {
            if (!(obj is MessageView))
                return true;
            var messageView = obj as MessageView;

            var text = messageView.Text.ToLower();

            var words = text.Split(new string[] { " ", "(", ")", "!", ",", ".", "[", "]", "{", "}", ";", ":", "?", "/", "\\", "|", " - " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var word in repository.BannedWords)
            {
                if (words.Contains(word.Word.ToLower()))
                {
                    return false;
                }
                if (word.IsCanBeSubWord && text.IndexOf(word.Word.ToLower()) != -1) 
                {
                    return false;
                }
            }

            return true;
        }
    }
}