﻿namespace twentySix.NeuralStock.Main
{
    using DevExpress.Mvvm;
    using DevExpress.Mvvm.DataAnnotations;

    using twentySix.NeuralStock.Common;

    [POCOViewModel]
    public class ShellViewModel : ComposedViewModelBase
    {
        protected ShellViewModel()
        {
        }

        public bool IsBusy
        {
            get { return GetProperty(() => IsBusy); }
            set { SetProperty(() => IsBusy, value); }
        }

        protected virtual INavigationService NavigationService => null;

        public void NavigateTo(string view)
        {
            NavigationService.Navigate(view, null, this);
        }
    }
}