/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderApp.ViewModels
{
	public class UploadBookViewModel : INotifyPropertyChanged
	{
		private string _title;
		public string Title
		{
			get { return _title; }
			set { _title = value; OnPropertyChanged(); }
		}

		private byte[] _coverImage; // Change type to byte[]
		public byte[] CoverImage
		{
			get { return _coverImage; }
			set { _coverImage = value; OnPropertyChanged(); }
		}

		private byte[] _bookImage; // Change type to byte[]
		public byte[] BookImage
		{
			get { return _bookImage; }
			set { _bookImage = value; OnPropertyChanged(); }
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; OnPropertyChanged(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
*/