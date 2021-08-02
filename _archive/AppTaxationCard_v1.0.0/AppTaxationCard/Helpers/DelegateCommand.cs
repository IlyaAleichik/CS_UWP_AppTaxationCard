//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;

//namespace AppTaxationCard.Helpers
//{
//    class DelegateCommand : ICommand
//    {
//        private SimpleEventHandler handler;
//        private bool idEnabled = true;
//        public event EventHandler CanExecuteChanged;
//        public delegate void SimpleEventHandler();
//        public DelegateCommand(SimpleEventHandler handler)
//        {
//            this.handler = handler;
//        }

//        public bool IsEnabled
//        {
//            get
//            {
//                return this.IsEnabled;
//            }
//        }

//        void ICommand.Execute(object args)
//        {
//            this.handler();
//        }
//        bool ICommand.CanExecute(object args)
//        {
//            return this.IsEnabled;
//        }
//        private void OnCanExecuteChanged()
//        {
//            if(this.CanExecuteChanged != null)
//            {
//                this.CanExecuteChanged(this, EventArgs.Empty);
//            }
//        }

//    }
//}
