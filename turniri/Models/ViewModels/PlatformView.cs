using System.ComponentModel.DataAnnotations;


namespace turniri.Models.ViewModels
{ 
	public class PlatformView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "������� ������������")]
		public string Name {get; set; }
    }
}