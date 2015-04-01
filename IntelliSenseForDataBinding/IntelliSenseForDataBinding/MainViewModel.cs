using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IntelliSenseForDataBinding
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Greeting = "Hello World";
            Answer = 42;
        }

        private string _Greeting;
        public string Greeting
        {
            get { return _Greeting; }
            set { _Greeting = value; OnPropertyChanged(); }
        }

        private int _Answer;
        public int Answer
        {
            get { return _Answer; }
            set { _Answer = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
