using System;
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
		private string _bookImage;
		private string _bookName;

		public event PropertyChangedEventHandler PropertyChanged;

		public string BookImage
		{
			get => _bookImage;
			set
			{
				if (_bookImage != value)
				{
					_bookImage = value;
					OnPropertyChanged();
				}
			}
		}

		public string BookName
		{
			get => _bookName;
			set
			{
				if (_bookName != value)
				{
					_bookName = value;
					OnPropertyChanged();
				}
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
